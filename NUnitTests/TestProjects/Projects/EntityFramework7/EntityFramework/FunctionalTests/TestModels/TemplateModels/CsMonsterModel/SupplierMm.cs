
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Proj829dec5d15c3d930815d72ce2d6909c51a97b1ef.EntityFramework.FunctionalTests.TestModels.TemplateModels.CsMonsterModel
{

using System;
    using System.Collections.Generic;
    
public partial class SupplierMm
{

    public SupplierMm()
    {

        this.Products = new HashSet<ProductMm>();

        this.BackOrderLines = new HashSet<BackOrderLineMm>();

    }


    public int SupplierId { get; set; }

    public string Name { get; set; }



    public virtual ICollection<ProductMm> Products { get; set; }

    public virtual ICollection<BackOrderLineMm> BackOrderLines { get; set; }

    public virtual SupplierLogoMm Logo { get; set; }

}

}
