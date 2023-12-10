using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaDomainLayer.DTOs
{
    public class TopDoctorDTO
    {
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Specialize { get; set; }
        public int NumberOfRequests { get; set; }
    }
}
