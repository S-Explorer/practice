'用来描述积木
Public Class Block
    Public p(0 To 3) As Point '4个矩形的位置
    Public c As Color '定义颜色
    Public IsRect As Boolean '判断是否是正方形的积木

    Public Sub New(x() As Integer, y() As Integer, c As Color, IsRect As Boolean)
        For i = 0 To 3
            p(i) = New Point(x(i), y(i))
        Next
        Me.c = c
        Me.IsRect = IsRect
    End Sub

    '让积木旋转
    Public Sub turn()
        For i = 0 To 3
            Dim t_x = p(i).X
            p(i).X = -p(i).Y
            p(i).Y = t_x
        Next
    End Sub

    '防止积木旋转超出边缘或者与别的积木卡在一起
    Public Sub turn_C()
        For i = 0 To 3
            Dim t_x = p(i).X
            p(i).X = p(i).Y
            p(i).Y = -t_x
        Next
    End Sub

End Class
