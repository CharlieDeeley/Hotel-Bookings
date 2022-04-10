using HotelBookings.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookings.Concrete
{
    public class BookingManager : IBookingManager
    {
        // Reasons for use:
        // - Don't have to use locks.
        // - Allows for another threat to retrieve values ei check room availability
        // - Tuple<room, Date> allows for a room to be booked once per day
        ConcurrentDictionary<Tuple<int, DateTime>, string> bookings;

        readonly int[] rooms;

        public BookingManager(ConcurrentDictionary<Tuple<int, DateTime>, string> bookings, int[] rooms)
        {
            this.bookings = bookings;
            this.rooms = rooms;
        }

        public void AddBooking(string guest, int room, DateTime date)
        {
            if (!rooms.Contains(room))
            {
                throw new Exception("ERROR! Room: " + room + " does not exist");
            }
            try
            {
                if (this.bookings.TryAdd(new Tuple<int, DateTime>(room, date), guest))
                {
                    Console.WriteLine("Success! Room: " + room + " booked for " + guest + " on " + date);
                }
                else
                {
                    // prevent dupolicate bookings
                    throw new Exception("ERROR! Room: " + room + " is already booked on: " + date);
                }
            }
            catch (OverflowException e)
            {
                Console.WriteLine("The system contains to many bookings" + e);
            }
        }


        public bool IsRoomAvailable(int room, DateTime date)
        {
            if (this.bookings.TryGetValue(new Tuple<int, DateTime>(room, date), out string guest))
            {
                Console.WriteLine("Unavailable! Room: " + room + " is already booked on: " + date + " by: " + guest);
                return false;
            }
            else
            {
                Console.WriteLine("Available! Room: " + room + " is available on: " + date);
                return true;
            }
        }

        public IEnumerable<int> GetAvailableRooms(DateTime date)
        {
            List<int> availableRooms = new List<int>();

            foreach (int room in this.rooms)
            {
                if (!this.bookings.TryGetValue(new Tuple<int, DateTime>(room, date), out string guest))
                {
                    availableRooms.Add(room);
                }
            }

            return availableRooms;
        }
    }
}