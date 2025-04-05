using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters
{
    // Custom action filter to ensure a user votes only once per poll.
    public class VoteRestrictionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // For example, check if a session key "Voted_{pollId}" exists.
            if (context.HttpContext.Request.Form.TryGetValue("pollId", out var pollId))
            {
                var key = $"Voted_{pollId}";
                if (context.HttpContext.Session.GetString(key) != null)
                {
                    // Redirect to an error or details page if the user has already voted.
                    context.Result = new RedirectToActionResult("Details", "Poll", new { id = pollId });
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
