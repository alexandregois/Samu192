Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class AtualizaMensagemV1
    Implements IOperacaoAPP192

    Private ReadOnly _dados As DCAtualizarMensagemV1

    Public Sub New(dados As DCAtualizarMensagemV1)

        _dados = dados

    End Sub

    ''' <summary>
    ''' Garantir que está atualizando a mensagem certa da convers certa
    ''' Não consegue sobrescrever os registros 
    ''' </summary>
    ''' <param name="params"></param>
    ''' <returns></returns>
    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a DCAtualizarMensagemV1")
        End If

        Dim appConversaCompletada As APP192.BOAPPConversaCompletada = APP192.BOAPPConversaCompletada.CarregaArray(New APP192.FiltroAppConversaCompletada With {.FcmRegistration = _dados.FCMRegistration})?.FirstOrDefault()

        If appConversaCompletada IsNot Nothing Then
            Dim conversaMensagem As New APP192.BOConversaMensagem
            conversaMensagem.Carrega(_dados.CodConversaMensagem.Value)

            If conversaMensagem IsNot Nothing Then
                If conversaMensagem.HorarioEnviada Is Nothing AndAlso _dados.HorarioRecebida IsNot Nothing AndAlso _dados.HorarioRecebida.HasValue Then
                    conversaMensagem.HorarioEnviada = _dados.HorarioRecebida.Value
                End If

                If conversaMensagem.HorarioRecebida Is Nothing AndAlso _dados.HorarioRecebida IsNot Nothing AndAlso _dados.HorarioRecebida.HasValue Then
                    conversaMensagem.HorarioRecebida = _dados.HorarioRecebida.Value
                End If

                If conversaMensagem.HorarioLida Is Nothing AndAlso _dados.HorarioLida IsNot Nothing AndAlso _dados.HorarioLida.HasValue Then
                    conversaMensagem.HorarioLida = _dados.HorarioLida.Value
                End If

                conversaMensagem.Salva()

                Return True
            End If
        End If

        Return False

    End Function
End Class
