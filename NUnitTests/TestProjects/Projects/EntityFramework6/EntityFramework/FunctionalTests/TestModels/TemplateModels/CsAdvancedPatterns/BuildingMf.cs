
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Proj8b9180bea7178d8348de47e28237c05ddb8a8244.EntityFramework.FunctionalTests.TestModels.TemplateModels.CsAdvancedPatterns
{

using System;
    using System.Collections.Generic;
    
public partial class BuildingMf
{

    public BuildingMf()
    {

        this.Offices = new HashSet<OfficeMf>();

        this.MailRooms = new HashSet<MailRoomMf>();

        this.Address = new AddressMf();

    }


    public System.Guid BuildingId { get; set; }

    public string Name { get; set; }

    internal decimal Value { get; set; }



    internal AddressMf Address { get; private set; }



    public virtual ICollection<OfficeMf> Offices { get; set; }

    internal virtual ICollection<MailRoomMf> MailRooms { get; private set; }

}

}
