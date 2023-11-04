using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRS.Entities
{
    public class Brand:BaseEntity
    {
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

    }
}
