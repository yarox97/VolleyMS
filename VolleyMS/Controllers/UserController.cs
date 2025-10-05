using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string userName, string Password, string Name, string Surname)
        {
            //try
            //{
                await _userService.Register(userName, Password, Name, Surname);
                return Ok(Password);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //return BadRequest();
        }
    }
}
