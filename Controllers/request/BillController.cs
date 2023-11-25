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
        public Task<IActionResult> GetBillDetail(String idBill);
        public Task<IActionResult> AddBill(BillRequest request);
        public IActionResult UpdateBill(BillRequest request);
        public IActionResult DeleteBill(SingleIdRequest request);
        public Task<IActionResult> FinishBill(SingleIdRequest request);
        public Task<IActionResult> GetAllBillOfUser(string id);
        public IActionResult GetAllBill();
    }
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class BillController : ControllerBase, IBillController
    {
        private readonly IBillService _billService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public BillController(IBillService billService, IUnitOfWork unitOfWork, IRoomService roomService, UserManager<User> userManager, IMailService mailService)
        {
            _billService = billService;
            _unitOfWork = unitOfWork;
            _roomService = roomService;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost]
        [Authorize]
        [Route("add")]
        public async Task<IActionResult> AddBill(BillRequest request)
        {
            var response = new AuthResponse();
            if(request.UserId == "" || request.Date == null)
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
                return Ok(response);
            }

            //check room is exist
            var roomDb = _roomService.GetById(request.RoomId);
            if(roomDb == null)
            {
                response.State = false;
                response.Message = "Room is not exist!";
                return Ok(response);
            }

            if (roomDb.Amount < request.amountRoom)
            {
                response.State = false;
                response.Message = "The amount of this room is out!";
                return Ok(response);
            }

            //generate ID
            var newId = SupportFunctions.GeneralId("B");

            Bill newBill = new Bill();
            newBill.Id = newId;
            newBill.Status = false;
            newBill.Period = request.Period;
            newBill.UserId = request.UserId;
            newBill.RoomId = request.RoomId;
            newBill.Date = request.Date;
            newBill.Amount = request.amountRoom;
            newBill.Total = roomDb.Price * request.amountRoom * request.Period;

            _billService.Add(newBill);

            //subtract number of room
            roomDb.Amount -= request.amountRoom;

            _roomService.Update(roomDb);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Add new bill successful!";
            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        [Route("delete")]
        public IActionResult DeleteBill(SingleIdRequest request)
        {
            var response = new AuthResponse();

            if(request.Id == "")
            {
                response.State = false;
                response.Message = "Missing field required!";
                return Ok(response) ;
            }

            var bill = _billService.GetById(request.Id);
            if (bill == null)
            {
                response.State = false;
                response.Message = "Bill does not exist!";
                return Ok(response);
            }


            var roomInDb = _roomService.GetById(bill.RoomId);
            roomInDb.Amount += bill.Amount;
            _billService.Delete(bill.Id);
            _roomService.Update(roomInDb);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Delete bill successful!";
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("finish")]
        public async Task<IActionResult> FinishBill(SingleIdRequest request)
        {
            var response = new AuthResponse();
            if(request.Id == "")
            {
                response.State = false;
                response.Message = "Missing field requied!";
                return Ok(response);
            }

            //get bill
            var billInDb = _billService.GetById(request.Id);
            if (billInDb == null)
            {
                response.State = false;
                response.Message = "Bill doesn't exist!";
                return Ok(response);
            }

            //get user
            var user = await _userManager.FindByIdAsync(request.Id);

            //send email
            string content = $"<h3>Hóa đơn mã #{billInDb.Id} đã được xác nhận!</h3>";
            await _mailService.SendEmailAsync(user.Email, "Xác nhận đặt phòng thành công!", content);

            billInDb.Status = true;
            _billService.Update(billInDb);
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Finish bill successful!";
            return Ok(response) ;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("/")]
        public IActionResult GetAllBill()
        {
            var response = new AuthResponse();

            var allBill = _billService.GetAll();

            response.State = true;
            response.Message = "Get all bill successful!";
            response.Data = allBill;
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [Route("/getAll")]
        public async Task<IActionResult> GetAllBillOfUser(string id)
        {
            var response = new AuthResponse();

            if(id == null)
            {
                response.State = false;
                response.Message = "Missing field required!";
                return Ok(response);
            }

            var userInDb = await _userManager.FindByIdAsync(id);
            if(userInDb == null)
            {
                response.State = false;
                response.Message = "User doesn't exist!";
                return Ok(response);
            }

            var bills = _billService.GetAllByUserID(userInDb.Id);
            response.State = true;
            response.Message = "Successful!";
            response.Data = bills;
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [Route("/getBillInfor")]
        public async Task<IActionResult> GetBillDetail(string idBill)
        {
            var response = new AuthResponse();
            
            if(idBill == null) {
                response.State = false;
                response.Message = "Missing field required!";
                return Ok(response);
            }

            var billInDb = _billService.GetById(idBill);
            if(billInDb == null)
            {
                response.State = false;
                response.Message = "Bill doesn't exist!";
                return Ok(response);
            }

            var roomInDb = _roomService.GetById(billInDb.RoomId);
            var user = await _userManager.FindByIdAsync(billInDb.UserId);
            user.PasswordHash = "";
            response.State = true;
            response.Message = "Successful!";
            response.Data = new { bill = billInDb, room = roomInDb, user = user };
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("/update")]
        public IActionResult UpdateBill(BillRequest request)
        {
            var response = new AuthResponse();

            if (request.Id == "")
            {
                response.State = false;
                response.Message = "Missing field required!";
                return Ok(response);
            }

            var billInDb = _billService.GetById(request.Id);
            if(billInDb == null)
            {
                response.State = false;
                response.Message = "Bill doesn't exist!";
                return Ok(response);
            }

            billInDb.Period = request.Period;
            billInDb.RoomId = request.RoomId;
            billInDb.Date = request.Date;
            billInDb.Amount = request.amountRoom;
            response.State = true;
            response.Message = "Successful!";
            return Ok(response);

        }
    }
}
