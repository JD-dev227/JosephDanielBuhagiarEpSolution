using Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    // Implements IPollRepository by saving and reading from a JSON file.
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filePath = "polls.json";

        // Helper method to load polls from the file.
        private List<Poll> LoadPolls()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Poll>();
            }
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Poll>>(json) ?? new List<Poll>();
        }

        // Helper method to save polls to the file.
        private void SavePolls(List<Poll> polls)
        {
            var json = JsonSerializer.Serialize(polls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = System.DateTime.Now;
            var polls = LoadPolls();
            // Simulate an auto-incrementing Id.
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            polls.Add(poll);
            SavePolls(polls);
        }

        public IEnumerable<Poll> GetPolls()
        {
            var polls = LoadPolls();
            // Return polls sorted by DateCreated (most recent first)
            return polls.OrderByDescending(p => p.DateCreated).ToList();
        }
        /*
        public void Vote(int pollId, int optionNumber)
        {
            var polls = LoadPolls();
            var poll = polls.FirstOrDefault(p => p.Id == pollId);
            if (poll != null)
            {
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
                        throw new System.ArgumentException("Invalid option number.");
                }
                SavePolls(polls);
            }
        }
        */

        // Asynchronous Vote method implementation.
        public async Task<bool> VoteAsync(int pollId, int optionNumber, string userId)
        {
            // Load polls from file asynchronously.
            List<Poll> polls;
            if (!File.Exists(_filePath))
            {
                polls = new List<Poll>();
            }
            else
            {
                var json = await File.ReadAllTextAsync(_filePath);
                polls = JsonSerializer.Deserialize<List<Poll>>(json) ?? new List<Poll>();
            }

            var poll = polls.FirstOrDefault(p => p.Id == pollId);
            if (poll == null)
            {
                throw new InvalidOperationException("Poll not found.");
            }

            // Increase vote count for the specified option.
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

            // Save the updated polls list back to the file asynchronously.
            var updatedJson = JsonSerializer.Serialize(polls, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, updatedJson);

            // For file repository, we are not tracking user votes persistently.
            // Therefore, always return true.
            return true;
        }
    }
}
