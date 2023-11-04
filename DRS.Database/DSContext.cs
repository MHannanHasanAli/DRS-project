using Microsoft.AspNet.Identity.EntityFramework;
using DRS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Database
{
    public class DSContext :IdentityDbContext<User>,IDisposable
    {
        public DSContext() : base("DSConnectionStrings")
        {

        }

        public static DSContext Create()
        {
            return new DSContext();
        }

        public DbSet<Brand> brands { get; set; }
        public DbSet<Branch> branches { get; set; }

        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<Supplier_Brand> supplierbrands { get; set; }
        public DbSet<Customer> customers { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<Order_Item> orderitems { get; set; }



    }
}
