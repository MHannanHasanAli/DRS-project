using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Entities
{
    public class Supplier_Brand:BaseEntity
    {
        public int IDSupplier { get; set; }
        public int IDBrand { get; set; }
        public string Default { get; set; }
        public string Note{ get; set; }
    }
}
