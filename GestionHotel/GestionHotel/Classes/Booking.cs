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
        private InvoiceStatus statusInvoice;

        private static SqlCommand command;

        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int RoomId { get => roomId; set => roomId = value; }
        public int OccupatedNumber { get => occupatedNumber; set => occupatedNumber = value; }
        public BookingStatus Status { get => status; set => status = value; }
        public InvoiceStatus StatusInvoice { get => statusInvoice; set => statusInvoice = value; }

        public Booking()
        {
            Code = Guid.NewGuid().ToString();
        }

        public Booking(string c)
        {
            command = new SqlCommand("SELECT * FROM Booking WHERE Code = @c", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@c", c));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                Code = reader.GetString(1);
                CustomerId = reader.GetInt32(2);
                RoomId = reader.GetInt32(3);
                OccupatedNumber = reader.GetInt32(4);
                status = (BookingStatus)reader.GetInt32(5);
                StatusInvoice = (InvoiceStatus)reader.GetInt32(6);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

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
        public bool UpdateStatus(InvoiceStatus s)
        {
            bool res = false;
            command = new SqlCommand("UPDATE Booking set InvoiceStatus = @s WHERE Id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s", s));
            command.Parameters.Add(new SqlParameter("@i", Id));
            Connection.Instance.Open();
            res = command.ExecuteNonQuery() > 0;
            command.Dispose();
            Connection.Instance.Close();
            return res;
        }

        public static List<Booking> GetBookings(int customerId)
        {
            List<Booking> res = new List<Booking>();
            command = new SqlCommand("SELECT * FROM Booking WHERE CustomerId = @c", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@c", customerId));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                Booking b = new Booking()
                {
                    Id = reader.GetInt32(0),
                    Code = reader.GetString(1),
                    CustomerId = reader.GetInt32(2),
                    RoomId = reader.GetInt32(3),
                    OccupatedNumber = reader.GetInt32(4),
                    Status = (BookingStatus)reader.GetInt32(5),
                    StatusInvoice = (InvoiceStatus) reader.GetInt32(6)
                };
                res.Add(b);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return res;
        }

        public static List<Booking> GetBookingsByHotelId(int id)
        {
            List<Booking> res = new List<Booking>();
            command = new SqlCommand("SELECT b.id, b.code, b.customerId, b.roomId, b.occupatedNumber, b.status FROM Booking as b inner join room as r on r.id = b.roomId WHERE r.HotelId = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@i", id));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Booking b = new Booking()
                {
                    Id = reader.GetInt32(0),
                    Code = reader.GetString(1),
                    CustomerId = reader.GetInt32(2),
                    RoomId = reader.GetInt32(3),
                    OccupatedNumber = reader.GetInt32(4),
                    Status = (BookingStatus)reader.GetInt32(5),
                    StatusInvoice = (InvoiceStatus)reader.GetInt32(6)
                };
                res.Add(b);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return res;
        }

        public override string ToString()
        {
            string res = "Code : " + Code;
            res += " Status : " + Status;
            res += " Customer Id : " + CustomerId;
            res += $" paiment status : {StatusInvoice}";
            return res;
        }
    }

    public enum BookingStatus
    {
        Canceled,
        Validated
    }
}
