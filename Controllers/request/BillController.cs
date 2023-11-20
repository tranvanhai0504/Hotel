using HotelServer.Common;
using HotelServer.Data;
using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelServer.Controllers.request
{
    public interface IBillController
    {
        //all
        public IActionResult GetBillDetail(String idBill);
        public IActionResult AddBill(AddBillRequest request);
        public IActionResult UpdateBill(AddBillRequest request);
        public IActionResult CancelBill(String idBill);
        public IActionResult FinishBill(String idBill);
        //admin
        public IActionResult DeleteBill(String idBill);
        //user
        
        //public Task<IActionResult> GetBill
    }
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        private readonly UserManager<User> _userManager;

        public BillController(IBillService billService, IUnitOfWork unitOfWork, IRoomService roomService, UserManager<User> userManager)
        {
            _billService = billService;
            _unitOfWork = unitOfWork;
            _roomService = roomService;
            _userManager = userManager;
        }


        [HttpPost]
        [Authorize]
        [Route("add")]
        public async Task<IActionResult> AddBill(AddBillRequest request)
        {
            var response = new AuthResponse();
            if(request.UserId == "" || request.RoomId == "" || request.Date == null)
            {
                response.State = false;
                response.Message = "Missing field required!";
                return BadRequest(response);
            }

            //check user is exist
            var userInDb = await _userManager.FindByIdAsync(request.UserId);
            if(userInDb == null)
            {
                response.State = false;
                response.Message = "User is not exist!";
                return BadRequest(response);
            }

            //check room is exist
            var roomDb = _roomService.GetById(request.RoomId);
            if(roomDb == null)
            {
                response.State = false;
                response.Message = "Room is not exist!";
                return BadRequest(response);
            }

            if (roomDb.Amount == 0)
            {
                response.State = false;
                response.Message = "The amount of this room is out!";
                return BadRequest(response);
            }

            //generate ID
            var newId = SupportFunctions.GeneralId("B");

            Bill newBill = new Bill();
            newBill.Id = newId;
            newBill.Status = true;
            newBill.UserId = request.UserId;
            newBill.RoomId = request.RoomId;
            newBill.Date = request.Date;
            newBill.Total = request.Total;

            _billService.Add(newBill);

            //subtract number of room
            roomDb.Amount -= 1;

            _roomService.Update(roomDb);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Add new bill successful!";
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [Route("cancel")]
        public IActionResult CancelBill(SingleIdRequest request)
        {
            var response = new AuthResponse();
            if(request.Id == "")
            {
                response.State=false;
                response.Message = "Missing field required!";
                return BadRequest(response);
            }

            var bill = _billService.GetById(request.Id);
            if(bill == null)
            {
                response.State = false;
                response.Message = "Bill doesn't exist!";
                return BadRequest(response);
            }

            bill.Status = false;
            _billService.Update(bill);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Update bill successful!";
            return Ok(response);
        }

        [HttpDelete]
        [Authorize( Roles = "admin")]
        [Route("delete")]
        public IActionResult DeleteBill(SingleIdRequest request)
        {
            var response = new AuthResponse();
            if(request.Id == "")
            {
                response.State = false;
                response.Message = "Missing field required!";
                return BadRequest(response) ;
            }

            _billService.Delete(request.Id);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Delete bill successful!";
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [Route("finish")]
        public IActionResult FinishBill(string idBill)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public IActionResult GetBillDetail(string idBill)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public IActionResult UpdateBill(AddBillRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
