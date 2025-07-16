using LiteDB;
using QR_Tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QR_Tracker.Services
{
    public class LiteDbService
    {
        private const string DB_FILE = @"Attendance.db";

        // 직원 등록
        public void AddEmployee(Employee emp)
        {
            try
            {
                using (var db = new LiteDatabase(DB_FILE))
                {
                    var col = db.GetCollection<Employee>("employees");
                    col.EnsureIndex(x => x.EmployeeNumber, unique: true);
                    col.Insert(emp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 저장 중 오류: " + ex.Message);
            }
        }

        // 출퇴근 기록 추가
        public void AddAttendance(AttendanceLog log)
        {
            using (var db = new LiteDatabase(DB_FILE))
            {
                var col = db.GetCollection<AttendanceLog>("logs");
                col.Insert(log);
            }
        }

        // 사번으로 직원 찾기
        public Employee GetEmployeeByNumber(string employeeNumber)
        {
            using (var db = new LiteDatabase(DB_FILE))
            {
                var col = db.GetCollection<Employee>("employees");
                return col.FindOne(x => x.EmployeeNumber == employeeNumber);
            }
        }

        // 특정 직원의 출근 기록 가져오기
        public List<AttendanceLog> GetLogs(ObjectId employeeId)
        {
            using (var db = new LiteDatabase(DB_FILE))
            {
                var col = db.GetCollection<AttendanceLog>("logs");
                return col.Find(x => x.EmployeeId == employeeId).ToList();
            }
        }
        public List<Employee> GetAllEmployees()
        {
            using (var db = new LiteDatabase(DB_FILE))
            {
                var col = db.GetCollection<Employee>("employees");
                return col.FindAll().ToList();
            }
        }

        // 특정 날짜 범위로 출근 기록 가져오기
        public List<AttendanceLog> GetLogsBetween(DateTime startDate, DateTime endDate)
        {
            using (var db = new LiteDatabase(DB_FILE))
            {
                var col = db.GetCollection<AttendanceLog>("logs");
                return col.Find(x => x.CheckInTime >= startDate && x.CheckInTime < endDate).ToList();
            }
        }
    }
}
