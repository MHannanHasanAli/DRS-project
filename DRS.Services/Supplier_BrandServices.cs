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
    public class Supplier_BrandServices
    {
        #region Singleton
        public static Supplier_BrandServices Instance
        {
            get
            {
                if (instance == null) instance = new Supplier_BrandServices();
                return instance;
            }
        }
        private static Supplier_BrandServices instance { get; set; }
        private Supplier_BrandServices()
        {
        }
        #endregion
        public List<Supplier_Brand> GetSupplier_Brand(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.supplierbrands.Where(p => p.Note != null && p.Note.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Note)
                                            .ToList();
                }
                else
                {
                    return context.supplierbrands.OrderBy(x => x.Note).ToList();
                }
            }
        }
        public List<string> GetSupplier_BrandNames()
        {
            using (var context = new DSContext())
            {
                var data = context.supplierbrands.Select(c => c.Note).ToList();
                data.Reverse();
                return data;
            }
        }
        public Supplier_Brand GetSupplier_BrandInSupplier_Brands(int Sentid)
        {
            using (var context = new DSContext())
            {
                var category = context.supplierbrands.FirstOrDefault(c => c.ID == Sentid);
                return category;

            }
        }
        public List<Supplier_Brand> GetSupplier_BrandsByBrandID(int idBrand)
        {
            using (var context = new DSContext())
            {
                var data = context.supplierbrands.Where(sb => sb.IDBrand == idBrand).ToList();
                data.Reverse();
                return data;
            }
        }
        public List<Supplier_Brand> GetSupplier_Brands()
        {
            using (var context = new DSContext())
            {
                var data = context.supplierbrands
                    .OrderBy(b => b.IDBrand)
                    .ToList();
                return data;
            }
        }
        public List<Supplier_Brand> GetSupplier_Brands(string SearchTerm)
        {
            using (var context = new DSContext())
            {
                return context.supplierbrands.Where(p => p.Note != null && p.Note.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Note)
                                            .ToList();
            }
        }



        public Entities.Supplier_Brand GetSupplier_BrandById(int id)
        {
            using (var context = new DSContext())
            {
                return context.supplierbrands.Find(id);

            }
        }

        public void CreateSupplier_Brand(Supplier_Brand Supplier_Brand)
        {
            using (var context = new DSContext())
            {
                context.supplierbrands.Add(Supplier_Brand);
                context.SaveChanges();
            }
        }

        public void UpdateSupplier_Brand(Entities.Supplier_Brand Supplier_Brand)
        {
            using (var context = new DSContext())
            {
                context.Entry(Supplier_Brand).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteSupplier_Brand(int ID)
        {
            using (var context = new DSContext())
            {

                var Product = context.supplierbrands.Find(ID);
                context.supplierbrands.Remove(Product);
                context.SaveChanges();
            }
        }
    }
}
