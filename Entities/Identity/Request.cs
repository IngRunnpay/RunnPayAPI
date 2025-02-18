using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Identity
{
    public class Request
    {
        public int ID { get; set; }
        public string UID { get; set; }
        public string RequestDetail { get; set; }
        public string CSR { get; set; }
        public DateTime Date_add { get; set; }
        public string ApplicationUserId { get; set; }
        public int States_RequestID { get; set; }
        public int RequestIdECD { get; set; }
        public string SerialNumber { get; set; }
        public string ManagedBy { get; set; }
        public string ContingencyRAId { get; set; }
        public ICollection<Response> Responses { get; set; }

    }

}
