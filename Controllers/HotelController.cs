using HotelServer.Common;
using HotelServer.Controllers.request;
using HotelServer.Data;
using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelServer.Controllers
{
    public interface IHotelController
    {
        public IActionResult GetAll();
        public IActionResult GetAllTypeHotel();
        public IActionResult GetDetail(string id);
        public IActionResult AddHotel(Hotel hotel);
        public IActionResult DeleteHotel(SingleIdHotelRequest request);
        public IActionResult UpdateHotel(Hotel hotel);
    }

    [Route("[controller]")]
    [ApiController]
    public class HotelController : ControllerBase, IHotelController
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

        [HttpGet("getType")]
        [Authorize]
        public IActionResult GetAllTypeHotel() {
            var typesHotel = _hotelService.GetTypes();
            return Ok(typesHotel);
        }

        [HttpGet("getDetail")]
        [Authorize]
        public IActionResult GetDetail(string id) {

            var response = new AuthResponse();
            if(id == null)
            {
                response.State = false;
                response.Message = "Id must not null";
                return BadRequest(response);
            }
            var hoteldb = _hotelService.GetById(id);
            if(hoteldb == null)
            {
                response.State = false;
                response.Message = "Can't get hotel by this id";
                return BadRequest(response);
            }

            var roomOfHotel = _hotelService.GetAllRooms(hoteldb.Id);
            hoteldb.Rooms = roomOfHotel;

            response.State = true;
            response.Message = "Success";
            response.Data = hoteldb;
            return Ok(response);
        }


        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddHotel(Hotel hotel)
        {
            var response = new AuthResponse();
            //generate ID
            var Id = "H" + DateTime.Now.ToString("yyyyMMddHHmmss");
            hotel.Id = Id;

            //validate hotel
            if(hotel.Name != null)
            {
                response.State = false;
                response.Message = "missing field required!";
                return BadRequest(response);
            };

            //add to Db
            _hotelService.Add(hotel);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Add new hotel successful!";

            return Ok(response);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteHotel(SingleIdHotelRequest request)
        {
            var response = new AuthResponse();
            //check id is exist
            var hotel = _hotelService.GetById(request.Id);
            if(hotel != null)
            {
                response.State = false;
                response.Message = "Hotel does not exist!";
                return BadRequest(response);
            }

            //delete to Db
            _hotelService.Delete(request.Id);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Delete new hotel successful!";

            return Ok(response);
        }

        [HttpPost("update")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateHotel(Hotel hotel)
        {
            var response = new AuthResponse();
            //generate ID

            //validate hotel
            if (hotel.Name != null)
            {
                response.State = false;
                response.Message = "missing field required!";
                return BadRequest(response);
            };

            var hotelDb = _hotelService.GetById(hotel.Id);
            if (hotelDb != null)
            {
                response.State = false;
                response.Message = "Hotel does not exist!";
                return BadRequest(response);
            }

            //add to Db
            _hotelService.Update(hotel);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "update new hotel successful!";

            return Ok(response);
        }
    }
}
