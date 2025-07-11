using LiteDB;
using QR_Tracker.Model;
using QR_Tracker.Services;
using QR_Tracker.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

            // 2. 중복 사번 검사
            var existing = _dbService.GetEmployeeByNumber(EmployeeNumber);
            if (existing != null)
            {
                MessageBox.Show("이미 등록된 사번입니다.");
                return;
            }

            // 3. QR 코드 문자열 생성
            string qrCodeText = $"{EmployeeName}|{EmployeeNumber}";

            // 4. Employee 객체 생성
            var newEmployee = new Employee
            {
                Id = ObjectId.NewObjectId(),
                EmployeeName = EmployeeName,
                EmployeeNumber = EmployeeNumber,
                QRCode = qrCodeText
            };

            // 5. DB 저장
            _dbService.AddEmployee(newEmployee);

            // 6. 완료 메시지
            MessageBox.Show("사원이 성공적으로 등록되었습니다.");

            // 7. 입력 초기화
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
