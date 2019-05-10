using GestionHotel.Classes;
using System;
using System.Collections.Generic;

namespace GestionHotel
{
    class Program
    {
        static Hotel hotel;
        static void Main(string[] args)
        {
            Init();
            Console.ReadLine();
        }

        static void Menu()
        {
            int choice = 4;
            do
            {
                Console.WriteLine("1- List of customers");
                Console.WriteLine("2- List of bookings");
                Console.WriteLine("3- Add Booking ");
                Console.WriteLine("0- Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            GetCustomers();
                            break;
                        case 2:
                            break;
                        case 3:
                            AddBooking();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            while (choice != 0);
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
                            GetHotel();
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
            Console.WriteLine("Hotel correctly added");
        }

        static void GetHotel()
        {
            Console.Write("Hotel Name : ");
            string name = Console.ReadLine();
            Hotel h = new Hotel(name);
            if(h.Id > 0)
            {
                hotel = h;
                Console.Clear();
                Menu();
            }
            else
            {
                Console.WriteLine("Hotel not found");
            }
        }

        static void GetCustomers()
        {
            Console.Clear();
            List<Customer> liste = Customer.GetCustomers(hotel.Id);
            if(liste.Count > 0)
            {
                Console.WriteLine("------Liste of Customers------");
                foreach(Customer c in liste)
                {
                    Console.WriteLine(c);
                    Console.WriteLine("----------------------------");
                }
            }
            else
            {
                Console.WriteLine("No customers found");
            }
            CustomerMenu();
        }

        static void CustomerMenu()
        {
            int choice = 4;
            do
            {
                Console.WriteLine("1- New Customer");
                Console.WriteLine("2- Customer information");
                Console.WriteLine("3- Add Booking");
                Console.WriteLine("0- Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            NewCustomer();
                            break;
                        case 2:
                            InformationCustomer();
                            break;
                        case 3:
                            AddBooking();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            while (choice != 0);
        }

        static void NewCustomer()
        {
            Customer c = new Customer();
            Console.Write("First Name : ");
            c.FirstName= Console.ReadLine();
            Console.Write("Last Name : ");
            c.LastName = Console.ReadLine();
            Console.Write("Phone : ");
            c.Phone = Console.ReadLine();
            Console.Write("Address : ");
            c.Address = Console.ReadLine();
            c.HotelId = hotel.Id;
            if (c.Save())
            {
                Console.WriteLine("Customer Added with Id : "+c.Id);
            }
            else
            {
                Console.WriteLine("Data Base error");
            }
        }
        static void InformationCustomer()
        {
            Console.Write("Customer phone : ");
            string p = Console.ReadLine();
            Customer c = new Customer(p);
            if(c.Id > 0)
            {
                Console.WriteLine(c);
                Console.WriteLine("-------Bookings---------");
                foreach(Booking b in Booking.GetBookings(c.Id))
                {
                    Console.WriteLine(b);
                }
            }
            else
            {
                Console.WriteLine("Customer not found");
            }
        }
        static void AddBooking()
        {
            Console.Write("Customer phone : ");
            string p = Console.ReadLine();
            Customer c = new Customer(p);
            if (c.Id > 0)
            {
                if(Room.GetRoomsByStatus(RoomStatus.Free).Count > 0)
                {
                    foreach (Room r in Room.GetRoomsByStatus(RoomStatus.Free))
                    {
                        Console.WriteLine(r);
                        Console.WriteLine("-------------------");
                    }
                    int number;
                    do
                    {
                        Console.Write("How many persons ? : ");
                        Int32.TryParse(Console.ReadLine(), out number);
                        do
                        {
                            Console.Write("Room's number : ");
                            int n = Convert.ToInt32(Console.ReadLine());
                            Room room = Room.GetRoomsByStatus(RoomStatus.Free).Find(x => x.Number == n);
                            Booking b = new Booking() {
                                CustomerId = c.Id,
                                RoomId = room.Id,
                                OccupatedNumber = (number > room.OccupatedMax) ? room.OccupatedMax : number,
                                Status = BookingStatus.Validated
                            };
                            if (b.Save())
                            {
                                room.UpdateStatus(RoomStatus.Busy);
                                number -= room.OccupatedMax;
                            }
                        }
                        while (number > 0);
                    }
                    while (number == 0);
                }
                else
                {
                    Console.WriteLine("No free room");
                }
                
            }
            else
            {
                Console.WriteLine("Customer not found");
            }
        }
    }
    
}
