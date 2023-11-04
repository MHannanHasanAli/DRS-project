using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DRS.Entities;

namespace DRS.ViewModels
{
    public class BrandViewModel
    {
    }
    public class BrandListingViewModel
    {
        public List<Brand> Brands { get; set; }
    }

    public class BrandActionViewModel
    {
        public int ID { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
    }
}