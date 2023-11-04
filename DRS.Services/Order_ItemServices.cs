using DRS.Database;
using DRS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Services
{
    public class Order_ItemServices
    {
        #region Singleton
        public static Order_ItemServices Instance
        {
            get
            {
                if (instance == null) instance = new Order_ItemServices();
                return instance;
            }
        }
        private static Order_ItemServices instance { get; set; }
        private Order_ItemServices()
        {
        }
        #endregion
        public List<Order_Item> GetOrder_Item(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.orderitems.Where(p => p.Note != null && p.Note.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Note)
                                            .ToList();
                }
                else
                {
                    return context.orderitems.OrderBy(x => x.Note).ToList();
                }
            }
        }

        public Order_Item GetItemsByOrderID(int ID)
        {
            using (var context = new DSContext())
            {
                var data = context.orderitems.Where(x => x.IDOrder == ID).FirstOrDefault();       
                return data;
            }
        }
        public List<string> GetOrder_ItemNames()
        {
            using (var context = new DSContext())
            {
                var data = context.orderitems.Select(c => c.Note).ToList();
                data.Reverse();
                return data;
            }
        }
        public Order_Item GetOrder_ItemInOrder_Items(int Sentid)
        {
            using (var context = new DSContext())
            {
                var category = context.orderitems.FirstOrDefault(c => c.ID == Sentid);
                return category;

            }
        }
        public List<Order_Item> GetOrder_Items()
        {
            using (var context = new DSContext())
            {
                var data = context.orderitems.ToList();
                data.Reverse();
                return data;
            }
        }
        public List<Order_Item> GetOrder_Items(string SearchTerm)
        {
            using (var context = new DSContext())
            {
                return context.orderitems.Where(p => p.Note != null && p.Note.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Note)
                                            .ToList();
            }
        }

        public Entities.Order_Item GetOrder_ItemById(int id)
        {
            using (var context = new DSContext())
            {
                return context.orderitems.Find(id);

            }
        }

        public void CreateOrder_Item(Order_Item Order_Item)
        {
            using (var context = new DSContext())
            {
                context.orderitems.Add(Order_Item);
                context.SaveChanges();
            }
        }

        public void UpdateOrder_Item(Entities.Order_Item Order_Item)
        {
            using (var context = new DSContext())
            {
                context.Entry(Order_Item).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteOrder_Item(int ID)
        {
            using (var context = new DSContext())
            {

                var Product = context.orderitems.Find(ID);
                context.orderitems.Remove(Product);
                context.SaveChanges();
            }
        }
    }
}
