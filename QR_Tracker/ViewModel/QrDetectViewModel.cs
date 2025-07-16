using OpenCvSharp;
using QR_Tracker.Model;
using QR_Tracker.ViewModel.BaseViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QR_Tracker.Services;

namespace QR_Tracker.ViewModel
{
    public class QrDetectViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;

        private readonly CameraModel _model; // 클래스 내부에서만 접근, 읽기 전용 필드(readonly field) 생성자에서만 값을 설정

        private WriteableBitmap _imageSource; //WriteableBitmap은 WPF에서 이미지를 실시간으로 생성하고, 수정하고, 화면에 표시하기 위해 사용하는 클래스, 픽셀 값을 직접 수정할 수 있는 쓰기 가능한 비트맵
        public WriteableBitmap ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private byte[] _byteFrame; //이미지나 비디오 프레임 데이터를 저장, WriteableBitmap에 이미지 쓰기용 버퍼
        private CancellationTokenSource _cts; // 비동기 작업을 취소할 수 있도록 토큰(CancellationToken) 을 만들어내는 "취소 컨트롤러"

        public QrDetectViewModel()
        {
            _model = new CameraModel();
            _cts = new CancellationTokenSource();
            ProcessCameraFramesAsync(_cts.Token);
        }

        private async void ProcessCameraFramesAsync(CancellationToken token)
        {
            await Task.Run(() => //CPU 바운드, 백그라운드 스레드 실행	UI를 차단하지 않고, 다른 스레드에서 작업 수행, await를 사용해서 작업이 끝날 때까지 기다리지만, 기다리는 동안에도 UI는 멈추지 않음
            {
                QRCodeDetector QrDecoder = new QRCodeDetector();
                bool bDetectQRCode = false;

                while (!token.IsCancellationRequested) // 주기적으로 취소 요청을 확인해 작업을 중단
                {
                    var matFrame = _model.CaptureFrame();
                    if (matFrame == null)
                        continue;

                    OpenCvSharp.Rect rect = new OpenCvSharp.Rect(220, 140, 200, 200); // 사각형(Rectangle) 정의
                    Cv2.Rectangle(matFrame, rect, Scalar.Blue, 2); // 영상에 사각형 그리기
                    var vRoiFrame = matFrame.SubMat(rect); // 해당 사각형 영역만 잘라낸 부분 영상 반환 (참조 기반)

                    Point2f[] fPoints;
                    string DecodedInfo = QrDecoder.DetectAndDecode(vRoiFrame, out fPoints);

                    if (!string.IsNullOrEmpty(DecodedInfo))
                    {
                        // Time출력
                        DateTime now = DateTime.Now;
                        string formattedDate = now.ToString("yyyyMMddHH:mm:ss ");
                        string Logtext = formattedDate + DecodedInfo;

                        //test용
                        MessageBox.Show(DecodedInfo);

                        if (fPoints != null && fPoints.Length == 4)
                        {
                            // QR 코드 윤곽선 그리기
                            for (int i = 0; i < 4; i++) // 4각형 그리기 0~3
                            {
                                Cv2.Line(vRoiFrame, (int)fPoints[i].X, (int)fPoints[i].Y, (int)fPoints[(i + 1) % 4].X, (int)fPoints[(i + 1) % 4].Y, Scalar.Green, 3);
                            }
                            Cv2.WaitKey(1000);
                            bDetectQRCode = true;
                        }
                    }

                    if (bDetectQRCode)
                    {
                        Cv2.WaitKey(2000);
                        bDetectQRCode = false;

                    }

                    int width = matFrame.Width;
                    int height = matFrame.Height;
                    int channels = matFrame.Channels();
                    int stride = width * channels;

                    if (_byteFrame == null || _byteFrame.Length != matFrame.Total() * matFrame.ElemSize()) // Total: 총 픽셀 수, ElemSize: 픽셀 당 채널 수
                        _byteFrame = new byte[matFrame.Total() * matFrame.ElemSize()];

                    System.Runtime.InteropServices.Marshal.Copy(matFrame.Data, _byteFrame, 0, _byteFrame.Length); // matrix데이터를 byte로 옮기는것

                    matFrame.Dispose(); // 관리가 안되는 메모리이기 때문에 폐기, 메모리 누수 방지

                    Application.Current.Dispatcher.Invoke(() => // Dispatcher: WPF UI 스레드와 작업 스레드 간에 작업을 전달
                    {
                        if (ImageSource == null) // ImageSource가 없으면 생성
                        {
                            PixelFormat pixelFormat = channels == 1 ? PixelFormats.Gray8 :
                                                      channels == 3 ? PixelFormats.Bgr24 :
                                                      PixelFormats.Bgra32;

                            ImageSource = new WriteableBitmap(width, height, 96, 96, pixelFormat, null); // DPI 이미지나 디스플레이에서 1인치당 점의 개수, 일반 화면 96
                        }

                        ImageSource.WritePixels(new Int32Rect(0, 0, width, height), _byteFrame, stride, 0); // _byteFrame 배열에 담긴 프레임 데이터를 WriteableBitmap에 복사
                    });

                    Thread.Sleep(30); // 약 30fps
                }
            });
        }



        public void Cleanup()
        {
            _cts.Cancel();
            _model.Release();
        }
    }
}

