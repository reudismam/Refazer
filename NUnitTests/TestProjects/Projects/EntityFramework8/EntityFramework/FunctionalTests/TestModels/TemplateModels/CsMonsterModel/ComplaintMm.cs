
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Projed9469e53a99d7f7f46176906582f04423619183.EntityFramework.FunctionalTests.TestModels.TemplateModels.CsMonsterModel
{

using System;
    using System.Collections.Generic;
    
public partial class ComplaintMm
{

    public int ComplaintId { get; set; }

    public Nullable<int> CustomerId { get; set; }

    public System.DateTime Logged { get; set; }

    public string Details { get; set; }



    public virtual Another.Place.CustomerMm Customer { get; set; }

    public virtual ResolutionMm Resolution { get; set; }

}

}
