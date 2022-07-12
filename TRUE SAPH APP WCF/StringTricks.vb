Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function GetValueOrDefault(valor As String, Optional padrao As String = "") As String

        If valor Is Nothing Then
            Return padrao
        Else
            Return valor
        End If

    End Function



End Module
