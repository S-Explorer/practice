Public Class GamePanel : Inherits Panel
    Public Const RECT_SIZE As Integer = 20 '矩形的尺寸

    '7种积木,数组形式存储积木的类型
    Private blockTypes() As Block = {
        New Block(New Integer() {0, -1, 1, 1}, New Integer() {0, 0, 0, 1}, Color.Red, False),
        New Block(New Integer() {0, -1, 1, 2}, New Integer() {0, 0, 0, 0}, Color.LightBlue, False),
        New Block(New Integer() {0, -1, 1, -1}, New Integer() {0, 0, 0, 1}, Color.Green, False),
        New Block(New Integer() {0, -1, 0, 1}, New Integer() {0, 0, 1, 1}, Color.YellowGreen, False),
        New Block(New Integer() {0, 1, 0, 1}, New Integer() {0, 0, 1, 1}, Color.Orange, True),
        New Block(New Integer() {0, 1, -1, 0}, New Integer() {0, 0, 1, 1}, Color.Gray, False),
        New Block(New Integer() {0, -1, 1, 0}, New Integer() {0, 0, 0, 1}, Color.Tomato, False)
    }

    Public nowBlock As Block    '正在下落的积木
    Public nextBlock As Block   '下一个将要落下来的积木
    Public x, y As Integer      '矫正积木的位置
    Public 落下的矩形集合 As List(Of 落下的矩形） = New List(Of 落下的矩形)
    Public level As Integer = 1 '游戏等级

    Public start As Boolean = False '游戏是否开始
    Public grade As Integer = 0 '游戏的得分

    Private 快速下落 As Boolean = False
    Private 下落时间 As Integer = 30
    Private t As Integer = 0

    Public Sub New()
        Me.BackColor = Color.FromArgb(255 * 0.3F, 0, 0, 0)
        Randomize(Now.Second) '初始化随机数生成器，使得每次产生的随机数都不相同
        nextBlock = blockTypes(Int(Rnd() * 7))  '随机数生成器产生随机数，使得积木的类型不同
        createBlock()

        timer1.Enabled = True
        timer1.Interval = 16  '每次执行的时间间隔 (ms)

        '消除闪烁
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        Me.Update()
    End Sub

    Public Sub createBlock()
        x = 8 : y = 0

        nowBlock = nextBlock
        nextBlock = blockTypes(Int(Rnd() * 7))
    End Sub

    Private Sub GamePanel_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        '绘制积木
        For i = 0 To 3
            e.Graphics.FillRectangle(New SolidBrush(nowBlock.c), New Rectangle((nowBlock.p(i).X + x) * RECT_SIZE, (nowBlock.p(i).Y + y) * RECT_SIZE, RECT_SIZE, RECT_SIZE))
        Next
        '绘制积木的边框
        For i = 0 To 3
            e.Graphics.DrawRectangle(New Pen(Color.White), New Rectangle((nowBlock.p(i).X + x) * RECT_SIZE, (nowBlock.p(i).Y + y) * RECT_SIZE, RECT_SIZE, RECT_SIZE))
        Next

        '绘制已经落下的矩形
        For Each a In 落下的矩形集合
            e.Graphics.FillRectangle(New SolidBrush(a.c), New Rectangle(a.p.X * RECT_SIZE, a.p.Y * RECT_SIZE, RECT_SIZE, RECT_SIZE))
        Next
        '绘制已经落下的矩形边框
        For Each a In 落下的矩形集合
            e.Graphics.DrawRectangle(New Pen(Color.White), New Rectangle(a.p.X * RECT_SIZE, a.p.Y * RECT_SIZE, RECT_SIZE, RECT_SIZE))
        Next

    End Sub

    Private WithEvents timer1 As New Timer

    Private Sub timer1_Tick(sender As Object, e As EventArgs) Handles timer1.Tick
        If start Then '当游戏处于运行状态
            t += 1
            If t > 下落时间 Or 快速下落 Then
                t = 0
                y += 1 '让积木下落

                If 重合底边和积木() Then
                    grade += 1
                    y -= 1 '撤销下落
                    快速下落 = False
                    For i = 0 To 3
                        落下的矩形集合.Add(New 落下的矩形(nowBlock.p(i).X + x, nowBlock.p(i).Y + y, nowBlock.c))
                    Next

                    '判断已经落下的矩形是否占满整行
                    For i = 0 To 27
                        Dim t(0 To 17) As 落下的矩形
                        Dim num As Integer = 0 '记录每行矩形的数量
                        For Each a In 落下的矩形集合
                            If a.p.Y = i Then
                                t(num) = a
                                num += 1
                            End If
                        Next
                        If num = 18 Then
                            grade += 9
                            For Each a In 落下的矩形集合
                                If a.p.Y < t(0).p.Y Then        '如果满足一行则消失一行，且使之下落
                                    a.p.Y += 1
                                End If
                            Next
                            For j = 0 To t.Length - 1 '消除整行矩形
                                落下的矩形集合.Remove(t(j))
                            Next
                        End If
                    Next

                    '判断玩家失败
                    For Each a In 落下的矩形集合
                        If a.p.Y < 0 Then
                            start = False
                            MsgBox("你失败了"，， "失败")
                        End If
                    Next

                    createBlock() '生成新的积木
                End If
            End If

            Me.Refresh() '重绘
        End If
    End Sub

    Public Function 重合底边和积木() As Boolean
        '判断是否与底边重合
        '查找积木中的矩形的最大y坐标
        Dim maxY As Integer = 0
        For i = 0 To 3
            If maxY < nowBlock.p(i).Y + y Then
                maxY = nowBlock.p(i).Y + y
            End If
        Next

        If maxY >= 28 Then
            Return True
        End If

        '判断是否与积木重合
        For i = 0 To 3
            For Each a In 落下的矩形集合
                If nowBlock.p(i).X + x = a.p.X And nowBlock.p(i).Y + y = a.p.Y Then
                    Return True
                End If
            Next
        Next

        Return False
    End Function

    '积木超过左侧的边缘
    Private Function isbored1() As Boolean
        '查找矩形的最小的X坐标
        Dim minx As Integer = 1000
        For i = 0 To 3
            If minx > nowBlock.p(i).X + x Then
                minx = nowBlock.p(i).X + x
            End If

            If minx < 0 Then
                Return True
            End If

        Next

        Return False
    End Function

    '积木超过右侧的边缘
    Private Function isbored2() As Boolean
        '查找矩形的最大的X坐标
        Dim maxx As Integer = 0
        For i = 0 To 3
            If maxx < nowBlock.p(i).X + x Then
                maxx = nowBlock.p(i).X + x
            End If

            If maxx >= 18 Then
                Return True
            End If

        Next

        Return False
    End Function

    Private Sub GamePanel1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If start Then
            Select Case e.KeyCode.ToString
                Case "Left" '左箭头
                    x -= 1
                    If isbored1() Or 重合底边和积木() Then
                        x += 1
                    End If
                Case "Right" '右箭头
                    x += 1
                    If isbored2() Or 重合底边和积木() Then
                        x -= 1
                    End If
                Case "Up" '上箭头
                    If Not nowBlock.IsRect Then
                        nowBlock.turn()
                        If 重合底边和积木() Or isbored1() Or isbored2() Then

                        End If
                    End If
                        Case "Down" '下箭头
                    快速下落 = True
            End Select
        End If
    End Sub



End Class
