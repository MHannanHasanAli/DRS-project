using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DRS.Entities;

namespace DRS.ViewModels
{
    public class BranchViewModel
    {
    }
    public class BranchListingViewModel
    {
        public List<Branch> Branches { get; set; }
    }

    public class BranchActionViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}