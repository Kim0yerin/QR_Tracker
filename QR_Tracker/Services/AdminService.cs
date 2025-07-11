using LiteDB;
using QR_Tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QR_Tracker.Services
{
    public class AdminService
    {
        private const string DB_FILE = @"Attendance.db";

        // 로그인 검증
        public bool ValidateLogin(string Id, string password)
        {
            using (var db = new LiteDatabase(DB_FILE)) {
                var col = db.GetCollection<Admin>("admins");
                var user = col.FindOne(a => a.AdminId == Id);

                if (user == null)
                {
                    return false;
                }
                return user.Password == password;
            }
        }
    }
}
