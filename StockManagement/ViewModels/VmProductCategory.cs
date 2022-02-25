using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.ViewModels
{
    public class VmProductCategory
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public List<tblProductCategory> CategoryList { get; set; }
        public List<VmProduct> ProductList { get; set; }
    }
}