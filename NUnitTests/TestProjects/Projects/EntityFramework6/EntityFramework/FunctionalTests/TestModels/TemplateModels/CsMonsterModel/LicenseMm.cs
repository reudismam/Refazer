
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Proj8b9180bea7178d8348de47e28237c05ddb8a8244.EntityFramework.FunctionalTests.TestModels.TemplateModels.CsMonsterModel
{

using System;
    using System.Collections.Generic;
    
public partial class LicenseMm
{

    public LicenseMm()
    {

        this.LicenseClass = "C";

    }


    public string Name { get; set; }

    public string LicenseNumber { get; set; }

    public string LicenseClass { get; set; }

    public string Restrictions { get; set; }

    public System.DateTime ExpirationDate { get; set; }

    public Nullable<Another.Place.LicenseStateMm> State { get; set; }



    public virtual DriverMm Driver { get; set; }

}

}
