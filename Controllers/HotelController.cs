﻿using HotelServer.Common;
using HotelServer.Controllers.request;
using HotelServer.Data;
using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelServer.Controllers
{
    public interface IHotelController
    {
        public IActionResult GetAll();
        public Task<IActionResult> GetAllTypeHotel();
        public Task<IActionResult> GetDetail(string id);
        public IActionResult AddHotel(HotelRequest hotel);
        public IActionResult DeleteHotel(SingleIdRequest request);
        public IActionResult UpdateHotel(HotelRequest hotel);
    }

    [Route("[controller]")]
    [ApiController]
    public class HotelController : ControllerBase, IHotelController
    {
        private readonly IHotelService _hotelService;
        private readonly IUnitOfWork _unitOfWork;

        public HotelController(IHotelService hotelService, IUnitOfWork unitOfWork)
        {
            _hotelService = hotelService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var hotel = _hotelService.GetAll();
            return Ok(hotel);
        }

        [HttpGet("getType")]
        [Authorize]
        public async Task<IActionResult> GetAllTypeHotel() {
            var typesHotel = await _hotelService.GetTypes();
            return Ok(typesHotel);
        }

        [HttpGet("getDetail")]
        [Authorize]
        public async Task<IActionResult> GetDetail(string id) {

            var response = new AuthResponse();
            if(id == null)
            {
                response.State = false;
                response.Message = "Id must not null";
                return BadRequest(response);
            }
            var hoteldb = _hotelService.GetById(id);
            if(hoteldb == null)
            {
                response.State = false;
                response.Message = "Can't get hotel by this id";
                return BadRequest(response);
            }

            response.State = true;
            response.Message = "Success";
            response.Data = hoteldb;
            return Ok(response);
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddHotel(HotelRequest request)
        {
            var response = new AuthResponse();
            var hotel = new Hotel();
            //generate ID
            var Id = SupportFunctions.GeneralId("H");
            hotel.Id = Id;

            //validate hotel
            if(request.Name == null)
            {
                response.State = false;
                response.Message = "missing field required!";
                return BadRequest(response);
            };

            hotel.Name = request.Name;
            hotel.Image = request.Image;
            hotel.Location = request.Location;
            hotel.TypeId = request.TypeId;
            hotel.PayType = request.PayType;
            hotel.Services = request.Service;
            hotel.Address = request.Address;

            //add to Db
            _hotelService.Add(hotel);
            _hotelService.SaveChanges();

            response.State = true;
            response.Message = "Add new hotel successful!";

            return Ok(response);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteHotel(SingleIdRequest request)
        {
            var response = new AuthResponse();
            //check id is exist
            var hotel = _hotelService.GetById(request.Id);
            if(hotel == null)
            {
                response.State = false;
                response.Message = "Hotel does not exist!";
                return BadRequest(response);
            }

            //delete to Db
            _hotelService.Delete(request.Id);
            _hotelService.SaveChanges();


            response.State = true;
            response.Message = "Delete new hotel successful!";

            return Ok(response);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateHotel(HotelRequest request)
        {
            var response = new AuthResponse();
            //generate ID

            //validate hotel
            if (request.Name == null)
            {
                response.State = false;
                response.Message = "missing field required!";
                return BadRequest(response);
            };

            var hotelDb = _hotelService.GetById(request.Id);
            if (hotelDb == null)
            {
                response.State = false;
                response.Message = "Hotel does not exist!";
                return BadRequest(response);
            }

            hotelDb.Name = request.Name;
            hotelDb.Image = request.Image;
            hotelDb.Location = request.Location;
            hotelDb.TypeId = request.TypeId;
            hotelDb.PayType = request.PayType;
            hotelDb.Services = request.Service;

            //add to Db
            _hotelService.Update(hotelDb);
            _hotelService.SaveChanges();


            response.State = true;
            response.Message = "update hotel successful!";

            return Ok(response);
        }
    }
}
