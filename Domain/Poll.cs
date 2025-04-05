using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Option1Text { get; set; } = string.Empty;
        public string Option2Text { get; set; } = string.Empty;
        public string Option3Text { get; set; } = string.Empty;
        public int Option1VotesCount { get; set; }
        public int Option2VotesCount { get; set; }
        public int Option3VotesCount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
