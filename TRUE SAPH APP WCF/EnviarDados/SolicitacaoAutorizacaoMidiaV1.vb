Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class SolicitacaoAutorizacaoMidiaV1
    Implements IOperacaoAPP192

    Private _dados As DCSolicitarAutorizacaoMidiaV1

    Public Sub New(dados As DCSolicitarAutorizacaoMidiaV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a SolicitacaoAutorizacaoMidiaV1")
        End If

        If String.IsNullOrWhiteSpace(_dados.Identificador) Then
            Return False
        End If

        Return APP192.BOAPP192.PodeReceberMidia(_dados.Identificador)

    End Function

End Class
