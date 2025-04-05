using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PollRepository : IPollRepository
    {
        private readonly PollDbContext _context;

        // Constructor Injection: The DbContext is provided via dependency injection.
        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        // Creates a poll, setting the creation date and saving it in the database.
        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = DateTime.Now;
            _context.Polls.Add(poll);
            _context.SaveChanges();
        }

        // Returns all polls, sorted by creation date in descending order.
        public IEnumerable<Poll> GetPolls()
        {
            return _context.Polls.OrderByDescending(p => p.DateCreated).ToList();
        }

        // Updates the vote count for the chosen option.
        public void Vote(int pollId, int optionNumber)
        {
            // Find the poll by its Id.
            var poll = _context.Polls.Find(pollId);
            if (poll != null)
            {
                // Increase the vote count for the selected option.
                switch (optionNumber)
                {
                    case 1:
                        poll.Option1VotesCount++;
                        break;
                    case 2:
                        poll.Option2VotesCount++;
                        break;
                    case 3:
                        poll.Option3VotesCount++;
                        break;
                    default:
                        throw new ArgumentException("Invalid option number.");
                }
                _context.SaveChanges();  // Persist changes.
            }
        }
    }
}
