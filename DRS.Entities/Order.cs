using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Entities
{
    public class Order: BaseEntity
    {
        public int IDBranch { get; set; }
        public string IDUser { get; set; }
        public int IDCustomer { get; set; }
        public int IDBrand { get; set; }
        public int IDSupplier{ get; set; }
        public string Plate { get; set; }
        public string Chassis { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Reminder1 { get; set; }
        public DateTime? Reminder2 { get; set; }
        public DateTime? Reminder3 { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? Unavailability { get; set; }
        public string Received { get; set; }
        public string Attachment { get; set; }
        public string AlternativeCode { get; set; }
        public string File { get; set; }
        public string Confirmation { get; set; }
    }
}
