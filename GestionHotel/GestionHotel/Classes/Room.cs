using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GestionHotel.Classes
{
    public class Room
    {
        private int id;
        private int number;
        private int hotelId;
        private int occupatedMax;
        private static SqlCommand command;
        private RoomStatus status;

        public int Id { get => id; set => id = value; }
        public int Number { get => number; set => number = value; }
        public int HotelId { get => hotelId; set => hotelId = value; }
        public int OccupatedMax { get => occupatedMax; set => occupatedMax = value; }
        public RoomStatus Status { get => status; set => status = value; }

        public bool Save()
        {
            bool res = false;
            command = new SqlCommand("INSERT INTO Room(Number, HotelId, OccupatedMax, Status) OUTPUT INSERTED.ID values(@n,@h,@o,@s)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@n", Number));
            command.Parameters.Add(new SqlParameter("@h", HotelId));
            command.Parameters.Add(new SqlParameter("@o", OccupatedMax));
            command.Parameters.Add(new SqlParameter("@s", Status));
            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            res = (Id > 0);
            return res;
        }

        public bool UpdateStatus(RoomStatus s)
        {
            bool res = false;
            command = new SqlCommand("UPDATE Room set Status = @s WHERE Id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s", s));
            command.Parameters.Add(new SqlParameter("@i", Id));
            Connection.Instance.Open();
            //if(command.ExecuteNonQuery() > 0)
            //{
            //    res = true;
            //}
            res = command.ExecuteNonQuery() > 0;
            command.Dispose();
            Connection.Instance.Close();
            return res;
        }
        public override string ToString()
        {
            string res = "Room Number : " + Number;
            res += " Status : " + Status;
            res += " Occupated Max : " + OccupatedMax;
            return res;
        }

        public static List<Room> GetRoomsByStatus(RoomStatus s)
        {
            List<Room> res = new List<Room>();
            command = new SqlCommand("SELECT * FROM Room WHERE Status = @s", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s", s));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Room r = new Room
                {
                    Id = reader.GetInt32(0),
                    Number = reader.GetInt32(1),
                    HotelId = reader.GetInt32(2),
                    OccupatedMax = reader.GetInt32(3),
                    Status =s
                };
                res.Add(r);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return res;
        }
    }

    public enum RoomStatus
    {
        Busy,
        Free
    }
    
}
