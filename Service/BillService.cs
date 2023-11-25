using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;
using System.Collections.Generic;

namespace HotelServer.Service
{
    public interface IBillService
    {
        void Add(Bill bill);
        void Delete(string id);
        void Update(Bill bill);
        Bill GetById(string id);
        IEnumerable<Bill> GetAllByUserID(string id);
        IEnumerable<Bill> GetAll();
        void FinishBill(string id);
        void ChangeRoom(Bill bill, string idRoom);
        void SaveChanges();

    }
    public class BillService : IBillService
    {
        IBillRepository _billRepository;
        IUnitOfWork _unitOfWork;
        IHotelRepository _hotelRepository;

        public BillService(IBillRepository billRepository, IUnitOfWork unitOfWork, IHotelRepository hotelRepository)
        {
            this._billRepository = billRepository;
            this._unitOfWork = unitOfWork;
            this._hotelRepository = hotelRepository;
        }
        public void Add(Bill bill)
        {
            _billRepository.Add(bill);
        }

        public void ChangeRoom(Bill bill, string idRoom)
        {
            bill.RoomId = idRoom;
            _billRepository.Update(bill);
        }

        public void Delete(string id)
        {
            _billRepository.Delete(GetById(id));
        }

        public void FinishBill(string id)
        {
            Bill bill = GetById(id);
            bill.Status = true;
            _billRepository.Update(bill);
        }

        public IEnumerable<Bill> GetAll()
        {
            return _billRepository.GetAll(new string[] { "Room", "User" }).ToList();
        }

        public IEnumerable<Bill> GetAllByUserID(string id)
        {
            IEnumerable<Bill> listBill = _billRepository.GetMulti(x => x.UserId == id, new string[] { "Room", "User" });
            foreach(Bill bill in listBill)
            {
                bill.Room.Hotel = _hotelRepository.GetSingleById(bill.Room.HotelId);
            }
            return listBill;
        }

        public Bill GetById(string id)
        {
            return _billRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Bill bill)
        {
            _billRepository.Update(bill);
        }
    }
}
