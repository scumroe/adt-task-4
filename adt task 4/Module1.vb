Imports System.Globalization
Imports System.Reflection
Module Module1
    Public CustomerList As New List(Of Customer)

    Const VALUEADDEDTAX As Single = 1.2
    Public Sub Main()

        Randomize()
        Dim intOption As UInteger = 0

        Do
            Try
                DisplayMenu("mm")

                intOption = GetChoice()

                Select Case intOption
                    Case 0
                        Exit Sub
                    Case 1
                        ViewCustomers()
                    Case 2
                        CreateNewCustomer()
                    Case 3
                        Console.WriteLine("Works.")
                    Case Else
                        Console.WriteLine("Invalid.")
                End Select

            Catch ex As System.FormatException
                Console.WriteLine("Format exception. Returning to main menu.")
            End Try
        Loop
        Console.Read()
    End Sub

    Class Customer
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
                If Char.IsLower(value, 1) Then
                    Char.ToUpper(value(1))
                End If

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

    Class PhonePlan
        Dim _isOverLimit As Boolean = False

        ReadOnly Property MAX_MINUTES As UInteger
            Get
                MAX_MINUTES = 500
            End Get
        End Property
        ReadOnly Property MAX_DATA As UInteger
            Get
                MAX_DATA = 500
            End Get
        End Property
        ReadOnly Property MAX_TEXTS As UInteger
            Get
                MAX_TEXTS = 500
            End Get
        End Property
        Property TypeOfPlan As String = ""
        Property MinsUsed As UInteger = 0
        Property TextsUsed As UInteger = 0
        Property DataUsed As String = 0

    End Class


    Sub DisplayMenu(Optional ByVal str As String = "mm")
        Select Case str
            Case "mm"
                Console.WriteLine("Main menu -- Enter corrosponding number: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. View Customers")
                Console.WriteLine("2. Create New Customer")
                Console.WriteLine("3. View Plan Details")
                Console.WriteLine("0. Exit program")
                Console.WriteLine()
            Case "vc"
                Console.WriteLine("1. View customers -- enter a corrosponding number: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. Show all customers.")
                Console.WriteLine("2. Search for customers.")
                Console.WriteLine("0. Exit menu.")
            Case "sc"
                Console.WriteLine("2. Search for customers -- enter up to two corrosponding number: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. Search by ID")
                Console.WriteLine("2. Search by Name")
                Console.WriteLine("3. Search by DOB")
        End Select
    End Sub

    Function GetChoice() As Integer 'Get menu choice from key
        Dim cki As ConsoleKeyInfo = Console.ReadKey
        Dim i As UInteger = UInteger.Parse(cki.KeyChar)
        Console.Clear()
        Return i
    End Function

    Public Sub CreateNewCustomer()
        Dim newcustomer As New Customer
        Dim isFinished As Boolean = False

        Do
            Console.WriteLine("2. Add New Customer: ")
            Console.WriteLine("=========================================")
            newcustomer.custID = GetUniqueID()
            Console.WriteLine("Customer ID is {0}: ", newcustomer.custID)

            newcustomer.custMobileNo = "07" + GetUniqueID.ToString
            Console.WriteLine("Customer mobile no. is {0}. ", newcustomer.custMobileNo)


            Do
                Console.WriteLine("Enter the customer name: ")
                Dim temp As String = Console.ReadLine
                If temp.Length < 5 Then
                    Console.WriteLine("Bad length, try again.")
                Else
                    newcustomer.custName = temp
                    Exit Do
                End If
            Loop

            Console.WriteLine("Enter customer address: ")
            newcustomer.custAddress = Console.ReadLine

            Console.WriteLine("Enter customer date as 'DD/MM/YYYY'")
            Dim DateString As String = Console.ReadLine
            Date.TryParseExact(DateString, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, newcustomer.custDOB)

            Do
                Console.WriteLine("Enter phone plan type: ")
                Dim temp As String = Console.ReadLine
                If temp.Length = 2 Then
                    If temp.Contains("3G") Or temp.Contains("4G") Then
                        newcustomer.TypeOfPlan = temp.ToString
                        Exit Do
                    Else
                        Console.WriteLine("Invalid plan type. Try again.")
                    End If
                End If
            Loop

            Console.WriteLine("Enter number of minutes used.")
            newcustomer.MinsUsed = Console.ReadLine
            Console.WriteLine("Enter number of texts used.")
            newcustomer.TextsUsed = Console.ReadLine
            Console.WriteLine("Enter number of data used.")
            newcustomer.DataUsed = Console.ReadLine

            Console.WriteLine("Customer added.")
            CustomerList.Add(newcustomer)
            isFinished = True
        Loop While Not isFinished

    End Sub

    Public Function GetUniqueID() As Integer
        Dim i As Integer = CInt(Math.Floor((999999 - 100000 + 1) * Rnd())) + 100000
        Return i

    End Function

    Public Sub PrintCustomers(ByVal listofcustomers As IEnumerable(Of Customer), Optional arg0 As Integer = 0, Optional arg1 As Integer = 0)

        For Each cust As Customer In listofcustomers

            Console.WriteLine("Customer Name: {0}", cust.custName)
            Console.WriteLine("Customer ID: {0} | Customer Mobile No.: {1}", cust.custID, cust.custMobileNo)
            Console.WriteLine("Customer Address: {0}", cust.custAddress)
            Console.WriteLine("Customer DOB: {0}", cust.custDOB)
            Console.WriteLine("Minutes used: {0} / {3} | Texts used: {1} / {3} | Data used: {2} / {3}", cust.MinsUsed, cust.MinsUsed, cust.DataUsed, cust.MAX_DATA)
            Console.WriteLine("=========================================")
        Next
    End Sub

    Public Sub ViewCustomers()
        If Not CustomerList.Any Then
            Console.WriteLine("There's no customers...")
            Exit Sub
        Else
            Dim ch As Integer
            DisplayMenu("vc")
            ch = GetChoice()
            Select Case ch

                Case 1
                    PrintCustomers(CustomerList)
                    Exit Sub
                Case 0
                    Exit Sub
                Case Else
                    Throw New FormatException
            End Select
        End If
    End Sub

    Function GetPhoneNumber() As String
        Console.WriteLine("Would you like to generate a new number? (Y/N)")
        Do

        Loop
    End Function

End Module
