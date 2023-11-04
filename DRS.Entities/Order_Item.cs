using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Entities
{
    public class Order_Item: BaseEntity
    {
        public int IDOrder { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }
}
