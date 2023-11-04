using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Entities
{
    public class Supplier:BaseEntity
    {
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Note { get; set; }

    }
}
