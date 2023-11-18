using HotelServer.Controllers.request;
using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HotelServer.Controllers
{
    public interface IRoomController
    {
        public IActionResult GetAllRoom();
        public IActionResult GetRoomDetail(string id);
        public IActionResult GetAllTypeRoom();
        public IActionResult AddRoom(Room room);
        public IActionResult DeleteRoom(SingleIdRequest request);
        public IActionResult UpdateRoom(Room room);
    }
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase, IRoomController
    {
        private readonly IRoomService _roomService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelService _hotelService;

        public RoomController(IRoomService roomService, IHotelService hotelService, IUnitOfWork unitOfWork)
        {
            _roomService = roomService;
            _unitOfWork = unitOfWork;
            _hotelService = hotelService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddRoom(Room room)
        {
            var response = new AuthResponse();
            //generate ID
            var Id = "H" + DateTime.Now.ToString("yyyyMMddHHmmss");
            room.Id = Id;

            //add to Db
            _roomService.Add(room);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Add new room successful!";

            return Ok(response);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteRoom(SingleIdRequest request)
        {
            var response = new AuthResponse();
            //check id is exist
            var hotel = _roomService.GetById(request.Id);
            if (hotel == null)
            {
                response.State = false;
                response.Message = "Room does not exist!";
                return BadRequest(response);
            }

            //delete to Db
            _roomService.Delete(request.Id);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Delete room successful!";

            return Ok(response);
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllRoom()
        {
            var rooms = _roomService.GetAll();
            return Ok(rooms);
        }

        [HttpGet("getAllType")]
        [Authorize()]
        public IActionResult GetAllTypeRoom()
        {
            var typeRoom = _roomService.GetTypeRooms();
            return Ok(typeRoom);
        }

        [HttpGet("getDetail")]
        [Authorize]
        public IActionResult GetRoomDetail(string id)
        {
            var response = new AuthResponse();
            if (id == null)
            {
                response.State = false;
                response.Message = "Id must not null";
                return BadRequest(response);
            }
            var roomdb = _roomService.GetById(id);
            if (roomdb == null)
            {
                response.State = false;
                response.Message = "Can't get room by this id";
                return BadRequest(response);
            }

            //get hotel
            var hotelOfRoom = _hotelService.GetById(roomdb.HotelId);
            roomdb.Hotel = hotelOfRoom;

            //get type
            var roomType = _roomService.GetTypeOfRoom(roomdb.QuantityId);
            roomdb.TypeRoom = roomType;

            response.State = true;
            response.Message = "Success";
            response.Data = roomdb;
            return Ok(response);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateRoom(Room room)
        {
            var response = new AuthResponse();
            //generate ID

            //validate hotel
            if (room.Amount == null)
            {
                response.State = false;
                response.Message = "missing field required!";
                return BadRequest(response);
            };

            var hotelDb = _roomService.GetById(room.Id);
            if (hotelDb == null)
            {
                response.State = false;
                response.Message = "Room does not exist!";
                return BadRequest(response);
            }

            //add to Db
            _roomService.Update(room);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "update room successful!";

            return Ok(response);
        }
    }
}
