using HotelBookings.Abstract;
using HotelBookings.Concrete;
using System;
using System.Collections.Concurrent;
using Xunit;

namespace HotelBookings.Test
{
    public class HotelBookingsTests
    {
        readonly int[] rooms = new int[] { 101, 102, 201, 203 };

        [Fact]
        public void AddBooking_RoomBooked_Success()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;

            bookingManager.AddBooking("Patel", 101, today);

            Assert.True(bookings.TryGetValue(new Tuple<int, DateTime>(101, today), out string guest));
        }

        [Fact]
        public void AddBooking_SameRoomBookedOnDifferentDays_Success()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;
            bookingManager.AddBooking("Patel", 101, today);

            DateTime dateInFuture = today.AddDays(1);
            bookingManager.AddBooking("Patel", 101, dateInFuture);

            Assert.True(bookings.TryGetValue(new Tuple<int, DateTime>(101, dateInFuture), out string guest));
        }

        [Fact]
        public void AddBooking_RoomBookedTwiceOnSameDay_ThrowsException()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;
            bookingManager.AddBooking("Patel", 101, today);

            Assert.Throws<Exception>(() => bookingManager.AddBooking("Davidson", 101, today));
        }

        [Fact]
        public void IsRoomAvailable_RoomIsAvailable_ReturnsTrue()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;

            Assert.True(bookingManager.IsRoomAvailable(101, today));
        }

        [Fact]
        public void IsRoomAvailable_RoomIsNotAvailable_ReturnsFalse()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;
            bookingManager.AddBooking("Patel", 101, today);

            Assert.False(bookingManager.IsRoomAvailable(101, today));
        }

        [Fact]
        public void GetAvailableRooms_AllRoomsAvailable_ReturnsAllRooms()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;

            Assert.Equal(rooms, bookingManager.GetAvailableRooms(today));
        }

        [Fact]
        public void GetAvailableRooms_RoomsBooked_ReturnsAllAvailableRooms()
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;

            bookingManager.AddBooking("Patel", 101, today);
            int[] availableRooms = new int[] { 102, 201, 203 };

            Assert.Equal(availableRooms, bookingManager.GetAvailableRooms(today));
        }
    }
}
