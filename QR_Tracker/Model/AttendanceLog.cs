using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Tracker.Model
{
    public class AttendanceLog
    {
        [BsonId]
        public ObjectId Id { get; set; }

        // 외래 키처럼 Employee의 ObjectId 참조
        public ObjectId EmployeeId { get; set; }

        // 날짜만 (시간 제외)
        public DateTime Date { get; set; }

        // 출근 시간 (nullable)
        public DateTime? CheckInTime { get; set; }

        // 퇴근 시간 (nullable)
        public DateTime? CheckOutTime { get; set; }
    }
}