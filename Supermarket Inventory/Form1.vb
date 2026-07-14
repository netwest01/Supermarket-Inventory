Imports MySql.Data.MySqlClient
Public Class Form1

    Const connectionString As String = "server=localhost;port=3306;userid=root;password=root;database='supermarket'"
    Dim mysqlConn As MySqlConnection

    Public Sub getDBdata()

        Dim query As String =
            "select mp.product_id, mp.product_name,
            coalesce((select sum(tpe.quantity) from t_product_entries tpe where tpe.product_id = mp.product_id),0) -
            coalesce((select sum(tps.quantity) from t_product_sales tps where tps.product_id = mp.product_id),0) as Inventory
            from m_products mp;"
        mysqlConn = New MySqlConnection(connectionString)
        Dim command As New MySqlCommand(query, mysqlConn)
        Try
            mysqlConn.Open()
            Dim reader As MySqlDataReader = command.ExecuteReader()
            Dim dt As New DataTable()
            dt.Load(reader)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message)
            Return
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getDBdata()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        entries.Show()
    End Sub
End Class