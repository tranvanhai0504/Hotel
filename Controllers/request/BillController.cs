using HotelServer.Data;
using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelServer.Controllers.request
{
    public interface IBillController
    {
        //all
        public Task<IActionResult> GetBillDetail(String idBill);
        public Task<IActionResult> AddBill(Bill bill);
        public Task<IActionResult> UpdateBill(Bill bill);
        //admin
        public Task<IActionResult> DeleteBill(String idBill);
        //user
        public Task<IActionResult> GetBillsByUserId(String userId);
        //public Task<IActionResult> GetBill
    }
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class BillController
    {
        private readonly IBillService _billService;
        private readonly IUnitOfWork _unitOfWork;

        public BillController(IBillService billService, IUnitOfWork unitOfWork)
        {
            _billService = billService;
            _unitOfWork = unitOfWork;
        }
    }
}
