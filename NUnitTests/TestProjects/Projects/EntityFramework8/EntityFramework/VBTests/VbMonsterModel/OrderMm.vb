
'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic



Namespace Projed9469e53a99d7f7f46176906582f04423619183.EntityFramework.VBTests.VbMonsterModel


Partial Public Class OrderMm

    Public Property OrderId As Integer

    Public Property CustomerId As Nullable(Of Integer)



    Public Property Concurrency As ConcurrencyInfoMm = New ConcurrencyInfoMm



    Public Overridable Property Customer As Another.Place.CustomerMm

    Public Overridable Property OrderLines As ICollection(Of OrderLineMm) = New HashSet(Of OrderLineMm)

    Public Overridable Property Notes As ICollection(Of OrderNoteMm) = New HashSet(Of OrderNoteMm)

    Public Overridable Property Login As Another.Place.LoginMm


End Class



End Namespace
