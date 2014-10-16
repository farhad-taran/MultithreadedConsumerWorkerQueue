using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTVSP
{
    public class Message
    {

        public int Id { get; set; }
        public int StartThreadId { get; set; }
        public int DestinationThreadId { get; set; }
        public int DispatchCount { get; set; }
        public bool Delivered { get; set; }
    }

}