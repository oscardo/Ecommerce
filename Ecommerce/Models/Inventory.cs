using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public double Stock { get; set; }

        //un inventario puede tener varios warehouse
        public virtual Warehouse Warehouse { get; set; }
        //un inventario puede tener varios Product
        public virtual Product Product { get; set; }
        
    }
}