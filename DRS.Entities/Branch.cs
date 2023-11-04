using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Entities
{
    public class Branch: BaseEntity
    {
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}
