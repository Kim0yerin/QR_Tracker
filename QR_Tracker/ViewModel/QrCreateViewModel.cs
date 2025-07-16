using LiteDB;
using Microsoft.Win32;
using QR_Tracker.Model;
using QR_Tracker.Services;
using QR_Tracker.ViewModel.BaseViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Serilog;

namespace QR_Tracker.ViewModel
{
    internal class QrCreateViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;

        private readonly LiteDbService _dbService = new LiteDbService();

        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set => SetProperty(ref _employeeName, value);  // BaseViewModel에서 구현된 메서드 사용
        }

        private string _employeeNumber;
        public string EmployeeNumber
        {
            get => _employeeNumber;
            set => SetProperty(ref _employeeNumber, value);
        }

        public ICommand CreateCommand { get; }

        public QrCreateViewModel()
        {
            CreateCommand = new RelayCommand(CreateQR);
        }

        private void CreateQR(object obj)
        {
            // 1. 입력값 확인
            if (string.IsNullOrWhiteSpace(EmployeeName) || string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                MessageBox.Show("이름과 사번을 모두 입력하세요.");
                return;
            }

            Log.Information("직원 등록 및 QR 생성 시도 - 이름 : {EmployeeName}, 사번 : {EmployeeNumber}", EmployeeName, EmployeeNumber);

            // 2. 중복 사번 검사
            var existing = _dbService.GetEmployeeByNumber(EmployeeNumber);
            if (existing != null)
            {
                Log.Warning("직원 등록 및 QR 생성 실패 - 이미 등록된 사원입니다.");
                MessageBox.Show("이미 등록된 사번입니다.");
                return;
            }

            // 3. QR 코드 문자열 생성
            string qrCodeText = $"{EmployeeName}{EmployeeNumber}";

            // 4. Employee 객체 생성
            var newEmployee = new Employee
            {
                Id = ObjectId.NewObjectId(),
                EmployeeName = EmployeeName,
                EmployeeNumber = EmployeeNumber,
                QRCode = qrCodeText
            };

            // 5. DB 저장 및 QrCode 저장
            var dialog = new SaveFileDialog
            {
                FileName = qrCodeText,
                DefaultExt = ".png",
                Filter = "PNG Image|*.png",
                InitialDirectory = @"C:\Users\Shin\source\repos\QR_Tracker\QR_Tracker\Model"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;

                try
                {
                    _dbService.AddEmployee(newEmployee);
                    Log.Information("QR 생성 성공 - QR 파일 경로 : {filePath}", filePath);
                    QrEncoder.SaveQrCode(qrCodeText, filePath);
                    MessageBox.Show($"사원이 성공적으로 등록되었습니다.: {qrCodeText}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류 발생: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show($"사원이 등록이 취소되었습니다..");
            }




            // 6. 입력 초기화
            EmployeeName = string.Empty;
            EmployeeNumber = string.Empty;

            var employees = _dbService.GetAllEmployees();
            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.EmployeeName} - {emp.EmployeeNumber} - {emp.QRCode}");
            }

        }

    }
}
