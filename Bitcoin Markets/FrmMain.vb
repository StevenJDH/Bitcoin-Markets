Option Explicit On
Option Infer On

Public Class FrmMain

    Private CheckConn As New CheckInternetConnection()
    Dim rememberedWindowState As Windows.Forms.FormBorderStyle 'Remembers the window state the user had before going to full screen.

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        If (CStr(CheckConn.IsConnected())) = False Then
            lblLoading.ContextMenuStrip = cmsLoading
            lblLoading.Text = "No internet connection detected." & vbNewLine & "Check your connection and try again." & vbNewLine & "(right-click to reconnect)"
            Exit Sub
        End If
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        WebBrowser1.Navigate("http://bitcoinity.org/markets?currency=EUR&exchange=kraken")
    End Sub

    Private Sub TimerLoad_Tick(sender As Object, e As EventArgs) Handles TimerLoad.Tick
        lblLoading.Visible = False
        TimerLoad.Enabled = False
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        TimerLoad.Enabled = True
    End Sub

    Private Sub WebBrowser1_StatusTextChanged(sender As Object, e As EventArgs) Handles WebBrowser1.StatusTextChanged
        'This changes these DIVs when ever the page changes.
        On Error Resume Next

        'TODO: Better write the code below in green to handle page cannot display "This program cannot display the webpage" when clicking on something in the chart without internet to better hide the white page. auto refreshes don't seem to bring it up, it just doesn't refresh.

        'If (CStr(CheckConn.IsConnected())) = False Then
        'If WebBrowser1.DocumentTitle = "" Then
        'lblLoading.ContextMenuStrip = cmsLoading
        'lblLoading.Text = "No internet connection detected." & vbNewLine & "Check your connection and try again." & vbNewLine & "(Right-Click To Reconnect)"
        ' Exit Sub
        ' End If

        'This will error if not on correct page, but the error handler above surpresses this. Best way to do it.
        WebBrowser1.Document.GetElementById("title").InnerHtml = "Bitcoin Markets v1.0 - Steven Jenkins De Haro"
        WebBrowser1.Document.GetElementById("footer").InnerHtml = ""
    End Sub

    Private Sub ForwardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForwardToolStripMenuItem.Click
        WebBrowser1.GoForward()
    End Sub

    Private Sub BackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackToolStripMenuItem.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub FullScreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FullScreenToolStripMenuItem.Click
        rememberedWindowState = Me.WindowState
        FullScreenToolStripMenuItem.Visible = False
        ExitFullScreenToolStripMenuItem.Visible = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.WindowState = FormWindowState.Normal 'This is a workaround because if already maximized then when in fullscreen the taskbark will show.
        Me.WindowState = FormWindowState.Maximized
        Me.TopMost = True
    End Sub

    Private Sub ExitFullScreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitFullScreenToolStripMenuItem.Click
        ExitFullScreenToolStripMenuItem.Visible = False
        FullScreenToolStripMenuItem.Visible = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
        Me.WindowState = rememberedWindowState
        Me.TopMost = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub ExitToolStripMenuCMSLoading_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuCMSLoading.Click
        Close()
    End Sub

    Private Sub ReconnectToolStripMenuCMSLoading_Click(sender As Object, e As EventArgs) Handles ReconnectToolStripMenuCMSLoading.Click
        lblLoading.ContextMenuStrip = Nothing
        lblLoading.Text = "Loading Chart Data...."
        Application.DoEvents()
        If (CStr(CheckConn.IsConnected())) = False Then
            lblLoading.ContextMenuStrip = cmsLoading
            MsgBox("Still no internet connection detected.", MsgBoxStyle.Exclamation, "Bitcoin Markets")
            lblLoading.Text = "No internet connection detected." & vbNewLine & "Check your connection and try again." & vbNewLine & "(right-click to reconnect)"
            Exit Sub
        End If
        BackgroundWorker1.RunWorkerAsync()
    End Sub

End Class
