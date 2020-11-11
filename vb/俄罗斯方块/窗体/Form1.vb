Class Form1

    Private Sub Form1_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        GamePanel1.Focus()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'GamePanel1.Press()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Text = "重新开始"
        GamePanel1.start = True
        Button2.Enabled = True

        '初始化

        GamePanel1.落下的矩形集合.Clear()
        GamePanel1.grade = 0
        GamePanel1.level = 1
        GamePanel1.createBlock()


        GamePanel1.Focus()
    End Sub

    Private Sub Button1_GotFocus(sender As Object, e As EventArgs) Handles Button1.GotFocus
        GamePanel1.Focus()
    End Sub

    Private Sub Button2_GotFocus(sender As Object, e As EventArgs) Handles Button2.GotFocus
        GamePanel1.Focus()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Label2.Text = GamePanel1.grade
        Me.Label3.Text = GamePanel1.level
    End Sub

    Private Sub PanelShow_Paint(sender As Object, e As PaintEventArgs) Handles PanelShow.Paint
        '绘制积木
        For i = 0 To 3
            e.Graphics.FillRectangle(New SolidBrush(GamePanel1.nextBlock.c), New Rectangle(GamePanel1.nextBlock.p(i).X * GamePanel1.RECT_SIZE + 40, GamePanel1.nextBlock.p(i).Y * GamePanel1.RECT_SIZE + 30, GamePanel1.RECT_SIZE, GamePanel1.RECT_SIZE))
        Next
        '绘制积木边框
        For i = 0 To 3
            e.Graphics.DrawRectangle(New Pen(Color.White), New Rectangle(GamePanel1.nextBlock.p(i).X * GamePanel1.RECT_SIZE + 40, GamePanel1.nextBlock.p(i).Y * GamePanel1.RECT_SIZE + 30, GamePanel1.RECT_SIZE, GamePanel1.RECT_SIZE))
        Next
    End Sub

    Private Sub Label2_TextChanged(sender As Object, e As EventArgs) Handles Label2.TextChanged
        PanelShow.Refresh()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GamePanel1.start = Not GamePanel1.start   '按钮2的暂停功能的实现
        Button2.Text = IIf(GamePanel1.start, "暂停"， "继续")
    End Sub
End Class