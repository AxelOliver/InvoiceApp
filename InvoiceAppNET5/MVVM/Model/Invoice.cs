using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAppNET5.MVVM.Model
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Invoice()
        {

        }
        public Invoice( int clientId, string description, decimal hourlyRate, DateTime date, string startTime, string endTime)
        {
            ClientId = clientId;
            Description = description;
            HourlyRate = hourlyRate;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            int startHour = int.Parse(StartTime.Split(':')[0]);
            decimal startMinutes = decimal.Parse(StartTime.Split(':')[1]);
            int endHour = int.Parse(EndTime.Split(':')[0]);
            decimal endMinutes = decimal.Parse(EndTime.Split(':')[1]);
            decimal start = startHour + (startMinutes / 60);
            decimal end = endHour + (endMinutes / 60);
            HoursWorked = end - start;
            TotalCost = Math.Round(HoursWorked * HourlyRate, 2);
        }
    }
}
