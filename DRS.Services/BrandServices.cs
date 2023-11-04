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
    public class BrandServices
    {
        #region Singleton
        public static BrandServices Instance
        {
            get
            {
                if (instance == null) instance = new BrandServices();
                return instance;
            }
        }
        private static BrandServices instance { get; set; }
        private BrandServices()
        {
        }
        #endregion
        public List<Brand> GetBrand(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.brands.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
                }
                else
                {
                    return context.brands.OrderBy(x => x.Description).ToList();
                }
            }
        }
        public int GetLastEntryId()
        {
            using (var context = new DSContext())
            {
                var lastAdjustment = context.brands.OrderByDescending(a => a.ID).FirstOrDefault();
                if (lastAdjustment != null)
                {
                    return lastAdjustment.ID;
                }
                // Return a default value (e.g., -1) or throw an exception if there are no entries.
                // You can decide the appropriate behavior based on your application requirements.
                return -1; // Default value when there are no entries.
            }
        }
        public List<string> GetBrandNames()
        {
            using (var context = new DSContext())
            {
                var data = context.brands.Select(c => c.Description).ToList();
                data.Reverse();
                return data;
            }
        }
        public Brand GetBrandInBrands(int Sentid)
        {
            using (var context = new DSContext())
            {
                var category = context.brands.FirstOrDefault(c => c.ID == Sentid);
                return category;

            }
        }
        public List<Brand> GetBrands()
        {
            using (var context = new DSContext())
            {
                var data = context.brands
                    .OrderBy(b => b.Description) // Order by Description property in ascending order
                    .ToList();
                return data;
            }
        }
        public List<Brand> GetBrands(string SearchTerm)
        {
            using (var context = new DSContext())
            {
                return context.brands.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
            }
        }



        public Entities.Brand GetBrandById(int id)
        {
            using (var context = new DSContext())
            {
                return context.brands.Find(id);

            }
        }

        public void CreateBrand(Brand Brand)
        {
            using (var context = new DSContext())
            {
                context.brands.Add(Brand);
                context.SaveChanges();
            }
        }

        public void UpdateBrand(Entities.Brand Brand)
        {
            using (var context = new DSContext())
            {
                context.Entry(Brand).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteBrand(int ID)
        {
            using (var context = new DSContext())
            {

                var Product = context.brands.Find(ID);
                context.brands.Remove(Product);
                context.SaveChanges();
            }
        }
    }
}
