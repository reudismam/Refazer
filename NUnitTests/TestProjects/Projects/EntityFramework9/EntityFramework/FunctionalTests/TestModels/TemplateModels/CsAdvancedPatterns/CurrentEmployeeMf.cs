
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Proj326d525b1d7d8e1c95f7227be63238ec783a9e92.EntityFramework.FunctionalTests.TestModels.TemplateModels.CsAdvancedPatterns
{

using System;
    using System.Collections.Generic;
    
public partial class CurrentEmployeeMf : EmployeeMf
{

    public Nullable<decimal> LeaveBalance { get; set; }



    public virtual CurrentEmployeeMf Manager { get; set; }

    public virtual OfficeMf Office { get; set; }

}

}