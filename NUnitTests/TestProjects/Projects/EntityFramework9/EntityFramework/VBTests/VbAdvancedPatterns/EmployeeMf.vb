
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


Namespace Proj326d525b1d7d8e1c95f7227be63238ec783a9e92.EntityFramework.VBTests.VbAdvancedPatterns


Partial Public MustInherit Class EmployeeMf

    Public Property EmployeeId As Integer


        Private _FirstName As String
        Public Property FirstName As String
            Get
                Return _FirstName
            End Get
            Private Set(ByVal value As String)
                _FirstName = value
            End Set
        End Property


        Private _LastName As String
        Friend Property LastName As String
            Private Get
                Return _LastName
            End Get
            Set(ByVal value As String)
                _LastName = value
            End Set
        End Property


End Class


End Namespace
