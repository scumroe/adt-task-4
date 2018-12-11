Imports System.Globalization
Imports System.Reflection
Imports AADT_Task4.Classes
Imports AADT_Task4.Procedures
Module Main

#Region "Globals"
    Public CustomerList As New List(Of Customer) 'create list object
    Public path As String = My.Computer.FileSystem.CurrentDirectory
    Public pathSave As String = String.Concat(path, "/data.txt")
    Public pathBill As String = String.Concat(path, "/bill.txt")

    Const VALUEADDEDTAX As Single = 1.2 'VAT global (might move)
    Function GetVAT(amount As Integer)
        Return FormatCurrency((Double.Parse(amount, NumberStyles.Currency) * (VALUEADDEDTAX - 1)), 2)
    End Function
#End Region



    Public Sub Main()

        Randomize() 'intialize random function
        CheckForFiles() 'check for text files

        Dim opt As UInteger

        Do
            Try
                DisplayMenu("mm")

                opt = GetChoice()

                Select Case opt
                    Case 0
                        Exit Sub
                    Case 1
                        ViewCustomers()
                    Case 2
                        CreateNewCustomer()
                    Case 3
                        SaveCustomers()
                    Case 4
                        CustomerList = LoadCustomers()
                    Case 5
                        ComplexSearch()
                    Case 6
                        Printbill()
                    Case 0
                        Exit Sub
                    Case Else
                        Console.WriteLine("Invalid choice. Try again.")
                End Select
            Catch ex As FormatException
                Console.WriteLine("Invalid choice. Try again.", ConsoleColor.Yellow)
                Exit Try
            End Try
        Loop
    End Sub

End Module
