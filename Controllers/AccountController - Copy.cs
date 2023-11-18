//using HotelServer.Common;
//using HotelServer.Data;
//using HotelServer.Model;
//using HotelServer.Service;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace HotelServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly IUserService _userService;

//        public AccountController(IUserService userService)
//        {
//            _userService = userService;
//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            var users = _userService.GetAll();
//            return Ok(users);
//        }

//        [HttpPost("register")]
//        public IActionResult Post(User user)
//        {
//            try
//            {
//                if(user == null)
//                {
//                    return BadRequest();
//                }

//                bool result = _userService.Add(user);

//                if (result) { 
//                    _userService.SaveChanges();
//                    return Ok("register successful!");
//                }
//                else
//                {
//                    return BadRequest();
//                }
//            }catch (Exception ex)
//            {
//                return StatusCode(500, "Error retrieving data from database");
//            }
//        }
//    }
//}
