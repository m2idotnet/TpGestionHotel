﻿using GestionHotel.Classes;
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
                            GetBookings();
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
                        Status = RoomStatus.Free,
                        
                    };
                    room.Price = room.OccupatedMax == 3 ? 50 : 40;
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
                List<Booking> bookingsUnpaid = Booking.GetBookings(c.Id).FindAll(x => x.StatusInvoice == InvoiceStatus.notPaid);
                decimal total = 0;
                foreach (Booking b in bookingsUnpaid)
                {
                    Room r = new Room(b.RoomId);
                    total += r.Price;
                }
                Console.WriteLine($"You have to pay : {total}");
                Console.WriteLine("1- Paid");
                Console.WriteLine("2- Not paid");
                int number;
                Int32.TryParse(Console.ReadLine(), out number);
                if(number == 1)
                {
                    Paid(c.Id, total, bookingsUnpaid);
                }
            }
            else
            {
                Console.WriteLine("Customer not found");
            }
        }

        static void Paid(int idCustomer, decimal total, List<Booking> bs)
        {
            Invoice i = new Invoice()
            {
                Price = total,
                CustomerId = idCustomer,
                Status = InvoiceStatus.paid
            };

            if (i.Save())
            {
                foreach(Booking b in bs)
                {
                    b.UpdateStatus(InvoiceStatus.paid);
                }
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
                    }
                    while (number == 0);
                    do
                    {
                        Console.Write("Room's number : ");
                        int n = Convert.ToInt32(Console.ReadLine());
                        Room room = Room.GetRoomsByStatus(RoomStatus.Free).Find(x => x.Number == n);
                        Booking b = new Booking()
                        {
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

        static void GetBookings()
        {
            Console.Clear();
            List<Booking> list = Booking.GetBookingsByHotelId(hotel.Id);
            if (list.Count > 0)
            {
                foreach(Booking b in list)
                {
                    Console.WriteLine(b);
                    Room r = new Room(b.RoomId);
                    Console.WriteLine(r);
                    Customer c = new Customer(b.CustomerId);
                    Console.WriteLine(c);
                    Console.WriteLine("-------------------");
                }
            }
            else
            {
                Console.WriteLine("No booking for this hotel");
            }
            BookingMenu();
        }

        static void BookingMenu()
        {
            int choice = 4;
            do
            {
                Console.WriteLine("1- New Booking");
                Console.WriteLine("2- Change status");
                Console.WriteLine("0- Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            AddBooking();
                            break;
                        case 2:
                            ChangeBookingStatus();
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

        static void ChangeBookingStatus()
        {
            Console.Write("Booking Code : ");
            string code = Console.ReadLine();
            Booking b = new Booking(code);
            if(b.Id > 0)
            {
                if(b.Status == BookingStatus.Validated)
                {
                    b.UpdateStatus(BookingStatus.Canceled);
                    Room r = new Room(b.RoomId);
                    r.UpdateStatus(RoomStatus.Free);
                }
                Console.WriteLine("Status updated");
            }
            else
            {
                Console.WriteLine("no booking found");
            }
        }
    }
    
}
