using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.ViewModels
{
    public class DashboardViewModel
    {
        public int customer_count { get; set; }
        public int product_count { get; set; }
        public int supplier_count { get; set; }
        public int warehouse_count { get; set; }
        public int purchase_count { get; set; }
        public int sales_count { get; set; }
        public int stock_count { get; set; }
    }
}