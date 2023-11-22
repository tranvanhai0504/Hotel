using HotelServer.Common;
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
        public IActionResult AddRoom(RoomRequest request);
        public IActionResult DeleteRoom(SingleIdRequest request);
        public IActionResult UpdateRoom(RoomRequest request);
        public IActionResult Search(RoomSearchRequest request);
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
        public IActionResult AddRoom(RoomRequest request)
        {
            var newRoom = new Room();
            var response = new AuthResponse();
            //generate ID
            var Id = SupportFunctions.GeneralId("R");
            newRoom.Id = Id;

            newRoom.TypeRoomId = request.TypeRoomId;
            newRoom.Status = true;
            newRoom.Amount = request.Amount;
            newRoom.Image = request.Image;
            newRoom.Bed = request.Bed;
            newRoom.HotelId = request.HotelId;
            newRoom.Price = request.Price;
            newRoom.Services = request.Services;
            newRoom.PriceDiscount = request.PriceDiscount;

            //add to Db
            _roomService.Add(newRoom);
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
            var response = new AuthResponse();

            var typeRoom = _roomService.GetTypeRooms();
            response.State = true;
            response.Message = "Get all type of room success!";
            response.Data = typeRoom;
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
            var roomType = _roomService.GetTypeOfRoom(roomdb.TypeRoomId);
            roomdb.TypeRoom = roomType;

            response.State = true;
            response.Message = "Success";
            response.Data = roomdb;
            return Ok(response);
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult Search(RoomSearchRequest request)
        {
            var hotelsInLocation = _hotelService.GetAllByFilter(hotel => hotel.Location.ToLower() ==  request.Location.ToLower());
            foreach(var hotel in hotelsInLocation)
            {
                var rooms = _roomService.GetRoomByFilter(room => room.HotelId == hotel.Id && room.Amount >= request.AmountRoom);
                hotel.Rooms = rooms;
            }

            var response = new AuthResponse();
            response.State = true;
            response.Message = "Successful!";
            response.Data = hotelsInLocation;
            return Ok(response);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateRoom(RoomRequest request)
        {
            var response = new AuthResponse();
            //generate ID

            //validate hotel
            if (request.Amount == null)
            {
                response.State = false;
                response.Message = "missing field required!";
                return BadRequest(response);
            };

            var roomDb = _roomService.GetById(request.Id);
            if (roomDb == null)
            {
                response.State = false;
                response.Message = "Room does not exist!";
                return BadRequest(response);
            }

            //update
            roomDb.TypeRoomId = request.TypeRoomId;
            roomDb.Status = true;
            roomDb.Amount = request.Amount;
            roomDb.Image = request.Image;
            roomDb.Bed = request.Bed;
            roomDb.HotelId = request.HotelId;
            roomDb.Price = request.Price;
            roomDb.Services = request.Services;

            //add to Db
            _roomService.Update(roomDb);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "update room successful!";

            return Ok(response);
        }

        [HttpGet("getByHotelId")]
        [Authorize]
        public IActionResult GetRoomsByHotelId(string id)
        {
            var response = new AuthResponse();

            var rooms = _roomService.GetRoomByFilter(room => room.HotelId == id);

            response.State = true;
            response.Message = "Get all rooms successful!";
            response.Data = rooms;
            return Ok(response);
        }
    }
}
