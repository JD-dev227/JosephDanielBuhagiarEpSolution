using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public interface IPollRepository
    {
        // Creates a new poll.
        void CreatePoll(Poll poll);

        // Returns all polls sorted by date (most recent first).
        IEnumerable<Poll> GetPolls();

        // Updates the poll with a vote for the specified option.
        //void Vote(int pollId, int optionNumber);
        // Asynchronously updates the poll with a vote for the specified option.
        Task<bool> VoteAsync(int pollId, int optionNumber, string userId);
    }
}
