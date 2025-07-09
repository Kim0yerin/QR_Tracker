using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QR_Tracker.ViewModel.BaseViewModels;

namespace QR_Tracker.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand ShowQrCreateCommand { get; }

        public ICommand ShowQrDetectCommand { get; }
        public ICommand ShowReportCommand { get; }
        public ICommand ShowDailyReportCommand { get; }
        public ICommand ShowWeekReportCommand { get; }
        public ICommand ShowMonthReportCommand { get; }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set {  _currentView = value; OnPropertyChanged(); }
        }

        private bool _isReportMenuVisible = false;
        public bool IsReportMenuVisible
        {
            get => _isReportMenuVisible;
            set { _isReportMenuVisible = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            ShowQrCreateCommand = new RelayCommand(_ => CurrentView = new QrCreateViewModel());
            ShowQrDetectCommand = new RelayCommand(_ => CurrentView = new QrDetectViewModel());
            ShowReportCommand = new RelayCommand(ShowReport);
            ShowDailyReportCommand = new RelayCommand(_ => CurrentView = new DailyReportViewModel());
            ShowWeekReportCommand = new RelayCommand(_ => CurrentView = new WeekReportViewModel());
            ShowMonthReportCommand = new RelayCommand(_ => CurrentView = new MonthReportViewModel());

            // 초기 화면
            CurrentView = new QrDetectViewModel();
        }

        public void ShowReport(object param)
        {
            if (!IsReportMenuVisible)
            {
                CurrentView = new DailyReportViewModel();
                IsReportMenuVisible = true;
            }
            else
            {
                IsReportMenuVisible = false;
            }
        }
    }
}
