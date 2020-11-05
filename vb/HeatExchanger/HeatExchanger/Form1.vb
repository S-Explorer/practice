Imports Microsoft.Office.Interop
Imports System.Collections

'Imports System.IO

Public Class Exchanegr

    Public maxtemp, mintemp As Double  '用以储存温差值
    Public NUM_LENGTH As Double        '储存算出来的管长
    'Public SAVE_FIEL As New SaveFileDialog 'TXT的文档的保存
    'Public SAVE_WORD As New SaveFileDialog 'doc的文档保存
    Dim WORD_APP As Word.Application        '初始化word程序
    Dim WORD_DOC As Word.Document           '初始化word文档
    Dim xls_app As New Excel.Application    '初始化excel程序
    Dim xls_book As Excel.Workbook          '初始化excel工作簿
    Dim xls_sheet As Excel.Worksheet        '初始化excel工作表

    'Dim panel As New Panel
    'Dim panel_W As New Panel
    Dim control_list1 As New ArrayList      '定义一个数组列表存储label控件
    Dim control_list2 As New ArrayList      '定义一个数组列表存储txtbox控件
    'Dim group1 As New GroupBox
    'Dim group2 As New GroupBox

    Private Property File_Path As String = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)      '定义一个属性存储桌面路径

    Private Sub Exchanegr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MsgBox("用于测试故灰色部分为默认数值，不予修改！", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "注意")  '程序使用注意说明
        GIFpicture.SizeMode = PictureBoxSizeMode.StretchImage
        'GIFpicture.Image.RotateFlip(RotateFlipType.Rotate90FlipNone)
        control_list1.Add(Label1)                                          '这一点大块是将控件添加到一个数组中方便后续对空间的批量操作 
        control_list1.Add(Label2)
        control_list1.Add(Label3)
        control_list1.Add(Label4)
        control_list1.Add(Label5)
        control_list1.Add(Label6)
        control_list1.Add(Label7)
        control_list1.Add(Label8)
        control_list1.Add(Label9)
        control_list1.Add(Label10)
        control_list1.Add(Label11)
        control_list1.Add(Label12)
        control_list1.Add(Label13)
        control_list1.Add(Label14)
        control_list1.Add(Label15)
        control_list1.Add(Label16)
        control_list1.Add(Label17)
        control_list1.Add(Label18)
        control_list1.Add(Label19)
        control_list2.Add(t1_1)
        control_list2.Add(t11_1)
        control_list2.Add(t1_2)
        control_list2.Add(t11_2)
        control_list2.Add(p_1)
        control_list2.Add(p_2)
        control_list2.Add(m_1)
        control_list2.Add(cp_1)
        control_list2.Add(p_ro_1)
        control_list2.Add(u_miu_1)
        control_list2.Add(l_lam_1)
        control_list2.Add(pr_plm_1)
        control_list2.Add(tm_1)
        control_list2.Add(cp_2)
        control_list2.Add(p_ro_2)
        control_list2.Add(u_miu_2)
        control_list2.Add(l_lam_2)
        control_list2.Add(pr_plm_2)
        'panel_W.Controls.Add(tm_2)
        control_list2.Add(tm_2)
        panel.Enabled = True
        panel_W.Enabled = True
        'panel.Visible = True
        'panel.Show()
        'panel_W.Show()
        'panel_W.Visible = True
    End Sub

    Private Sub 计算ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 计算ToolStripMenuItem.Click
        If tube_list.SelectedItem = "无缝钢管25X2.5" And ComboBox1.SelectedItem = "等边三角形(必选)" Then
            wuxing()
            heat()
            Face()   
        Else
            MsgBox("请确认'管束的材料规格'和'管的排列方式'已经选择")
        End If
    End Sub


    '-----------------------------------------物性计算-------------------------------------------------------'
    Public Sub wuxing()  '调用计算物性参数部分
        Try             '判断数据填写的时候正确，不正确则弹窗告知检查数据正确与否！
            tm_1.Text = (Val(t1_1.Text) + Val(t11_1.Text)) / 2                  '计算煤油的定性温度
            tm_2.Text = (Val(t1_2.Text) + Val(t11_2.Text)) / 2                  '计算水的定性温度
            pr_plm_1.Text = u_miu_1.Text * cp_1.Text * 1000 / l_lam_1.Text      '计算煤油的普兰德数
            pr_plm_2.Text = u_miu_2.Text * cp_2.Text * 1000 / l_lam_2.Text      '计算水的普兰德数
        Catch ex As Exception
            MsgBox("物性计算相关数据填写不正确，请检查后再试！")
        End Try
    End Sub
    '-------------------------------------------------------------------------------------------------------'


    '------------------------------------------传热温差计算---------------------------------------------------'
    Public Sub max_min()       '判断最大逆流温差和最小逆流温差
        Dim max_tem, min_tem As Double
        max_tem = Math.Abs(Val(t1_1.Text) - Val(t11_1.Text))
        min_tem = Math.Abs(Val(t1_2.Text) - Val(t11_2.Text))
        If max_tem < min_tem Then
            maxtemp = min_tem
            mintemp = max_tem
        Else
            maxtemp = max_tem
            mintemp = min_tem
        End If
    End Sub

    Public Sub heat()  '调用计算传热量
        Try
            Q_heat.Text = m_1.Text * cp_1.Text * (Val(t1_1.Text) - Val(t11_1.Text)) * lostNUM.Text '计算传热量
            m_2.Text = Q_heat.Text / cp_2.Text * (Val(t11_2.Text) - Val(t1_2.Text)) / 100 '计算冷却水量
            max_min()            '调用逆流温差计算函数
            t1_mc.Text = (maxtemp - mintemp) / Math.Log(maxtemp / mintemp)          '计算逆流对数平均温差
            p_num.Text = (Val(t11_2.Text) - Val(t1_2.Text)) / (Val(t1_1.Text) - Val(t1_2.Text)) '计算参数P的值
            r_num.Text = (Val(t1_1.Text) - Val(t11_1.Text)) / (Val(t11_2.Text) - Val(t1_2.Text)) '计算参数R的值
            tm_avg.Text = Val(Repair_num.Text) * t1_mc.Text    '计算有效平均温差
        Catch ex As Exception
            MsgBox("传热量计算相关数据填写不正确，请检查后再试！")
        End Try
    End Sub
    '-------------------------------------------------------------------------------------------------------'


    '----------------------------------------传热面积和结构计算------------------------------------------------'
    Public Sub Face()
        Try
            F_S.Text = Q_heat.Text * 1000 / K_num.Text / tm_avg.Text   '估算传热面积
            At.Text = m_2.Text / p_ro_2.Text / w_2.Text             '计算管程所需的流通截面
            n_num.Text = Math.Ceiling(4 * At.Text / Math.PI / Math.Pow(0.02, 2)) '计算每程根数
            NUM_LENGTH = F_S.Text / Val(n_num.Text) / 4 / Math.PI / 0.025    '先计算管长的数值，再存储到NUM_LENGTH中
            'l_num.Text = NUM_LENGTH
            l_num_OK() '这里判断管长的数值应该取多少
            sp.Text = Val(s_cen.Text) * Math.Cos(30 * Math.PI / 180)    '计算平行于流向的管距
            sn.Text = Val(s_cen.Text) * Math.Sin(30 * Math.PI / 180)    '计算垂直于流向的管距
            single_S.Text = 136 * Math.PI * 0.025 * l_num.Text  '计算单台的传热面积
        Catch ex As Exception
            MsgBox("有些数值填写错误，请检查再试！")
        End Try
    End Sub

    Public Sub l_num_OK()        '管长的估计方案
        If NUM_LENGTH > 4.85 And NUM_LENGTH < 5.25 Then
            l_num.Text = "5"
        ElseIf NUM_LENGTH < 4.85 And NUM_LENGTH > 4.25 Then
            l_num.Text = "4.5"
        ElseIf NUM_LENGTH < 4.25 Then
            l_num.Text = "4"
        End If
    End Sub

    Private Sub tube_list_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tube_list.SelectedIndexChanged
        If tube_list.SelectedItem.ToString <> "无缝钢管25X2.5" Then
            MsgBox("只能选择 ：无缝钢管25X2.5", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "选择注意！")
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem <> "等边三角形(必选)" Then
            MsgBox("只能选择 等边三角形(必选)")
        End If
    End Sub
    '-------------------------------------------------------------------------------------------------------'

    '--------------------------------------保存文件----------------------------------------------------------'
    Private Sub 保存为txt文本ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存为txt文本ToolStripMenuItem.Click
        'SAVE_FIEL.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)        '对话框的起始位置
        'SAVE_FIEL.Filter = "txt文件(*.txt)|*.txt"                 '存储的文件格式
        'SAVE_FIEL.Title = "txt文件存储位置"                            '对话框的标题
        'SAVE_FIEL.RestoreDirectory = True                       '再次运行的打开路径重置
        'SAVE_FIEL.ShowDialog()
        Dim filename As String
        If MessageBox.Show("接下来保存为TXT文件，请确认？") = DialogResult.OK Then
            filename = File_Path & "\默认数据.txt"
            My.Computer.FileSystem.WriteAllText(filename, record(), False)      '调用record（）函数来提取信息
            MsgBox("保存完毕！")
        End If
    End Sub

    Private Function record() As String                                     '提取信息导出为TXT文件
        Dim TXT_CODE As String
        TXT_CODE = "煤油进口温度：" & t1_1.Text.ToString & "   煤油出口温度：" & t11_1.Text.ToString & Environment.NewLine
        TXT_CODE &= "冷却水进口温度：" & t1_2.Text.ToString & "    冷却水出口温度：" & t11_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油工作表压力：" & p_1.Text.ToString & "    冷却水工作表压力：" & p_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油定性温度：" & tm_1.Text.ToString & "    水的定性温度：" & tm_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油比热：" & cp_1.Text.ToString & "    水的比热：" & cp_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油密度：" & p_ro_1.Text.ToString & "    水的密度：" & p_ro_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油黏度：" & u_miu_1.Text.ToString & "    水的黏度：" & u_miu_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油导热系数：" & l_lam_1.Text.ToString & "    水的导热系数：" & l_lam_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油普兰德数(Pr)：" & pr_plm_1.Text.ToString & "    水的普兰德数(Pr)：" & pr_plm_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "煤油流量：" & m_1.Text.ToString & "    冷却水流量：" & m_2.Text.ToString & Environment.NewLine
        TXT_CODE &= "热损系数：" & lostNUM.Text.ToString & "    传热量：" & Q_heat.Text.ToString & Environment.NewLine
        TXT_CODE &= "逆流时的对数平均温差：" & t1_mc.Text.ToString & Environment.NewLine
        TXT_CODE &= "参数P：" & p_num.Text.ToString & "    参数R值：" & r_num.Text.ToString & Environment.NewLine
        Return TXT_CODE
    End Function
    '-------------------------------------------------------------------------------------------------------'


    '--------------------------------------退出程序----------------------------------------------------------'
    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        End
    End Sub
    '-------------------------------------------------------------------------------------------------------'

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        MsgBox("数据填写尽量不要更改，注意combox的选择是否正确！")
    End Sub


    '-------------------------------------WORD的保存---------------------------------------------------------'
    Public Sub New_Word()               '调用一个word程序
        WORD_APP = New Word.Application
        NEW_DOC()
        'WORD_APP.Visible = True
    End Sub

    Public Sub NEW_DOC()                '添加一个word文档
        WORD_DOC = WORD_APP.Documents.Add()
    End Sub

    Public Sub Add_Text(ByRef STR As String)    '为word文档中书写内容
        WORD_APP.Selection.TypeText(STR)
    End Sub

    Private Sub 保存为word文本ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存为word文本ToolStripMenuItem.Click
        'SAVE_WORD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)        'word保存对话框的路径
        'SAVE_WORD.Filter = "word文件(.docx)|*.docx"                               'word保存对话框的文件拓展名过滤器
        'SAVE_WORD.Title = "word文件保存 "                                         'word文件对话框的标题"
        'SAVE_WORD.RestoreDirectory = True
        Dim file_text As String = record()                                  '导入要保存的数据
        Dim file_name As String                                             '要保存的文件名和路径
        Try
            If MessageBox.Show("接下来保存为word文件") = Windows.Forms.DialogResult.OK Then    '开始保存
                file_name = File_Path & "\默认数据.docx"
                New_Word()
                Add_Text(file_text)                         '为word的内容添加文本
                WORD_DOC.SaveAs(file_name)                  '保存的文件名和位置
                WORD_DOC.Close()                            '关闭文件
                MsgBox("保存成功！")                         '提示保存成功
            End If
        Catch ex As Exception
            MsgBox("something erro !")
        End Try
    End Sub
    '-------------------------------------------------------------------------------------------------------'

    '-----------------------------------------Excel文件保存-------------------------------------------------------'
    Private Sub Excel_Creat()
    xls_book = xls_app.Workbooks.Add     '创建一个excel工作簿
        xls_sheet = xls_book.Sheets(1)      '工作簿中设置工作表
        xls_sheet.Name = "计算数据"         '为工作表命名
        Try
            For i = 1 To 19 Step 1
                xls_sheet.Cells(i, 1) = control_list1.Item(i - 1).Text              '将窗口文本栏导入到工作表中
                xls_sheet.Cells(i, 2) = control_list2.Item(i - 1).Text              '将窗口数据栏导入到工作表中
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub 保存为excel文本ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存为excel文本ToolStripMenuItem.Click
        Excel_Creat()
        xls_app.ActiveWorkbook.SaveAs(File_Path + "\test.xlsx")                     '保存的路径和文件名
        MessageBox.Show("保存成功", "保存Excel")                                      '提示保存成功
        xls_book.Close()                                                            '关闭excel
        xls_app = Nothing                                                          '清空缓存数据
    End Sub
    '-------------------------------------------------------------------------------------------------------'

    '---------------------------------------联系方式按钮------------------------------------------------------'
    Private Sub 联系我们ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 联系我们ToolStripMenuItem.Click
        MsgBox("电话：1234-45678910" & Environment.NewLine & "邮箱123456@gmail.com","联系")
    End Sub
    '-------------------------------------------------------------------------------------------------------'

    '--------------------------------------帮助文档说明------------------------------------------------------'
    Private Sub 帮助ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 帮助ToolStripMenuItem.Click
        MsgBox("暂时没有此功能，以后会开发！", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "帮助说明")
    End Sub
    '-------------------------------------------------------------------------------------------------------'

    '---------------------------------------数据清除-----------------------------------------------------'
    Private Sub 清除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清除ToolStripMenuItem.Click

    End Sub
    '-------------------------------------------------------------------------------------------------------'
End Class
