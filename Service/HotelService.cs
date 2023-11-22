using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;
using System.Linq.Expressions;
using static HotelServer.Data.Respositories.HotelRepository;

namespace HotelServer.Service
{
    public interface IHotelService
    {
        void Add(Hotel hotel);
        void Delete(string id);
        void Update(Hotel hotel);
        Hotel GetById(string id);
        IEnumerable<Hotel> GetAll();
        void ChangeImage(string imageURL, string id);
        IEnumerable<TypeHotel> GetTypes();
        void SaveChanges();
        Task<IEnumerable<Room>> GetAllRooms(string hotelId);
        IEnumerable<Hotel> GetAllByFilter(Expression<Func<Hotel, bool>> predicate);
        
    }
    public class HotelService : IHotelService
    {
        IHotelRepository _hotelRepository;
        IUnitOfWork _unitOfWork;

        public HotelService(IHotelRepository hotelRepository, IUnitOfWork unitOfWork)
        {
            _hotelRepository = hotelRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Hotel hotel)
        {
            _hotelRepository.Add(hotel);
            SaveChanges();
        }

        public void ChangeImage(string imageURL, string id)
        {
            Hotel hotel = GetById(id);
            _hotelRepository.Update(hotel);
            SaveChanges();
        }

        public void Delete(string id)
        {
            _hotelRepository.Delete(GetById(id));
            SaveChanges();
        }

        public IEnumerable<Hotel> GetAll()
        {
            return _hotelRepository.GetAll(new string[] {"Hotels"});
        }

        public IEnumerable<Hotel> GetAllByFilter(Expression<Func<Hotel, bool>> predicate)
        {
            var hotels = _hotelRepository.GetMulti(predicate);
            return hotels;
        }

        public async Task<IEnumerable<Room>> GetAllRooms(string hotelId)
        {
            return await _hotelRepository.GetAllRooms(hotelId);
        }

        public Hotel GetById(string id)
        {
            return _hotelRepository.GetSingleById(id);
        }

        public IEnumerable<TypeHotel> GetTypes()
        {
            var types = _hotelRepository.GetTypes();
            foreach (var type in types)
            {
                type.Hotels = null;
            }
            return types;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Hotel hotel)
        {
            _hotelRepository.Update(hotel);
            SaveChanges();
        }
    }
}
