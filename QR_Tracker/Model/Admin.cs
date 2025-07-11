using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace QR_Tracker.Model
{
    public class Admin
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string AdminId { get; set; }
        public string Password { get; set; }
    }
}
