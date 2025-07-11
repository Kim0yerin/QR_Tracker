using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QR_Tracker.Services;
using QR_Tracker.ViewModel.BaseViewModels;

namespace QR_Tracker.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;
        private readonly LiteDbService _dbService = new LiteDbService();

        public List<string> FormatOptions { get; } = new List<string> { "출퇴근 기록표", "시간별 인원수 그래프" };
        public List<string> DayOptions { get; } = new List<string> { "일간", "주간", "월간" };

        private string _formatSelectedItem = "출퇴근 기록표";
        public string FormatSelectedItem
        {
            get => _formatSelectedItem;
            set
            {
                if (SetProperty(ref _formatSelectedItem, value))
                {
                    bIsTableFormat = (_formatSelectedItem == "출퇴근 기록표");
                }
            }
        }
        private string _daySelectedItem = "일간";
        public string DaySelectedItem
        {
            get => _daySelectedItem;
            set => SetProperty(ref _daySelectedItem, value);
        }
        private bool _bisTableFormat = true;
        public bool bIsTableFormat
        {
            get => _bisTableFormat;
            set
            {
                if (SetProperty(ref _bisTableFormat, value))
                {
                    OnPropertyChanged(nameof(bIsNotTableFormat));
                }
            }
        }
        public bool bIsNotTableFormat => !bIsTableFormat;

        public ICommand ShowReportCommand { get; }
        public ICommand ExportCSVCommand { get; }

        public ReportViewModel()
        {
            ShowReportCommand = new RelayCommand(ShowReport);
            ExportCSVCommand = new RelayCommand(ExportCSV);
        }

        private void ShowReport(object param)
        {
            if (bIsTableFormat)
            {
                //datagrid 표시
            }
            else
            {
                //histogram 표시
            }
        }

        private void ExportCSV(object param) 
        {

        
        }

    }
}
