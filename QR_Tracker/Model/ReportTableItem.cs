using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Tracker.Model
{
    public class ReportTableItem
    {
        public string EmployeeNumber { get; set; }   // 사번
        public string Name { get; set; }             // 이름
        public DateTime Date { get; set; }           // 날짜
        public DateTime? CheckInTime { get; set; }   // 출근 시간
        public DateTime? CheckOutTime { get; set; }  // 퇴근 시간
    }
}
