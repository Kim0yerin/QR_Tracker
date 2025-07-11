using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QR_Tracker.Services;
using QR_Tracker.View;
using QR_Tracker.ViewModel.BaseViewModels;


namespace QR_Tracker.ViewModel
{
    public class AdminLoginViewModel : BaseViewModel
    {
        private readonly AdminService _adminService;
        private readonly MainViewModel _mainViewModel;
        public LocalizationManager Loc => LocalizationManager.Instance;

        public string strAdminId { get; set; }

        public ICommand AdminLoginCommand { get; }

        public AdminLoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _adminService = new AdminService();
            AdminLoginCommand = new RelayCommand(AdminLogin);
        }

        private void AdminLogin (object param)
        {
            var passwordBox = param as System.Windows.Controls.PasswordBox;
            if (passwordBox == null) return;

            var password = passwordBox.Password;

            bool bIsValid = _adminService.ValidateLogin(strAdminId, password);

            if (bIsValid)
            {
                _mainViewModel.IsAdminMode = true;

                // 창닫기
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.Content is AdminLoginView)?.Close();

                // QR 생성 창으로 이동
                _mainViewModel.CurrentView = new QrCreateViewModel();
            }
            else
            {
                MessageBox.Show(Loc["LoginFailMessage"], Loc["LoginFailTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



    }
}
