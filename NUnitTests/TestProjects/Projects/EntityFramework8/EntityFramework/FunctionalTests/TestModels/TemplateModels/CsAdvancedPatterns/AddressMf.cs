
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Projed9469e53a99d7f7f46176906582f04423619183.EntityFramework.FunctionalTests.TestModels.TemplateModels.CsAdvancedPatterns
{

using System;
    
public partial class AddressMf
{

    public AddressMf()
    {

        this.SiteInfo = new SiteInfoMf();

    }


    public string Street { get; set; }

    public string City { private get; set; }

    internal string State { get; set; }

    internal string ZipCode { get; private set; }



    internal SiteInfoMf SiteInfo { get; private set; }

}

}
