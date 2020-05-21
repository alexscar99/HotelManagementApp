﻿using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            // Load and return data using stored procedure
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                          new { startDate, endDate },
                                                          connectionStringName,
                                                          true);
        }

        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            // Insert to 'Guests' table and then retrieve that guest to eventually book them
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert",
                                                                 new { firstName, lastName },
                                                                 connectionStringName,
                                                                 true).First();

            // To calculate total cost for the booking, need the room type and how many days were stayed
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id = @Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName,
                                                                          false).First();
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            // Get available rooms that fit the criteria of the dates desired and the room type
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                              new { startDate, endDate, roomTypeId },
                                                                              connectionStringName,
                                                                              true);

            // Save booking by inserting into 'Bookings' table
            _db.SaveData("dbo.spBookings_Insert",
                         new 
                         { 
                             roomId = availableRooms.First().Id, 
                             guestId = guest.Id,
                             startDate,
                             endDate,
                             totalCost = timeStaying.Days * roomType.Price
                         },
                         connectionStringName,
                         true);
        }
    }
}
