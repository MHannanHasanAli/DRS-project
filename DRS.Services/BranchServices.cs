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
    public class BranchServices
    {
        #region Singleton
        public static BranchServices Instance
        {
            get
            {
                if (instance == null) instance = new BranchServices();
                return instance;
            }
        }
        private static BranchServices instance { get; set; }
        private BranchServices()
        {
        }
        #endregion
        public List<Branch> GetBranch(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.branches.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
                }
                else
                {
                    return context.branches.OrderBy(x => x.Description).ToList();
                }
            }
        }
        public int GetLastEntryId()
        {
            using (var context = new DSContext())
            {
                var lastAdjustment = context.branches.OrderByDescending(a => a.ID).FirstOrDefault();
                if (lastAdjustment != null)
                {
                    return lastAdjustment.ID;
                }
                // Return a default value (e.g., -1) or throw an exception if there are no entries.
                // You can decide the appropriate behavior based on your application requirements.
                return -1; // Default value when there are no entries.
            }
        }
        public List<string> GetBranchNames()
        {
            using (var context = new DSContext())
            {
                var data = context.branches.Select(c => c.Description).ToList();
                data.Reverse();
                return data;
            }
        }
        public Branch GetBranchInBranchs(int Sentid)
        {
            using (var context = new DSContext())
            {
                var category = context.branches.FirstOrDefault(c => c.ID == Sentid);
                return category;

            }
        }
        public List<Branch> GetBranchs()
        {
            using (var context = new DSContext())
            {
                var data = context.branches
                    .OrderBy(b => b.Description)
                    .ToList();
                return data;
            }
        }
        public List<Branch> GetBranchs(string SearchTerm)
        {
            using (var context = new DSContext())
            {
                return context.branches.Where(p => p.Description != null && p.Description.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.Description)
                                            .ToList();
            }
        }



        public Entities.Branch GetBranchById(int id)
        {
            using (var context = new DSContext())
            {
                return context.branches.Find(id);

            }
        }

        public void CreateBranch(Branch Branch)
        {
            using (var context = new DSContext())
            {
                context.branches.Add(Branch);
                context.SaveChanges();
            }
        }

        public void UpdateBranch(Entities.Branch Branch)
        {
            using (var context = new DSContext())
            {
                context.Entry(Branch).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteBranch(int ID)
        {
            using (var context = new DSContext())
            {

                var Product = context.branches.Find(ID);
                context.branches.Remove(Product);
                context.SaveChanges();
            }
        }
    }
}
