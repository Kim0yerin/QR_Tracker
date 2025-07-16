using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using Microsoft.Win32;
using QR_Tracker.Model;
using QR_Tracker.Services;
using QR_Tracker.ViewModel.BaseViewModels;
using System.IO;
using Serilog;

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
        private ObservableCollection<ReportTableItem> _dataGridItem;
        public ObservableCollection<ReportTableItem> DataGridItem
        {
            get => _dataGridItem;
            set { _dataGridItem = value; OnPropertyChanged(); }
        }

        public ICommand ShowReportCommand { get; }
        public ICommand ExportCSVCommand { get; }

        public ReportViewModel()
        {
            ShowReportCommand = new RelayCommand(ShowReport);
            ExportCSVCommand = new RelayCommand(ExportCSV);
            DataGridItem = new ObservableCollection<ReportTableItem>();
            
            //test용
            DataGridItem.Add(new ReportTableItem { Name = "김예린", EmployeeNumber = "P345SG", Date = new DateTime(2025, 7, 1), CheckInTime = new DateTime(2025, 7, 1, 7, 56, 24)});
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
            if (DataGridItem ==null || !DataGridItem.Any())
            {
                Log.Warning("CSV 내보내기 시도 - 데이터 없음");
                MessageBox.Show("저장할 데이터가 없습니다.");
                return;
            }

            var dialog = new SaveFileDialog
            {
                FileName = "report.csv",
                Filter = "CSV 파일 (*.csv)|*.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                Log.Information("CSV 내보내기 시도 - 경로 : {path}", dialog.FileName);

                var csvLines = new List<string>();
                var header = "이름, 사번, 날짜, 출근시간, 퇴근시간";
                csvLines.Add(header);

                foreach (var record in DataGridItem)
                {
                    string line = $"{record.Name},{record.EmployeeNumber},{record.Date:yyyy-MM-dd}, {record.CheckInTime:HH:mm:ss}, {record.CheckOutTime:HH:mm:ss}";
                    csvLines.Add(line);
                }
                try
                {
                    File.WriteAllLines(dialog.FileName, csvLines, Encoding.UTF8);
                    Log.Information("CSV 내보내기 성공 - 총 {Count}건", DataGridItem.Count);
                    MessageBox.Show("CSV 저장이 완료되었습니다.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "CSV 내보내기 실패 - 경로: {Path}", dialog.FileName);
                    MessageBox.Show($"CSV 저장 중 오류 발생 : {ex.Message}");
                }
            }
        
        }

    }
}
