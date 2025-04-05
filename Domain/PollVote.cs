using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PollVote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string UserId { get; set; }
        public int OptionNumber { get; set; } // The option voted for

        // Navigation properties (optional)
        public Poll Poll { get; set; }
    }
}
