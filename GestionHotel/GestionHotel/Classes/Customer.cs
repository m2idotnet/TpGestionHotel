using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GestionHotel.Classes
{
    public class Customer
    {
        private int id;
        private string firstName, lastName, phone, address;
        private int hotelId;

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Address { get => address; set => address = value; }
        public int HotelId { get => hotelId; set => hotelId = value; }

        private static SqlCommand command;

        public Customer()
        {

        }

        public Customer(string phone)
        {
            command = new SqlCommand("SELECT * FROM Customer WHERE Phone = @p", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@p", phone));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                Id = reader.GetInt32(0);
                FirstName = reader.GetString(1);
                LastName = reader.GetString(2);
                Phone = phone;
                Address = reader.GetString(4);
                HotelId = reader.GetInt32(5);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public Customer(int i)
        {
            command = new SqlCommand("SELECT * FROM Customer WHERE Id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@i", i));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                FirstName = reader.GetString(1);
                LastName = reader.GetString(2);
                Phone = reader.GetString(3);
                Address = reader.GetString(4);
                HotelId = reader.GetInt32(5);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public bool Save()
        {
            bool res = false;
            command = new SqlCommand("INSERT INTO Customer(firstName, lastName, phone, address, hotelId) OUTPUT INSERTED.ID values(@f,@l,@p,@a, @h)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@f", FirstName));
            command.Parameters.Add(new SqlParameter("@l", LastName));
            command.Parameters.Add(new SqlParameter("@p", Phone));
            command.Parameters.Add(new SqlParameter("@a", Address));
            command.Parameters.Add(new SqlParameter("@h", HotelId));
            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            res = (Id > 0);
            return res;
        }

        public static List<Customer> GetCustomers(int hotelId)
        {
            List<Customer> customers = new List<Customer>();
            command = new SqlCommand("SELECT * FROM Customer WHERE hotelId = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@i", hotelId));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                Customer c = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Phone = reader.GetString(3),
                    Address = reader.GetString(4),
                    HotelId = reader.GetInt32(5)
                };
                customers.Add(c);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return customers;
        }

        public override string ToString()
        {
            string res = "Name : " + FirstName + " " + LastName;
            res += " Phone : " + Phone;
            res += "\nAddress : " + Address;
            res += "\nCustomer Id : " + Id;
            return res;
        }
    }
}
