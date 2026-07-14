Imports MySql.Data.MySqlClient

Public Class sales
    'データベース接続文字列
    Const connectionString As String = "server=localhost;port=3306;userid=root;password=root;database=supermarket"
    Dim mysqlConn As MySqlConnection


    'データベースからデータを取得してDataGridViewに表示するメソッド
    Public Sub getDBdata()

        Dim query As String =
            "select tps.sale_id, mp.product_id, mp.product_name, tps.quantity, tps.sale_datetime from m_products mp
            left join t_product_sales tps
            on mp.product_id = tps.product_id"
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
        Finally
            mysqlConn.Close()
        End Try
    End Sub

    Private Sub sales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getDBdata()
    End Sub

    'DataGridViewのセルがクリックされたときの処理
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Dim selectedRow = DataGridView1.CurrentRow
        Dim saleId As Integer
        If selectedRow.Cells(0).Value Is DBNull.Value Then
            MessageBox.Show("選択された行のデータがありません。")
            Return
        Else
            saleId = Integer.Parse(selectedRow.Cells(0).Value.ToString())
        End If

        Dim productId As String = selectedRow.Cells(1).Value.ToString()
        Dim quantity As String = selectedRow.Cells(3).Value.ToString()
        Dim dateTime As String = selectedRow.Cells(4).Value.ToString()

        TextBox1.Text = productId
        TextBox2.Text = quantity
        DateTimePicker1.Value = dateTime
        TextBox4.Text = saleId.ToString()

    End Sub

    'データベースにデータを追加するメソッド
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles buttonAdd.Click
        Dim productId As Integer
        Dim quantity As Integer

        Try
            productId = Integer.Parse(TextBox1.Text)
            quantity = Integer.Parse(TextBox2.Text)
        Catch ex As FormatException
            MessageBox.Show("商品IDと数量を正しく入力してください。")
            Return
        End Try
        Dim dateTime As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")

        Dim query As String = "insert into t_product_sales (product_id, quantity, sale_datetime) values ('" & productId & "', '" & quantity & "', '" & dateTime & "')"

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
            getDBdata()
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

    Private Sub buttonEdit_Click(sender As Object, e As EventArgs) Handles buttonEdit.Click
        Dim productId As Integer = Integer.Parse(TextBox1.Text)
        Dim quantity As Integer = Integer.Parse(TextBox2.Text)
        Dim dateTime As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Dim saleId As Integer = Integer.Parse(TextBox4.Text)

        Dim query As String = "UPDATE t_product_sales SET product_id = '" & productId & "', quantity = '" & quantity & "', sale_datetime = '" & dateTime & "' WHERE sale_id = '" & saleId & "'"

        mysqlConn = New MySqlConnection(connectionString)
        Dim command As New MySqlCommand(query, mysqlConn)
        Try
            mysqlConn.Open()
            command.ExecuteNonQuery()
            MessageBox.Show("データが更新されました。")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            mysqlConn.Close()
            getDBdata()
        End Try
    End Sub

    '削除処理
    Private Sub buttonDel_Click(sender As Object, e As EventArgs) Handles buttonDel.Click
        Dim saleId As Integer = Integer.Parse(TextBox4.Text)
        Dim query As String = "DELETE FROM t_product_sales WHERE sale_id = '" & saleId & "'"

        mysqlConn = New MySqlConnection(connectionString)
        Dim command As New MySqlCommand(query, mysqlConn)
        Try
            mysqlConn.Open()
            command.ExecuteNonQuery()
            MessageBox.Show("データが削除されました。")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            mysqlConn.Close()
            getDBdata()
        End Try
    End Sub
End Class