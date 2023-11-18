using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;

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

        public Room GetById(string id)
        {
            return _roomsRepository.GetSingleById(id);
        }

        public void Update(Room room)
        {
            _roomsRepository.Update(room);
        }
    }
}
