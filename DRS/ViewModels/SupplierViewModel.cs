using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DRS.Entities;

namespace DRS.ViewModels
{
    public class SupplierViewModel
    {
    }
    public class SupplierListingViewModel
    {
        public List<Supplier> Suppliers { get; set; }
    }

    public class SupplierActionViewModel
    {
        public int ID { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Note { get; set; }
    }
}