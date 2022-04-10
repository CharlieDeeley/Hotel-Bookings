using HotelBookings.Abstract;
using HotelBookings.Concrete;
using System;
using System.Collections.Concurrent;

namespace HotelBookings
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentDictionary<Tuple<int, DateTime>, string> bookings = new ConcurrentDictionary<Tuple<int, DateTime>, string>();
            int[] rooms = new int[] { 101, 102, 201, 203 };

            IBookingManager bookingManager = new BookingManager(bookings, rooms);
            DateTime today = DateTime.Now;
            Console.WriteLine(bookingManager.IsRoomAvailable(101, today)); // outputs true 
            bookingManager.AddBooking("Patel", 101, today);
            Console.WriteLine(bookingManager.IsRoomAvailable(101, today)); // outputs false 
            bookingManager.AddBooking("Li", 101, today); // throws an exception
        }
    }
}
