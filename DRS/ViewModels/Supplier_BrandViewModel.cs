using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DRS.Entities;

namespace DRS.ViewModels
{
    public class Supplier_BrandViewModel
    {
    }
    public class Supplier_BrandListingViewModel
    {
        public List<DisplayModelForRelation> BrandListing { get; set; }
           
        //public string BrandLogo { get; set; }
        //public string BrandDescription { get; set; }
        //public string SupplierDescription { get; set; }
        //public string Default { get; set; }
        //public string Note { get; set; }

    }
    public class DisplayModelForRelation
    {
        public int ID { get; set; }
        public string BrandLogo { get; set; }
        public string BrandDescription { get; set; }
        public string SupplierDescription { get; set; }
        public string Default { get; set; }
        public string Note { get; set; }
    }

    public class Supplier_BrandActionViewModel
    {
        public List<Branch> Branches { get; set; }
        public List<Customer> Customer { get; set; }
        public List<Supplier> Supplier { get; set; }
        public List<Brand> Brands { get; set; }

        public int ID { get; set; }
        public int IDSupplier { get; set; }
        public int IDBrand { get; set; }
        public string Default { get; set; }
        public string Note { get; set; }

    }
}