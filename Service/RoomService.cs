using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelServer.Service
{
    public interface IRoomService
    {
        void Add(Room room);
        void Delete(string id);
        void Update(Room room);
        Room GetById(string id);
        IEnumerable<Room> GetAll();
        void ChangeAmount(string id,  int amount);
        void ChangeImage(string id, string image);
        Task<IEnumerable<TypeRoom>> GetTypeRooms();
        Task<TypeRoom> GetTypeOfRoom(string idType);
        Task<IEnumerable<Room>> GetRoomByFilter(Expression<Func<Room, bool>> predicate);
        void SaveChanges();

    } 
    public class RoomService : IRoomService
    {
        IRoomRepository _roomsRepository;
        IUnitOfWork _unitOfWork;

        public RoomService(IRoomRepository roomsRepository, IUnitOfWork unitOfWork)
        {
            _roomsRepository = roomsRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Room room)
        {
            _roomsRepository.Add(room);
        }

        public void ChangeAmount(string id, int amount)
        {
            Room room = GetById(id);
            room.Amount = amount;
            Update(room);
        }

        public void ChangeImage(string id, string image)
        {
            Room room = GetById(id);
            room.Image = image;
            Update(room);
        }

        public void Delete(string id)
        {
            _roomsRepository.Delete(GetById(id));
        }

        public IEnumerable<Room> GetAll()
        {
            var rooms = _roomsRepository.GetAll().ToList();
            //foreach (var room in rooms)
            //{
            //    room.Hotel = null;
            //}
            return rooms;
        }

        public async Task<IEnumerable<TypeRoom>> GetTypeRooms()
        {
            var types = await _roomsRepository.GetTypeRooms();
            //foreach (var type in types)
            //{
            //    type.Rooms = null;
            //}
            return types;
        }

        public Room GetById(string id)
        {
            return _roomsRepository.GetSingleByCondition(room => room.Id == id, new string[] {"Hotel", "TypeRoom"});
        }

        public void Update(Room room)
        {
            _roomsRepository.Update(room);
        }

        public async Task<TypeRoom> GetTypeOfRoom(string idType)
        {
            var type = await _roomsRepository.GetTypeOfRoom(idType);
            return type;
        }

        public async Task<IEnumerable<Room>> GetRoomByFilter(Expression<Func<Room, bool>> predicate)
        {
            var roomFilter = await _roomsRepository.GetMulti(predicate).ToListAsync();
            return roomFilter;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
