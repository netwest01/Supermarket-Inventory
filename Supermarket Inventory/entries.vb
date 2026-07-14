Imports MySql.Data.MySqlClient

Public Class entries
    'データベース接続文字列
    Const connectionString As String = "server=localhost;port=3306;userid=root;password=root;database='supermarket'"
    Dim mysqlConn As MySqlConnection

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged

    End Sub

    'データベースからデータを取得してDataGridViewに表示するメソッド
    Public Sub getDBdata()

        Dim query As String =
            "select mp.product_id, mp.product_name, tpe.quantity from m_products mp
            left join t_product_entries tpe
            on mp.product_id = tpe.product_id"
        mysqlConn = New MySqlConnection(connectionString)
        Dim command As New MySqlCommand(query, mysqlConn)
        Try
            mysqlConn.Open()
            Dim reader As MySqlDataReader = command.ExecuteReader()
            Dim dt As New DataTable()
            dt.Load(reader)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return
        End Try
    End Sub

    Private Sub entries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getDBdata()


    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Dim selectedRow = DataGridView1.CurrentRow
        Dim productId As String = selectedRow.Cells(0).Value.ToString()
        Dim quantity As String = selectedRow.Cells(2).Value.ToString()


        TextBox1.Text = productId
        TextBox2.Text = quantity


    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub    
    'データベースにデータを追加するメソッド
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles buttonAdd.Click
        Dim productId As Integer = Integer.Parse(TextBox1.Text)
        Dim quantity As Integer = Integer.Parse(TextBox2.Text)
        Dim dateTime As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")

        Dim query As String = "insert into t_product_entries (product_id, quantity, entry_datetime) values ('" & productId & "', '" & quantity & "', '" & dateTime & "')"

        mysqlConn = New MySqlConnection(connectionString)
        Dim command As New MySqlCommand(query, mysqlConn)
        Try
            mysqlConn.Open()
            command.ExecuteNonQuery()
            MessageBox.Show("データが追加されました。")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            mysqlConn.Close()
        End Try
    End Sub

    '商品IDから商品名を取得してTextBox3に表示するメソッド
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim productId As Integer = Integer.Parse(TextBox1.Text)
        Dim keyword As String = ""
        Dim query As String = "select mp.product_name from m_products mp where mp.product_id = '" & productId & "'"

        mysqlConn = New MySqlConnection(connectionString)
        Dim command As New MySqlCommand(query, mysqlConn)
        Try
            mysqlConn.Open()
            keyword = command.ExecuteScalar
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            mysqlConn.Close()
        End Try

        TextBox3.Text = keyword
        TextBox3.ReadOnly = True

    End Sub
End Class