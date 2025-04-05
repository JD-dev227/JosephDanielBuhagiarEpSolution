using DataAccess;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using System.Linq;
using JosephDanielBuhagiarEpSolution.Models;

namespace Presentation.Controllers
{
    
    public class PollController : Controller
    {
        private readonly IPollRepository _pollRepository;

        // Constructor Injection: IPollRepository is injected.
        public PollController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        // Action to list all polls.
        public IActionResult Index()
        {
            var polls = _pollRepository.GetPolls();
            return View(polls);
        }

        // GET: Display form for creating a new poll.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create a new poll.
        [HttpPost]
        public IActionResult Create(Poll poll)
        {
            if (ModelState.IsValid)
            {
                _pollRepository.CreatePoll(poll);
                return RedirectToAction("Index");
            }
            return View(poll);
        }

        // GET: Show poll details and voting options.
        public IActionResult Details(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            if (poll == null)
                return NotFound();
            return View(poll);
        }

      

        [HttpPost]
        [VoteRestrictionFilter]
        /* public IActionResult Vote(int pollId, int optionNumber)
         {
             _pollRepository.Vote(pollId, optionNumber);
             // Set session key so the user cannot vote again on this poll.
             HttpContext.Session.SetString($"Voted_{pollId}", "true");
             return RedirectToAction("Details", new { id = pollId });
         }
        */

        [HttpPost]
        [VoteRestrictionFilter]
        public async Task<IActionResult> Vote(int pollId, int optionNumber)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var success = await _pollRepository.VoteAsync(pollId, optionNumber, userId);
            if (!success)
            {
                // Optionally display an error message that the user has already voted.
                TempData["Error"] = "You have already voted in this poll.";
            }
            return RedirectToAction("Details", new { id = pollId });
        }

    }
}
