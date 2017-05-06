Option Explicit On
Option Infer On

Public Class CheckInternetConnection

    Public Declare Function InternetGetConnectedState Lib "wininet.dll" (ByRef lpdwFlags As Integer, ByVal dwReserved As Integer) As Integer

    Public Function IsConnected() As Boolean
        IsConnected = InternetGetConnectedState(0, 0)
    End Function

End Class
