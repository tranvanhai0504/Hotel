using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;
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
        IEnumerable<TypeRoom> GetTypeRooms();
        TypeRoom GetTypeOfRoom(string idType);
        IEnumerable<Room> GetRoomByFilter(Expression<Func<Room, bool>> predicate);
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
            return _roomsRepository.GetAll(new string[] {"Rooms"});
        }

        public IEnumerable<TypeRoom> GetTypeRooms()
        {
            var types = _roomsRepository.GetTypeRooms();
            foreach (var type in types)
            {
                type.Rooms = null;
            }
            return _roomsRepository.GetTypeRooms();
        }

        public Room GetById(string id)
        {
            return _roomsRepository.GetSingleById(id);
        }

        public void Update(Room room)
        {
            _roomsRepository.Update(room);
        }

        public TypeRoom GetTypeOfRoom(string idType)
        {
            var type = _roomsRepository.GetTypeOfRoom(idType);
            return type;
        }

        public IEnumerable<Room> GetRoomByFilter(Expression<Func<Room, bool>> predicate)
        {
            var roomFilter = _roomsRepository.GetMulti(predicate);
            return roomFilter;
        }
    }
}
