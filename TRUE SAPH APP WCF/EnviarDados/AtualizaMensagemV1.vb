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

        Dim appChamado As APP192.BOAPPChamado = APP192.BOAPPChamado.CarregaArray(New APP192.FiltroAPPChamado With {.Identificador = _dados.Identificador, .ChamadoAberto = True})?.OrderByDescending(Function(f) f.CodAppChamado).FirstOrDefault()

        If appChamado IsNot Nothing Then
            Dim conversaMensagem As New APP192.BOConversaMensagem
            conversaMensagem.Carrega(_dados.CodConversaMensagem.Value)

            If conversaMensagem IsNot Nothing Then
                If conversaMensagem.HorarioEnviada Is Nothing AndAlso _dados.AtualizaHorarioRecebida IsNot Nothing AndAlso _dados.AtualizaHorarioRecebida.HasValue AndAlso _dados.AtualizaHorarioRecebida.Value Then
                    conversaMensagem.HorarioEnviada = DateTime.Now
                End If

                If conversaMensagem.HorarioRecebida Is Nothing AndAlso _dados.AtualizaHorarioRecebida IsNot Nothing AndAlso _dados.AtualizaHorarioRecebida.HasValue AndAlso _dados.AtualizaHorarioRecebida.Value Then
                    conversaMensagem.HorarioRecebida = DateTime.Now
                End If

                If conversaMensagem.HorarioLida Is Nothing AndAlso _dados.AtualizaHorarioLida IsNot Nothing AndAlso _dados.AtualizaHorarioLida.HasValue AndAlso _dados.AtualizaHorarioLida.Value Then
                    conversaMensagem.HorarioLida = DateTime.Now
                End If

                conversaMensagem.Salva()

                Return True
            End If
        End If

        Return False

    End Function
End Class
