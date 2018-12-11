
Public Class Classes
    Class PhonePlan 'class contains phone plan properties
        Const VALUEADDEDTAX As Single = 1.2 'VAT global (might move)
        Protected ReadOnly Property MAX_MINUTES As UInteger
            Get
                MAX_MINUTES = 500
            End Get
        End Property

        Protected ReadOnly Property MAX_DATA As UInteger
            Get
                MAX_DATA = 500
            End Get
        End Property

        Protected ReadOnly Property MAX_TEXTS As UInteger
            Get
                MAX_TEXTS = 500
            End Get
        End Property
        Property TypeOfPlan As String = ""
        Property MinsUsed As UInteger = 0
        Property TextsUsed As UInteger = 0
        Property DataUsed As UInteger = 0
        Property TotalBill As String = GetBill()

        Function GetBill() As String

            Dim calc As Single

            If Me.TypeOfPlan = "3G" Then
                calc = 20.0
            Else
                calc = 30.0
            End If

            If Me.MinsUsed > MAX_MINUTES Then
                calc += GetPrices(Me.MinsUsed, MAX_MINUTES, 0.02)
            End If

            If Me.TextsUsed > MAX_TEXTS Then
                calc += GetPrices(Me.TextsUsed, MAX_TEXTS, 0.01)
            End If

            If Me.DataUsed > MAX_DATA Then
                calc += GetPrices(Me.DataUsed, MAX_DATA, 0.05)
            End If

            TotalBill = FormatCurrency(calc * VALUEADDEDTAX, 2)
            Return TotalBill
        End Function

        Private Function GetPrices(ByRef usage As UInteger, ByRef tarriff As UInteger, ByRef cost As Single) As Single 'to calculate price diff
            Return (usage - tarriff) * cost
        End Function
    End Class
    Class Customer 'class contains customer properties
        Inherits PhonePlan
        Dim _FName As String
        Dim _SName As String
        Dim _FullName As String
        Dim _ID As UInteger
        Dim _Address As String
        Dim _DOB As Date
        Dim _MobileNo As String

        Public Property custName As String
            Get
                If _SName = "" Then
                    Return _FName
                Else
                    Return _FName + " " + _SName
                End If
            End Get

            Set(value As String)

                Dim space As String = value.IndexOf(" ")
                If space < 0 Then
                    _FName = value
                    _SName = ""
                Else
                    _FName = value.Substring(0, space)
                    _SName = value.Substring(space + 1)
                End If


            End Set
        End Property

        Public Property custID As UInteger
            Get
                Return _ID
            End Get
            Set(value As UInteger)
                _ID = value
            End Set
        End Property
        Public Property custAddress As String
            Get
                Return _Address
            End Get
            Set(value As String)
                _Address = value

            End Set
        End Property
        Public Property custDOB As Date
            Get
                Return _DOB
            End Get
            Set(value As Date)
                _DOB = value
            End Set
        End Property
        Public Property custMobileNo As String
            Get
                Return _MobileNo
            End Get
            Set(value As String)
                _MobileNo = value
            End Set
        End Property




    End Class
End Class
