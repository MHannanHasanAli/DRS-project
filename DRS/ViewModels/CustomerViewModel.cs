using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DRS.Entities;

namespace DRS.ViewModels
{
    public class CustomerViewModel
    {
    }
    public class CustomerListingViewModel
    {
        public List<Customer> Customers { get; set; }
    }

    public class CustomerActionViewModel
    {
        public int ID { get; set; }
        public string Logo { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string Erp { get; set; }

    }
}