//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using VolleyMS.BusinessLogic.Services;
//using VolleyMS.Contracts.DTOs;
//using VolleyMS.Core.Requests;
//using VolleyMS.Extensions;


//namespace VolleyMS.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class userController : ControllerBase
//    {
//        private readonly UserService _userService;
//        private readonly NotificationService _notificationService;
//        private readonly JoinClubService _joinClubService;

//        public userController(UserService userService, NotificationService notificationService, JoinClubService joinClubService)
//        {
//            _userService = userService;
//            _notificationService = notificationService;
//            _joinClubService = joinClubService;
//        }

//        [HttpGet("{userName}")]
//        [Authorize]
//        public async Task<IActionResult> Get(string userName)
//        {
//            var user = await _userService.Get(userName);
//            if(user == null) return Ok(null);

//            var response = new UserDto()
//            {
//                Id = user.Id,
//                FirstName = user.Name,
//                LastName = user.Surname,
//                UserName = user.UserName
//            };
//            return Ok(response);
//        }

//        [HttpPut("{userName}")]
//        [Authorize]
//        public async Task<IActionResult> Modify(string userName, [FromBody] UserModificationRequest userModificationRequest)
//        {
//            var userNameClaim = User.GetUserName();
            
//            await _userService.Modify(userName, userModificationRequest);
//            return Ok("User was successfuly modified");
//        }
//    }
//}
