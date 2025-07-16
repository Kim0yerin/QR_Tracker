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
using LiveCharts;
using System.Security.Permissions;
using System.Reflection;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using Serilog;

namespace QR_Tracker.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;
        private readonly LiteDbService _dbService = new LiteDbService();
        //콤보박스
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
        // 표로 볼지 그래프로 볼지
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

        // 그래프
        private SeriesCollection _seriesHistogramData;
        public SeriesCollection SeriesHistogramData
        {
            get => _seriesHistogramData;
            set => SetProperty(ref _seriesHistogramData, value);
        }
        public string[] XLabels { get; set; }
        public Func<double, string> YValues { get; set; }

        // Command
        public ICommand ShowReportCommand { get; }
        public ICommand ExportCSVCommand { get; }

        public ReportViewModel()
        {
            ShowReportCommand = new RelayCommand(ShowReport);
            ExportCSVCommand = new RelayCommand(ExportCSV);
            DataGridItem = new ObservableCollection<ReportTableItem>();
            
            // Data grid test용
            DataGridItem.Add(new ReportTableItem { Name = "김예린", EmployeeNumber = "P345SG", Date = new DateTime(2025, 7, 14), CheckInTime = new DateTime(2025, 7, 14, 7, 56, 24)});
            
            SeriesHistogramData = new SeriesCollection();
            XLabels = new string[0];
            YValues = val => val.ToString("N0");
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
                // dayoption = 일간 , 시간대별 출근 인원수 
                if(DaySelectedItem == "일간")
                {
                    var today = DateTime.Today;
                    var data = _dbService.GetLogsBetween(today, today);
                    
                }

                // dayoption = 주간 , 요일별 출근 인원수
                else if (DaySelectedItem == "주간")
                {

                }
                // dayoption = 월간 , 날짜별 출근 인원수
                else if(DaySelectedItem == "월간")
                {

                }

                //histogram 표시
                XLabels = new[] { "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00" };
                var counts = new ChartValues<int> { 5, 12, 18, 15, 7, 9, 6 };

                SeriesHistogramData = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "출근 인원수",
                        Values = counts,
                        Fill = new SolidColorBrush(Colors.SteelBlue)
                    }
                };
                YValues = value => value.ToString("N0"); // 정수 출력
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
