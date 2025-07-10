using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace QR_Tracker.Model
{
    //사원 정보 테이블
    public class Employee
    {
        // 자동 생성되는 ObjectId (Primary Key)
        [BsonId]
        public ObjectId Id { get; set; }

        // 사번
        public string EmployeeNumber { get; set; }
        //사원이름
        public string EmployeeName { get; set; }
        //QR에 저장된 문자열
        public string QRCode { get; set; }
    }
}
