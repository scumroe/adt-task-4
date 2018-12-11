Imports AADT_Task4.Classes
Imports System
Imports System.Reflection
Imports System.Globalization
Imports System.IO
Imports System.IO.File

Module Procedures

    Sub CheckForFiles()

        If Not IO.File.Exists(pathSave) Then
            IO.File.Create(pathSave)
        End If
        If Not IO.File.Exists(pathBill) Then
            IO.File.Create(pathBill)
        End If
        FileClose()

    End Sub
    Sub DisplayMenu(ByVal str As String) 'displays various menus
        Select Case str
            Case "mm"
                Console.WriteLine("Main menu -- Enter corrosponding number: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. View Customers")
                Console.WriteLine("2. Create New Customer")
                Console.WriteLine("3. Save Customers")
                Console.WriteLine("4. Load Customers")
                Console.WriteLine("5. Search customers")
                Console.WriteLine("6. Print Bill ")
                Console.WriteLine("0. Exit program")
            Case "vc"
                Console.WriteLine("1. View customers -- enter a corrosponding number: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. Show all customers.")
                Console.WriteLine("0. Exit menu.")
            Case "pb"
                Console.WriteLine("6. Print Bill: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. Search by ID.")
                Console.WriteLine("2. Search by Name.")
                Console.WriteLine("0. Exit menu.")
        End Select

    End Sub

    Function GetChoice() As Integer 'Get menu choice from key
        Dim cki As ConsoleKeyInfo = Console.ReadKey
        Dim i As UInteger = UInteger.Parse(cki.KeyChar)
        Console.Clear()
        Return i
    End Function

    Public Sub CreateNewCustomer() 'create new customer subroutine with some validation
        Dim newcustomer As New Customer
        Dim isFinished As Boolean = False

        Do
            Console.WriteLine("2. Add New Customer: ")
            Console.WriteLine("=========================================")
            newcustomer.custID = GetUniqueID()
            Console.WriteLine("Customer ID is {0}. ", newcustomer.custID)
            newcustomer.custMobileNo = "07" + GetUniqueID.ToString
            Console.WriteLine("Customer mobile no. is {0}. ", newcustomer.custMobileNo)


            Do
                Console.WriteLine("Enter the customer name (minimum of five characters): ")
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

            Do
                Try
                    Console.WriteLine("Enter number of minutes used: ")
                    newcustomer.MinsUsed = Console.ReadLine
                    Console.WriteLine("Enter number of texts used.")
                    newcustomer.TextsUsed = Console.ReadLine
                    Console.WriteLine("Enter number of data used.")
                    newcustomer.DataUsed = Console.ReadLine
                    Exit Do
                Catch ex As InvalidCastException
                    Console.WriteLine("Invalid input. Enter a number.")
                End Try
            Loop


            PrintCustomers(newcustomer)
            Console.WriteLine("Would you like to add this customer to the list? (0 for Yes/1 for No)")
            Dim ch As Integer = GetChoice()
            If ch = 0 Then 'this currently loops back all the way; need to learn more (reflection?)
                CustomerList.Add(newcustomer)
                Console.WriteLine("Customer added.")
                isFinished = True
            End If

        Loop While Not isFinished

    End Sub

    Public Function GetUniqueID() As Integer 'generate random number
        Dim i As Integer = CInt(Math.Floor((999999 - 100000 + 1) * Rnd())) + 100000

        For Each cust In CustomerList
            If i = cust.custID Then
                Do
                    i = CInt(Math.Floor((999999 - 100000 + 1) * Rnd())) + 100000
                Loop While i = cust.custID
            End If
        Next
        Return i

    End Function

    Public Sub PrintCustomers(ByVal obj As Object) 'print customers based on parameter provided

        If obj.GetType() = GetType(List(Of Customer)) Or obj.GetType() = GetType(IEnumerable(Of Customer)) Then
            Dim temp As IEnumerable(Of Customer) = obj
            temp = temp.OrderBy(Function(x) x.custID)
            For Each cust As Customer In temp
                Console.WriteLine("Customer Name: {0}", cust.custName)
                Console.WriteLine("Customer ID: {0} | Customer Mobile No.: {1}", cust.custID, cust.custMobileNo)
                Console.WriteLine("Customer Address: {0}", cust.custAddress)
                Console.WriteLine("Customer DOB: {0}", cust.custDOB)
                Console.WriteLine("Minutes used: {0} / {3} | Texts used: {1} / {3} | Data used: {2} / {3}", cust.MinsUsed, cust.MinsUsed, cust.DataUsed, 500)
                Console.WriteLine("Total monthly bill: {0} (VAT: {1})", cust.GetBill, GetVAT(cust.GetBill))
                Console.WriteLine("=========================================")
            Next
        ElseIf obj.GetType() = GetType(Customer) Then
            Dim temp As New Customer
            temp = obj
            Console.WriteLine("Customer Name: {0}", temp.custName)
            Console.WriteLine("Customer ID: {0} | Customer Mobile No.: {1}", temp.custID, temp.custMobileNo)
            Console.WriteLine("Customer Address: {0}", temp.custAddress)
            Console.WriteLine("Customer DOB: {0}", temp.custDOB)
            Console.WriteLine("Plan type: " + temp.TypeOfPlan)
            Console.WriteLine("Minutes used: {0} / {3} | Texts used: {1} / {3} | Data used: {2} / {3}", temp.MinsUsed, temp.MinsUsed, temp.DataUsed, 500)
            Console.WriteLine("Total monthly bill: {0} (VAT: {1})", temp.GetBill, GetVAT(temp.GetBill))
            Console.WriteLine("=========================================")
        End If


    End Sub

    Public Sub ViewCustomers() 'routine to handle customer viewing
        If Not CustomerList.Any Then
            Console.WriteLine("There's no customers...")
            Exit Sub
        Else
            Do
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
            Loop
        End If
    End Sub

    Public Sub SaveCustomers()

        Dim count As Integer
        If CustomerList.Any() Then
            Using sw As System.IO.StreamWriter = New System.IO.StreamWriter(pathSave)
                Console.WriteLine("Overwriting file {0}" + pathSave)
                For Each cust As Customer In CustomerList
                    For Each propinfo As PropertyInfo In cust.GetType.GetProperties 'figured out reflections but it's too late to go back and change :pensive:, will change post deadline
                        If Not propinfo.Name = "TotalBill" Or propinfo.Name = "GetBill" Then
                            sw.Write(propinfo.GetValue(cust).ToString + ",")
                        End If
                    Next
                    sw.WriteLine()
                    count += 1
                Next
                Console.WriteLine("{0} customer data added. Saved to {1}", count, path)
            End Using
        Else
            Console.WriteLine("There are no customers in the list.")
        End If
    End Sub

    Function LoadCustomers() As List(Of Customer)
        Dim lines() As String
        Dim propdata() As String
        Dim count As Integer = 0
        Dim templist As New List(Of Customer)
        Dim tempcust As New Customer
        Dim LoadSuccess As Boolean = False
        Console.WriteLine("Adding data from {0}.", pathSave)
        Using sr As System.IO.StreamReader = New System.IO.StreamReader(pathSave)
            lines = sr.ReadToEnd.Split("" & vbLf & "")
            For Each line In lines
                If Not line = "" Then
                    propdata = line.Split(",")
                    If Not propdata.Contains("" & vbLf & "") Then
                        templist.Add(New Customer With {.custName = propdata(0), .custID = propdata(1),
                                                                    .custAddress = propdata(2),
                                                                    .custDOB = propdata(3),
                                                                    .custMobileNo = propdata(4),
                                                                    .TypeOfPlan = propdata(5),
                                                                    .MinsUsed = propdata(6),
                                                                    .TextsUsed = propdata(7),
                                                                    .DataUsed = propdata(8)})
                        count += 1
                    Else
                        Console.WriteLine("File is empty.")

                    End If
                End If
            Next

        End Using


        If Not templist.Any.Equals(CustomerList.Any) Then 'Don't load if customer in templist is already in CustomerList
            LoadSuccess = True
        Else
            LoadSuccess = False
        End If

        If LoadSuccess = True Then
            Console.WriteLine("Successfully added {0} customer(s) from {1}.", count, pathSave)
            Return templist
        Else
            Console.WriteLine("Customer is already in list.")
            Return CustomerList
        End If

    End Function
    Public Sub ComplexSearch()

        If Not CustomerList.Any Then
            Console.WriteLine("There's no customers...")
            Exit Sub
        Else
            Do
                Dim ch As Integer
                Dim searchCList As IEnumerable(Of Customer)
                Dim FoundCustomer As Boolean = False
                Console.WriteLine("2. Search Customers -- enter a corrosoponding number: ")
                Console.WriteLine("=========================================")
                Console.WriteLine("1. Search by ID range")
                Console.WriteLine("2. Exit menu")
                ch = GetChoice()
                Select Case ch
                    Case 1
                        Do
                            Console.WriteLine("Enter first ID number: ")
                            Dim query1 As UInteger = Integer.Parse(Console.ReadLine)
                            Console.WriteLine("Enter second ID number: ")
                            Dim query2 As UInteger = Integer.Parse(Console.ReadLine)

                            If query1 > query2 Then
                                Dim temp As UInteger = query2
                                query2 = query1
                                query1 = temp
                            End If

                            Console.WriteLine("Searching for IDs between {0} and {1}", query1, query2)
                            For Each cust In CustomerList
                                If cust.custID >= query1 And cust.custID < query2 Then
                                    searchCList = CustomerList.Where(Function(x) cust.custID <= query1 Or cust.custID <= query2)
                                    FoundCustomer = True
                                Else
                                    Console.WriteLine("No customers found.")
                                    Exit Do
                                End If
                            Next
                        Loop While FoundCustomer = False

                        If FoundCustomer = True Then
                            Console.WriteLine("{0} customers found: ", searchCList.LongCount)
                            For Each cust In searchCList
                                Console.WriteLine("{0} (ID: {1})", cust.custName, cust.custID.ToString)
                            Next
                        End If
                    Case 0
                        Exit Sub
                End Select
            Loop
        End If
    End Sub
    Public Sub PrintBill()

        If Not CustomerList.Any Then
            Console.WriteLine("There's no customers...")
            Exit Sub
        Else
            Do
                DisplayMenu("pb")
                Dim ch As Integer
                Dim foundCust As New Customer
                Dim FoundCustomer As Boolean = False
                Dim query As String = ""
                ch = GetChoice()
                Do
                    Select Case ch
                        Case 1
                            Console.Write("Enter customer ID number: ")
                            query = Console.ReadLine
                            For Each cust As Customer In CustomerList
                                If Int32.Parse(query) = cust.custID Then
                                    foundCust = CustomerList.Find(Function(x) query = cust.custID)
                                    PrintCustomers(foundCust)
                                    FoundCustomer = True
                                End If
                            Next
                        Case 2
                            Console.Write("Enter customer name: ")
                            query = Console.ReadLine
                            For Each cust As Customer In CustomerList
                                If query = cust.custName Then
                                    foundCust = CustomerList.Find(Function(x) cust.custName = query)
                                    FoundCustomer = True
                                Else
                                    Console.WriteLine("No customers found.")
                                End If
                            Next
                        Case 0
                            Exit Sub
                        Case Else
                            Throw New FormatException
                    End Select
                Loop While FoundCustomer = False

                If FoundCustomer = True Then
                    PrintCustomers(foundCust)
                    Console.WriteLine("Would you like to print this customer's bill? (0 for Yes/1 for No)")
                    ch = GetChoice()
                    If ch = 0 Then
                        SaveBill(foundCust)
                        Exit Sub
                    ElseIf ch = 1 Then
                        Exit Sub
                        Throw New FormatException
                    End If
                Else
                End If

            Loop
        End If

    End Sub
    Public Sub SaveBill(ByVal customer As Customer)
        Using sw As IO.StreamWriter = New IO.StreamWriter(pathBill)
            sw.WriteLine("Bill for: {0}", customer.custName)
            sw.WriteLine("=========================================")
            sw.WriteLine("Minutes used: {0} | Texts used: {1} | Data used: {2}", customer.MinsUsed, customer.MinsUsed, customer.DataUsed)
            sw.WriteLine("Total monthly bill: " + customer.TotalBill)
            sw.WriteLine("=========================================")
        End Using
        Console.WriteLine("Bill saved to {0}", pathBill)
    End Sub

End Module
