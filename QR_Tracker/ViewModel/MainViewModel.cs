using QRtracker.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QR_Tracker.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand ShowQrCreateCommand { get; }

        public ICommand ShowQrDetectCommand { get; }
        public ICommand ShowReportCommand { get; }

        private object _currentView;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object CurrentView
            {
                get => _currentView;
                set { _currentView = value; OnPropertyChanged(); }
            }


        public MainViewModel()
            {
                ShowQrDetectCommand = new RelayCommand(_ => CurrentView = new QrDetectViewModel());
                ShowReportCommand = new RelayCommand(_ => CurrentView = new ReportViewModel());

                // 초기 화면
                CurrentView = new QrDetectViewModel();
            }

    }
}
