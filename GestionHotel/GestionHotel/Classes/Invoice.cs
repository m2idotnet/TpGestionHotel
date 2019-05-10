using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GestionHotel.Classes
{
    public class Invoice
    {
        private int id;
        private int customerId;
        private decimal price;
        private InvoiceStatus status;
        private static SqlCommand command;

        public int Id { get => id; set => id = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public decimal Price { get => price; set => price = value; }
        public InvoiceStatus Status { get => status; set => status = value; }

        public bool Save()
        {
            bool res = false;
            command = new SqlCommand("INSERT INTO invoice(customerId, price, status) OUTPUT INSERTED.ID values(@c, @p, @s)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@c", CustomerId));
            command.Parameters.Add(new SqlParameter("@p", Price));
            command.Parameters.Add(new SqlParameter("@s", Status));
            
            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            res = (Id > 0);
            return res;
        }
    }

    public enum InvoiceStatus
    {
        paid,
        notPaid
    }
}
