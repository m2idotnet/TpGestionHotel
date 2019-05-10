using GestionHotel.Classes;
using System;

namespace GestionHotel
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();
            Console.ReadLine();
        }

        static void Menu()
        {
            Console.WriteLine("1- List of customers");
            Console.WriteLine("2- List of bookings");
            Console.WriteLine("3- Add Booking ");
            Console.WriteLine("0- Exit");
        }

        static void Init()
        {
            int choice = 3;
            do
            {
                Console.WriteLine("1- New Hotel");
                Console.WriteLine("2- Choose Hotel");
                Console.WriteLine("0- Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch(choice)
                    {
                        case 1:
                            NewHotel();
                            break;
                        case 2:
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            while (choice != 0);
        }

        static void NewHotel()
        {
            Console.Write("Hotel Name : ");
            string n = Console.ReadLine();
            int roomNumber;
            do
            {
                Console.Write("Rooms Number : ");
                Int32.TryParse(Console.ReadLine(), out roomNumber);
            }
            while (roomNumber == 0);
            Hotel h = new Hotel() { Name = n, RoomsNumber = roomNumber };
            if(h.Save())
            {
                Random r = new Random();
                for(int i=1; i<= h.RoomsNumber; i++)
                {
                    Room room = new Room()
                    {
                        Number = i,
                        HotelId = h.Id,
                        OccupatedMax = r.Next(2, 4),
                        Status = RoomStatus.Free
                    };
                    room.Save();
                }
            }
        }
    }
}
