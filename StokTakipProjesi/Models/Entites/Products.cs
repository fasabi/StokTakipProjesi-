//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StokTakipProjesi.Models.Entites
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string StockCode { get; set; }
        public double UnitPrice { get; set; }
        public int StockAmount { get; set; }
    
        public virtual Categories Categories { get; set; }
    }
}
