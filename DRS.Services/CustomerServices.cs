using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRS.Database;
using DRS.Entities;

namespace DRS.Services
{
    public class CustomerServices
    {
        #region Singleton
        public static CustomerServices Instance
        {
            get
            {
                if (instance == null) instance = new CustomerServices();
                return instance;
            }
        }
        private static CustomerServices instance { get; set; }
        private CustomerServices()
        {
        }
        #endregion
        public List<Customer> GetCustomer(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.customers.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
                }
                else
                {
                    return context.customers.OrderBy(x => x.Description).ToList();
                }
            }
        }
        public int GetLastEntryId()
        {
            using (var context = new DSContext())
            {
                var lastAdjustment = context.customers.OrderByDescending(a => a.ID).FirstOrDefault();
                if (lastAdjustment != null)
                {
                    return lastAdjustment.ID;
                }
                // Return a default value (e.g., -1) or throw an exception if there are no entries.
                // You can decide the appropriate behavior based on your application requirements.
                return -1; // Default value when there are no entries.
            }
        }
        public List<string> GetCustomerNames()
        {
            using (var context = new DSContext())
            {
                var data = context.customers.Select(c => c.Description).ToList();
                data.Reverse();
                return data;
            }
        }
        public Customer GetCustomerInCustomers(int Sentid)
        {
            using (var context = new DSContext())
            {
                var category = context.customers.FirstOrDefault(c => c.ID == Sentid);
                return category;

            }
        }
        public List<Customer> GetCustomers()
        {
            using (var context = new DSContext())
            {
                var data = context.customers
                    .OrderBy(b => b.Description)
                    .ToList();
                return data;
            }
        }
        public List<Customer> GetCustomers(string SearchTerm)
        {
            using (var context = new DSContext())
            {
                return context.customers.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
            }
        }



        public Entities.Customer GetCustomerById(int id)
        {
            using (var context = new DSContext())
            {
                return context.customers.Find(id);

            }
        }

        public void CreateCustomer(Customer Customer)
        {
            using (var context = new DSContext())
            {
                context.customers.Add(Customer);
                context.SaveChanges();
            }
        }

        public void UpdateCustomer(Entities.Customer Customer)
        {
            using (var context = new DSContext())
            {
                context.Entry(Customer).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteCustomer(int ID)
        {
            using (var context = new DSContext())
            {

                var Product = context.customers.Find(ID);
                context.customers.Remove(Product);
                context.SaveChanges();
            }
        }
    }
}
