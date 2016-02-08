

'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Imports System.Data.Entity.Core.Objects
Imports System.Data.Entity.Core.Objects.DataClasses
Imports System.Linq



Namespace Proj829dec5d15c3d930815d72ce2d6909c51a97b1ef.EntityFramework.VBTests.VbMonsterModel


Partial Public Class MonsterModel
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=MonsterModel")

    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub


    Public Property Customer() As DbSet(Of Another.Place.CustomerMm)

    Public Property Barcode() As DbSet(Of BarcodeMm)

    Public Property IncorrectScan() As DbSet(Of IncorrectScanMm)

    Public Property BarcodeDetail() As DbSet(Of BarcodeDetailMm)

    Public Property Complaint() As DbSet(Of ComplaintMm)

    Public Property Resolution() As DbSet(Of ResolutionMm)

    Public Property Login() As DbSet(Of Another.Place.LoginMm)

    Public Property SuspiciousActivity() As DbSet(Of SuspiciousActivityMm)

    Public Property SmartCard() As DbSet(Of SmartCardMm)

    Public Property RSAToken() As DbSet(Of RSATokenMm)

    Public Property PasswordReset() As DbSet(Of PasswordResetMm)

    Public Property PageView() As DbSet(Of PageViewMm)

    Public Property LastLogin() As DbSet(Of LastLoginMm)

    Public Property Message() As DbSet(Of MessageMm)

    Public Property Order() As DbSet(Of OrderMm)

    Public Property OrderNote() As DbSet(Of OrderNoteMm)

    Public Property OrderQualityCheck() As DbSet(Of OrderQualityCheckMm)

    Public Property OrderLine() As DbSet(Of OrderLineMm)

    Public Property Product() As DbSet(Of ProductMm)

    Public Property ProductDetail() As DbSet(Of ProductDetailMm)

    Public Property ProductReview() As DbSet(Of ProductReviewMm)

    Public Property ProductPhoto() As DbSet(Of ProductPhotoMm)

    Public Property ProductWebFeature() As DbSet(Of ProductWebFeatureMm)

    Public Property Supplier() As DbSet(Of SupplierMm)

    Public Property SupplierLogo() As DbSet(Of SupplierLogoMm)

    Public Property SupplierInfo() As DbSet(Of SupplierInfoMm)

    Public Property CustomerInfo() As DbSet(Of CustomerInfoMm)

    Public Property Computer() As DbSet(Of ComputerMm)

    Public Property ComputerDetail() As DbSet(Of ComputerDetailMm)

    Public Property Driver() As DbSet(Of DriverMm)

    Public Property License() As DbSet(Of LicenseMm)


    Private Function FunctionImport1(modifiedDate As ObjectParameter) As ObjectResult(Of Another.Place.AuditInfoMm)

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of Another.Place.AuditInfoMm)("FunctionImport1", modifiedDate)
    End Function


    Friend Overridable Function FunctionImport2() As ObjectResult(Of Another.Place.CustomerMm)

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of Another.Place.CustomerMm)("FunctionImport2")
    End Function


    Friend Overridable Function FunctionImport2(mergeOption As MergeOption) As ObjectResult(Of Another.Place.CustomerMm)

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of Another.Place.CustomerMm)("FunctionImport2", mergeOption)
    End Function


    Public Overridable Function ParameterTest(binary As Byte(), bool As Nullable(Of Boolean), dateTime As Nullable(Of Date), [decimal] As Nullable(Of Decimal), float As Nullable(Of Double), guid As Nullable(Of System.Guid), int As ObjectParameter, [string] As ObjectParameter) As Integer

        Dim binaryParameter As ObjectParameter = If(binary IsNot Nothing, New ObjectParameter("binary", binary), New ObjectParameter("binary", GetType(Byte())))


        Dim boolParameter As ObjectParameter = If(bool.HasValue, New ObjectParameter("bool", bool), New ObjectParameter("bool", GetType(Boolean)))


        Dim dateTimeParameter As ObjectParameter = If(dateTime.HasValue, New ObjectParameter("dateTime", dateTime), New ObjectParameter("dateTime", GetType(Date)))


        Dim decimalParameter As ObjectParameter = If([decimal].HasValue, New ObjectParameter("decimal", [decimal]), New ObjectParameter("decimal", GetType(Decimal)))


        Dim floatParameter As ObjectParameter = If(float.HasValue, New ObjectParameter("float", float), New ObjectParameter("float", GetType(Double)))


        Dim guidParameter As ObjectParameter = If(guid.HasValue, New ObjectParameter("guid", guid), New ObjectParameter("guid", GetType(System.Guid)))


        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("ParameterTest", binaryParameter, boolParameter, dateTimeParameter, decimalParameter, floatParameter, guidParameter, int, [string])
    End Function


    Public Overridable Function EntityTypeTest() As ObjectResult(Of ComputerMm)

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of ComputerMm)("EntityTypeTest")
    End Function


    Public Overridable Function EntityTypeTest(mergeOption As MergeOption) As ObjectResult(Of ComputerMm)

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of ComputerMm)("EntityTypeTest", mergeOption)
    End Function


End Class


End Namespace

