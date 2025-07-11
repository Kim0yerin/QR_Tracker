using OpenCvSharp;

public class CameraModel
{
    private VideoCapture _video;
    private Mat _frame;

    public CameraModel(int cameraIndex = 0)
    {
        _video = new VideoCapture(cameraIndex);
        _frame = new Mat();
    }

    public Mat CaptureFrame()
    {
        _video.Read(_frame);
        Cv2.Flip(_frame, _frame, FlipMode.Y); // 좌우 반전
        if (_frame.Empty())
            return null;
        return _frame.Clone();
    }

    public void Release()
    {
        _frame.Dispose();
        _video.Release();
        _video.Dispose();
    }
}
