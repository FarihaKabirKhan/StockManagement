using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.ViewModels
{
    public class VmProduct
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public int categoryID { get; set; }
        public decimal salesPrice { get; set; }
        public Nullable<decimal> purchasePrice { get; set; }
        public string ImagePath { get; set; }
        public string status { get; set; }
        public string categoryName { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }
        public List<tblProductCategory> CategoryList { get; set; }
    }
}