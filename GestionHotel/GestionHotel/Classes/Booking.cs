using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GestionHotel.Classes
{
    public class Booking
    {
        private int id;
        private string code;
        private int customerId;
        private int roomId;
        private int occupatedNumber;
        private BookingStatus status;
        private SqlCommand command;

        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int RoomId { get => roomId; set => roomId = value; }
        public int OccupatedNumber { get => occupatedNumber; set => occupatedNumber = value; }
        public BookingStatus Status { get => status; set => status = value; }

        public bool Save()
        {
            bool res = false;
            command = new SqlCommand("INSERT INTO Booking(Code, CustomerId, RoomId, OccupatedNumber, Status) OUTPUT INSERTED.ID values(@c,@cu,@r,@o,@s)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@c", Code));
            command.Parameters.Add(new SqlParameter("@cu", CustomerId));
            command.Parameters.Add(new SqlParameter("@r", RoomId));
            command.Parameters.Add(new SqlParameter("@o", OccupatedNumber));
            command.Parameters.Add(new SqlParameter("@s", Status));
            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            res = (Id > 0);
            return res;
        }
        public bool UpdateStatus(BookingStatus s)
        {
            bool res = false;
            command = new SqlCommand("UPDATE Booking set Status = @s WHERE Id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s", s));
            command.Parameters.Add(new SqlParameter("@i", Id));
            Connection.Instance.Open();
            res = command.ExecuteNonQuery() > 0;
            command.Dispose();
            Connection.Instance.Close();
            return res;
        }

        public override string ToString()
        {
            string res = "Code : " + Code;
            res += " Status : " + Status;
            res += " Customer Id : " + CustomerId;
            return base.ToString();
        }
    }

    public enum BookingStatus
    {
        Canceled,
        Validated
    }
}
