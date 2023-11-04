using DRS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DRS.ViewModels
{
    public class OrderViewModel
    {
    }
    public class OrderListingViewModel
    {
        public List<OrderIndex> Order { get; set; }
    }
    public class OrderIndex
    {
        public string Branch { get; set; }
        public string User { get; set; }
        public string Customer { get; set; }
        public string Brand { get; set; }
        public string Supplier { get; set; }
        public string Plate { get; set; }
        public string Chassis { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Reminder1 { get; set; }
        public DateTime? Reminder2 { get; set; }
        public DateTime? Reminder3 { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int IDOrder { get; set; }
        public int IDItem { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string NoteItem { get; set; }
        public string Attachment { get; set; }
        public string AlternativeCode { get; set; }
        public string Alias{ get; set; }
        public int? Unavailability { get; set; }
        public string Photo { get; set; }

        public string Received { get; set; }
    }
    public class OrderActionViewModel
    {
        public List<Branch> Branches { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Brand> Brands { get; set; }
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public int? Unavailability { get; set; }
        public int IDBranch { get; set; }
        public string IDUser { get; set; }
        public int IDCustomer { get; set; }
        public int IDBrand { get; set; }
        public int IDSupplier { get; set; }
        public string Plate { get; set; }
        public string Chassis { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? Reminder1 { get; set; }
        public DateTime? Reminder2 { get; set; }
        public DateTime? Reminder3 { get; set; }
        public string AlternativeCode { get; set; }
        public string Received { get; set; }
        public string Attachment { get; set; }
        public int IDItem { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string NoteItem { get; set; }
        public string Photo { get; set; }

    }

    public class OrderProductModel
    {
        public string ItemId { get; set; }
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Date { get; set; }
    }
}