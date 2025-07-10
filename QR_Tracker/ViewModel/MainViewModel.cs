using LiteDB;
using QR_Tracker.Model;
using QR_Tracker.Services;
using QR_Tracker.ViewModel.BaseViewModels;
using QR_Tracker.Service;
using QR_Tracker.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QR_Tracker.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;
        public ObservableCollection<string> Languages { get; } = new ObservableCollection<string> { "한국어", "English" };
        private string _selectedLanguage = "한국어";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));

                    // 언어 변경 처리
                    if (value == "한국어") Loc.ChangeCulture("ko");
                    else if (value == "English") Loc.ChangeCulture("en");
                }
            }
        }
        private bool _isAdminMode;
        public bool IsAdminMode
        {
            get => _isAdminMode;
            set
            {
                if (_isAdminMode != value)
                {
                    _isAdminMode = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNotAdminMode));
                }
            }
        }
        public bool IsNotAdminMode => !IsAdminMode;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set {  _currentView = value; OnPropertyChanged(); }
        }

        public ICommand AdminModeCommand { get; }
        public ICommand AdminModeExitCommand { get; }
        public ICommand ShowQrCreateCommand { get; }
        public ICommand ShowQrDetectCommand { get; }
        public ICommand ShowReportCommand { get; }

        public MainViewModel()
        {
            IsAdminMode = false;
            ShowQrCreateCommand = new RelayCommand(_ => CurrentView = new QrCreateViewModel());
            ShowQrDetectCommand = new RelayCommand(_ => CurrentView = new QrDetectViewModel());
            ShowReportCommand = new RelayCommand(_ => CurrentView = new ReportViewModel());
            AdminModeCommand = new RelayCommand(_ =>
            {
                var loginWindow = new Window
                {
                    Title = "관리자 로그인",
                    Content = new AdminLoginView
                    {
                        DataContext = new AdminLoginViewModel(this)  // MainViewModel 전달
                    },
                    Width = 300,
                    Height = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.NoResize,
                    Owner = Application.Current.MainWindow
                };

                loginWindow.ShowDialog();
            });
            AdminModeExitCommand = new RelayCommand(AdminModeLogout);
            // 초기 화면
            CurrentView = new QrDetectViewModel();
        }

        private void AdminModeLogout(object param)
        {
            IsAdminMode = false;
            CurrentView = new QrDetectViewModel();
        }
    }
}
