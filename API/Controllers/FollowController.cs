using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command{TargetUsername = username}));
        }

        [HttpGet("{username}")]
        //predicate comes from query string, for example: ?predicate=following
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new List.Query{Username = username, 
                Predicate = predicate}));
        }
    }
}