Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class EnviaMensagemV1
    Implements IOperacaoAPP192

    Private ReadOnly _dados As DCEnviarMensagemV1

    Public Sub New(dados As DCEnviarMensagemV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a DCEnviarMensagemV1")
        End If

        Dim appConversaCompletada As APP192.BOAPPConversaCompletada = APP192.BOAPPConversaCompletada.CarregaArray(New APP192.FiltroAppConversaCompletada With {.FcmRegistration = _dados.FCMRegistration})?.FirstOrDefault()

        If appConversaCompletada IsNot Nothing Then
            Dim conversaMensagem As New BOConversaMensagem With {
                .CodConversa = appConversaCompletada.CodConversa,
                .Sentido = 1,
                .HorarioRegistro = _dados.HorarioRegistro.GetValueOrDefault(),
                .HorarioEnviada = DateTime.Now(),
                .HorarioRecebida = DateTime.Now(),
                .Mensagem = _dados.Mensagem.GetValueOrDefault(),
                .Acao = 0,
                .Erros = 0
            }

            conversaMensagem.Salva()

            Dim conversa As New BOConversa
            conversa.Carrega(appConversaCompletada.CodConversa.Value)

            If conversa IsNot Nothing Then
                Dim andamento As New BOAndamento
                Dim reserva As BOChamadoTicket.Reserva = BOChamadoTicket.ExisteReservaChamado(conversa.CodChamado.Value)

                'Não deve estar reservado (aberto por um regulador)
                'Deve estar na lista da Andamentos
                'Não pode ter uma conversa atrelada no momento
                If reserva Is Nothing AndAlso andamento.CarregaAK1(conversa.CodChamado.Value) AndAlso andamento IsNot Nothing AndAlso andamento.CodConversa Is Nothing Then
                    andamento.CodConversa = conversa.CodConversa

                    andamento.Salva()
                End If

                andamento = Nothing
                reserva = Nothing
            End If

            Return True
        Else
            Return False
        End If

    End Function
End Class