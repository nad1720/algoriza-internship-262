using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaDomainLayer.DTOs
{
    public class RequestStatisticsDTO
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int CancelledRequests { get; set; }
    }
}
