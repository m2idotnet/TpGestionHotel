using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GestionHotel.Classes
{
    public class Hotel
    {
        private int id;
        private string name;
        private int roomsNumber;
        private SqlCommand command;

        public string Name { get => name; set => name = value; }
        public int RoomsNumber { get => roomsNumber; set => roomsNumber = value; }
        public int Id { get => id; set => id = value; }

        public Hotel()
        {

        }

        public bool Save()
        {
            bool res = false;
            command = new SqlCommand("INSERT INTO Hotel(Name, RoomsNumber) OUTPUT INSERTED.ID values(@n,@r)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@n", Name));
            command.Parameters.Add(new SqlParameter("@r", RoomsNumber));   
            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            res = (Id > 0);
            return res;
        }
    }
}
