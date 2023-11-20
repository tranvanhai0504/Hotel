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
        public Task<IActionResult> AddBill(AddBillRequest request);
        public IActionResult UpdateBill(AddBillRequest request);
        public IActionResult AcceptBill(String idBill);
        public IActionResult DeleteBill(SingleIdRequest request);
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
            newBill.Status = false;
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
                return BadRequest(response) ;
            }

            _billService.Delete(request.Id);
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
                return BadRequest(response);
            }

            //get bill
            var billInDb = _billService.GetById(request.Id);
            if (billInDb == null)
            {
                response.State = false;
                response.Message = "Bill doesn't exist!";
                return BadRequest(response);
            }

            //get user
            var user = await _userManager.FindByIdAsync(request.Id);

            //send email
            string content = $"<h3>Hóa đơn mã #{billInDb.Id} đã được xác nhận!</h3>";
            await _mailService.SendEmailAsync(user.Email, "Xác nhận đặt phòng thành công!", content);

            billInDb.Status = true;
            _unitOfWork.Commit();

            response.State = true;
            response.Message = "Finish bill successful!";
            return Ok(response) ;
        }

        //[httpget]
        //[authorize]
        //[route("")]
        //public iactionresult getbilldetail(string idbill)
        //{
        //    throw new notimplementedexception();
        //}

        //[httpput]
        //[authorize]
        //[route("update")]
        //public iactionresult updatebill(addbillrequest request)
        //{
        //    throw new notimplementedexception();
        //}
    }
}
