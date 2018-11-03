Module Module1

    Const VALUEADDEDTAX As Single = 1.2
    Public listCustomers As New List(Of Customer)

    Sub Main()

        Dim intOption As Integer = 0

        Dim foo As New Customer
        listCustomers.Add(foo)

        Do
            Try
                MainMenu()

                intOption = GetChoice()

                Select Case intOption
                    Case 0
                        Exit Sub
                    Case 1
                        Console.WriteLine(foo.custDOB)
                    Case 2
                        Console.WriteLine("Works.")
                    Case 3
                        Console.WriteLine("Works.")
                    Case Else
                        Console.WriteLine("Invalid.")
                End Select

            Catch ex As System.FormatException
                Console.WriteLine("Invalid. Try again.")
                Exit Try
            End Try
        Loop
    End Sub

    Class Customer 'Class for customer objects

        Public Property custName As String = ""
        Public Property custIDNo As Integer = 0
        Public Property custAddress As String = ""
        Public Property custDOB As Date
        Public Property custMobNo As String = ""

    End Class

    Structure PhonePlan

    End Structure




    Sub MainMenu()
        Console.WriteLine("Main menu -- Enter corrosponding number: ")
        Console.WriteLine()
        Console.WriteLine("1. View Customer")
        Console.WriteLine("2. Create New Customer")
        Console.WriteLine("3. View Plan Details")
        Console.WriteLine("0. Exit program")
        Console.WriteLine()
    End Sub

    Function GetChoice() As Integer 'Get menu choice from key
        Dim cki As ConsoleKeyInfo = Console.ReadKey
        Dim s As Integer = Integer.Parse(cki.KeyChar)
        Return s
    End Function


End Module
