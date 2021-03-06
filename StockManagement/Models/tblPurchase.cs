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
    
    public partial class tblPurchase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPurchase()
        {
            this.tblStocks = new HashSet<tblStock>();
        }
    
        public int purchaseID { get; set; }
        public int productID { get; set; }
        public int supplierID { get; set; }
        public int warehouseID { get; set; }
        public decimal purchasePrice { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public System.DateTime purchaseDate { get; set; }
        public string description { get; set; }
    
        public virtual tblProduct tblProduct { get; set; }
        public virtual tblSupplier tblSupplier { get; set; }
        public virtual tblWarehouse tblWarehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblStock> tblStocks { get; set; }
    }
}
