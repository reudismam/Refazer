

























//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7.TestModels.TemplateModels.CsAdvancedPatterns
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;


internal partial class AdvancedPatternsModelFirstContext : DbContext
{
    public AdvancedPatternsModelFirstContext()
        : base("name=AdvancedPatternsModelFirstContext")
    {

        this.Configuration.LazyLoadingEnabled = false;

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<EmployeeMf> Employees { get; set; }

    public DbSet<OfficeMf> Offices { get; set; }

    public DbSet<BuildingMf> Buildings { get; set; }

    internal DbSet<MailRoomMf> MailRooms { get; set; }

    public DbSet<WhiteboardMf> Whiteboards { get; set; }

    public DbSet<BuildingDetailMf> BuildingDetails { get; set; }

    public DbSet<WorkOrderMf> WorkOrders { get; set; }


    public virtual ObjectResult<OfficeMf> AllOfficesStoredProc()
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<OfficeMf>("AllOfficesStoredProc");
    }


    public virtual ObjectResult<OfficeMf> AllOfficesStoredProc(MergeOption mergeOption)
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<OfficeMf>("AllOfficesStoredProc", mergeOption);
    }


    public virtual ObjectResult<Nullable<int>> EmployeeIdsInOfficeStoredProc(string officeNumber, Nullable<System.Guid> buildingId)
    {

        var officeNumberParameter = officeNumber != null ?
            new ObjectParameter("OfficeNumber", officeNumber) :
            new ObjectParameter("OfficeNumber", typeof(string));


        var buildingIdParameter = buildingId.HasValue ?
            new ObjectParameter("BuildingId", buildingId) :
            new ObjectParameter("BuildingId", typeof(System.Guid));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("EmployeeIdsInOfficeStoredProc", officeNumberParameter, buildingIdParameter);
    }


    public virtual ObjectResult<OfficeMf> OfficesInBuildingStoredProc(Nullable<System.Guid> buildingId)
    {

        var buildingIdParameter = buildingId.HasValue ?
            new ObjectParameter("BuildingId", buildingId) :
            new ObjectParameter("BuildingId", typeof(System.Guid));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<OfficeMf>("OfficesInBuildingStoredProc", buildingIdParameter);
    }


    public virtual ObjectResult<OfficeMf> OfficesInBuildingStoredProc(Nullable<System.Guid> buildingId, MergeOption mergeOption)
    {

        var buildingIdParameter = buildingId.HasValue ?
            new ObjectParameter("BuildingId", buildingId) :
            new ObjectParameter("BuildingId", typeof(System.Guid));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<OfficeMf>("OfficesInBuildingStoredProc", mergeOption, buildingIdParameter);
    }


    public virtual int SkimOffLeaveBalanceStoredProc(string first, string last)
    {

        var firstParameter = first != null ?
            new ObjectParameter("First", first) :
            new ObjectParameter("First", typeof(string));


        var lastParameter = last != null ?
            new ObjectParameter("Last", last) :
            new ObjectParameter("Last", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SkimOffLeaveBalanceStoredProc", firstParameter, lastParameter);
    }


    public virtual ObjectResult<SiteInfoMf> AllSiteInfoStoredProc()
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SiteInfoMf>("AllSiteInfoStoredProc");
    }

}

}

