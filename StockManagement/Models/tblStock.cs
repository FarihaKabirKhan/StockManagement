//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblStock
    {
        public int stockID { get; set; }
        public int productID { get; set; }
        public int purchaseID { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
    
        public virtual tblProduct tblProduct { get; set; }
        public virtual tblPurchase tblPurchase { get; set; }
    }
}
