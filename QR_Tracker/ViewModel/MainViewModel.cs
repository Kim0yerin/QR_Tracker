using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QR_Tracker.Model.Service;
using QR_Tracker.ViewModel.BaseViewModels;

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
        public ICommand ShowQrCreateCommand { get; }

        public ICommand ShowQrDetectCommand { get; }
        public ICommand ShowReportCommand { get; }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set {  _currentView = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            ShowQrCreateCommand = new RelayCommand(_ => CurrentView = new QrCreateViewModel());
            ShowQrDetectCommand = new RelayCommand(_ => CurrentView = new QrDetectViewModel());
            ShowReportCommand = new RelayCommand(_ => CurrentView = new ReportViewModel());

            // 초기 화면
            CurrentView = new QrDetectViewModel();
        }

    }
}
