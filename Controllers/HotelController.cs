using HotelServer.Common;
using HotelServer.Controllers.request;
using HotelServer.Data;
using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IUnitOfWork _unitOfWork;

        public HotelController(IHotelService userService, IUnitOfWork unitOfWork)
        {
            _hotelService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var hotel = _hotelService.GetAll();
            return Ok(hotel);
        }



        [HttpPost("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult AddHotel(Hotel hotel)
        {
            var result = 0;
            //var response = new AuthResponse();
            ////generate ID
            //var Id = "H" + DateTime.Now.ToString("yyyyMMddHHmmss");

            //validate hotel

            //add to Db
            //_hotelService.Add(hotel);
            //_unitOfWork.Commit();


            return Ok("hello");
        }

    }
}
