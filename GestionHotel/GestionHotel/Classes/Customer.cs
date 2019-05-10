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


        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Address { get => address; set => address = value; }
        private SqlCommand command;

        public Customer()
        {

        }

        public bool Save()
        {
            bool res = false;
            command = new SqlCommand("INSERT INTO Customer(firstName, lastName, phone, address) OUTPUT INSERTED.ID values(@f,@l,@p,@a)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@f", FirstName));
            command.Parameters.Add(new SqlParameter("@l", LastName));
            command.Parameters.Add(new SqlParameter("@p", Phone));
            command.Parameters.Add(new SqlParameter("@a", Address));
            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            res = (Id > 0);
            return res;
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
