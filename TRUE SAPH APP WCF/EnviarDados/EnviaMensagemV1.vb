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

        'Recebimento mensagem só se o chamado estiver aberto
        Dim appChamado As APP192.BOAPPChamado = APP192.BOAPPChamado.CarregaArray(New APP192.FiltroAPPChamado With {.Identificador = _dados.Identificador, .ChamadoAberto = True})?.OrderByDescending(Function(f) f.CodAppChamado).FirstOrDefault()

        If appChamado IsNot Nothing Then
            Dim conversaMensagem As New APP192.BOConversaMensagem With {
                    .CodChamado = appChamado.CodChamado,
                    .Sentido = 1,
                    .HorarioRegistro = DateTime.Now(),
                    .HorarioEnviada = DateTime.Now(),
                    .HorarioRecebida = DateTime.Now(),
                    .Mensagem = _dados.Mensagem.GetValueOrDefault(),
                    .Acao = 0,
                    .Erros = 0
                }

            conversaMensagem.Salva()

            Return True
        End If

        Return False

    End Function
End Class