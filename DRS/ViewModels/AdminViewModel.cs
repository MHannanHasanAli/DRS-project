using DRS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DRS.ViewModels
{
    public class AdminViewModel
    {
        public User SignedInUser { get; set; }
        public string Name { get; set; }

        public string ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string SearchTerm { get; set; }


        

    }

    
}

