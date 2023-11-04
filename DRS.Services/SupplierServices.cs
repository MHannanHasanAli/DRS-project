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
    public class SupplierServices
    {
        #region Singleton
        public static SupplierServices Instance
        {
            get
            {
                if (instance == null) instance = new SupplierServices();
                return instance;
            }
        }
        private static SupplierServices instance { get; set; }
        private SupplierServices()
        {
        }
        #endregion
        public List<Supplier> GetSupplier(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.suppliers.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
                }
                else
                {
                    return context.suppliers.OrderBy(x => x.Description).ToList();
                }
            }
        }
        public int GetLastEntryId()
        {
            using (var context = new DSContext())
            {
                var lastAdjustment = context.suppliers.OrderByDescending(a => a.ID).FirstOrDefault();
                if (lastAdjustment != null)
                {
                    return lastAdjustment.ID;
                }
                // Return a default value (e.g., -1) or throw an exception if there are no entries.
                // You can decide the appropriate behavior based on your application requirements.
                return -1; // Default value when there are no entries.
            }
        }
        public List<string> GetSupplierNames()
        {
            using (var context = new DSContext())
            {
                var data = context.suppliers.Select(c => c.Description).ToList();
                data.Reverse();
                return data;
            }
        }
        public Supplier GetSupplierInSuppliers(int Sentid)
        {
            using (var context = new DSContext())
            {
                var category = context.suppliers.FirstOrDefault(c => c.ID == Sentid);
                return category;

            }
        }
        public List<Supplier> GetSuppliers()
        {
            using (var context = new DSContext())
            {
                var data = context.suppliers
                .OrderBy(b => b.Description)
                .ToList();
                return data;
            }
        }
        public List<Supplier> GetSuppliers(string SearchTerm)
        {
            using (var context = new DSContext())
            {
                return context.suppliers.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
            }
        }



        public Entities.Supplier GetSupplierById(int id)
        {
            using (var context = new DSContext())
            {
                return context.suppliers.Find(id);

            }
        }

        public void CreateSupplier(Supplier Supplier)
        {
            using (var context = new DSContext())
            {
                context.suppliers.Add(Supplier);
                context.SaveChanges();
            }
        }

        public void UpdateSupplier(Entities.Supplier Supplier)
        {
            using (var context = new DSContext())
            {
                context.Entry(Supplier).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteSupplier(int ID)
        {
            using (var context = new DSContext())
            {

                var Product = context.suppliers.Find(ID);
                context.suppliers.Remove(Product);
                context.SaveChanges();
            }
        }
    }
}
