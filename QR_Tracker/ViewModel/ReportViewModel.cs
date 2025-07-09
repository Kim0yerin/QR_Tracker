using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QR_Tracker.ViewModel.BaseViewModels;

namespace QR_Tracker.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        public ICommand ShowDailyReportCommand { get; }
        public ICommand ShowWeekReportCommand { get; }
        public ICommand ShowMonthReportCommand { get; }

        public ReportViewModel()
        {
            ShowDailyReportCommand = new RelayCommand(DailyReportChart);
            ShowWeekReportCommand = new RelayCommand(WeekReportChart);
            ShowMonthReportCommand = new RelayCommand(MonthReportChart);
        }

        private void DailyReportChart(object param)
        {

        }

        private void WeekReportChart(object param)
        {

        }

        private void MonthReportChart(object param)
        {

        }

    }
}
