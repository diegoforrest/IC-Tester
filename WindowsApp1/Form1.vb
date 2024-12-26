Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.IO.Ports
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Public Class Form1
    Public connection As MySqlConnection
    Dim feedback1 As String
    Dim vout1 As String
    Dim vout2 As String
    Dim vout3 As String
    Dim vout4 As String
    Dim state1 As String
    Dim state2 As String
    Dim state3 As String
    Dim state4 As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GroupBox2.Enabled = False
        Try
            Timer1.Enabled = True
            Timer1.Start()
            GroupBox2.Enabled = False
            For Each port As String In SerialPort.GetPortNames()
                ComboBox1.Items.Add(port)
            Next
            ComboBox1.SelectedIndex = 0
            ComboBox2.SelectedItem = "9600"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Enabled = False
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        If Button1.Text = "CONNECT" Then
            Panel1.Enabled = False
            Panel2.Enabled = False
            Panel3.Enabled = False
            Panel4.Enabled = False
            GroupBox2.Enabled = False
            SerialPort1.BaudRate = CInt(ComboBox2.SelectedItem)
            SerialPort1.PortName = ComboBox1.SelectedItem.ToString()
            SerialPort1.Open()
            Button1.Text = "Disconnect"
            GroupBox2.Enabled = True
        Else

            SerialPort1.Close()
            GroupBox1.Enabled = True
            GroupBox2.Enabled = False
            Button1.Text = "CONNECT"


        End If
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer1.Enabled = False
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        If Button2.Text = "TEST" Then
            Timer1.Enabled = True
            Button2.Text = "Untest"
            Panel1.Enabled = True
            Panel2.Enabled = True
            Panel3.Enabled = True
            Panel4.Enabled = True
        Else
            Button2.Text = "TEST"
            Panel1.Enabled = False
            Panel2.Enabled = False
            Panel3.Enabled = False
            Panel4.Enabled = False

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "CONNECT" Then
            Try
                connection = New MySqlConnection("server=localhost;userid=root;password=;database=ictester")
                connection.Open()
                Button3.Text = "Disconnect"
                MsgBox("Connected to the database successfully!")
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            End Try
        Else
            If connection IsNot Nothing AndAlso connection.State = ConnectionState.Open Then
                connection.Close()
                Button3.Text = "CONNECT"
                MsgBox("Disconnected from the database.")
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Timer1.Enabled = True
            connection = New MySqlConnection("server=localhost;userid=root;password=;database=ictester")
            connection.Open()
            Dim command As New MySqlCommand("INSERT INTO ictester (OPAMP, VOUT, STATE) VALUES (@OPAMP, @VOUT, @STATE)", connection)
            Dim opamps As String() = {Label9.Text, Label11.Text, Label13.Text, Label15.Text}
            Dim vouts As String() = {vout1, vout2, vout3, vout4}
            Dim states As String() = {state1, state2, state3, state4}
            For i As Integer = 0 To 3
                command.Parameters.Clear()
                command.Parameters.AddWithValue("@OPAMP", opamps(i))
                command.Parameters.AddWithValue("@VOUT", vouts(i))
                command.Parameters.AddWithValue("@STATE", states(i))
                command.ExecuteNonQuery()
            Next
            MsgBox("Data saved successfully!")
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub
    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Try
            If SerialPort1.IsOpen Then
                feedback1 = SerialPort1.ReadLine()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not String.IsNullOrEmpty(feedback1) Then
            Dim str2() As String = feedback1.Split(","c)
            vout1 = str2(0)
            vout2 = str2(1)
            vout3 = str2(2)
            vout4 = str2(3)
            state1 = TextBox1.Text
            state2 = TextBox3.Text
            state3 = TextBox5.Text
            state4 = TextBox7.Text
            If str2.Length >= 4 Then
                UpdateTextBoxValues(str2(0), TextBox1, TextBox2)
                UpdateTextBoxValues(str2(1), TextBox3, TextBox4)
                UpdateTextBoxValues(str2(2), TextBox5, TextBox6)
                UpdateTextBoxValues(str2(3), TextBox7, TextBox8)
            End If
        End If
    End Sub
    Private Sub UpdateTextBoxValues(value As String, statusTextBox As TextBox,
voltageTextBox As TextBox)
        Dim voltage As Decimal
        If Decimal.TryParse(value, voltage) Then
            If voltage >= 1 Then
                statusTextBox.Text = "Bad"
            Else
                statusTextBox.Text = "Good"
            End If
            voltageTextBox.Text = $"{voltage} V"
        End If
    End Sub


End Class