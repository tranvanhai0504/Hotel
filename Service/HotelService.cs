using HotelServer.Data.Infrastructure;
using HotelServer.Data.Respositories;
using HotelServer.Model;
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
        }

        public void ChangeImage(string imageURL, string id)
        {
            Hotel hotel = GetById(id);
            _hotelRepository.Update(hotel);
        }

        public void Delete(string id)
        {
            _hotelRepository.Delete(GetById(id));
        }

        public IEnumerable<Hotel> GetAll()
        {
            return _hotelRepository.GetAll(new string[] {"Hotels"});
        }

        public Hotel GetById(string id)
        {
            return _hotelRepository.GetSingleById(id);
        }

        public IEnumerable<TypeHotel> GetTypes()
        {
            return _hotelRepository.GetTypes();
        }

        public void Update(Hotel hotel)
        {
            _hotelRepository.Update(hotel);
        }
    }
}
