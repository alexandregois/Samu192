Imports Newtonsoft.Json
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class BuscaMensagemV1
    Implements IOperacaoAPP192

    Private ReadOnly _dados As DCBuscarMensagemV1

    Public Sub New(dados As DCBuscarMensagemV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a DCBuscarMensagemV1")
        End If

        Dim retorno As String = String.Empty 'New List(Of BOConversaMensagem)
        Dim appConversaCompletada As APP192.BOAPPConversaCompletada = APP192.BOAPPConversaCompletada.CarregaArray(New APP192.FiltroAppConversaCompletada With {.FcmRegistration = _dados.FCMRegistration})?.FirstOrDefault()

        If appConversaCompletada IsNot Nothing AndAlso appConversaCompletada.CodConversa.HasValue Then
#Region "Filtro"
            Dim filtro As New FiltroConversaMensagem With {.CodConversa = appConversaCompletada.CodConversa.Value}

            If _dados.Sentido IsNot Nothing AndAlso _dados.Sentido.HasValue Then
                filtro.Sentido = _dados.Sentido.Value
            End If
            If _dados.HorarioRegistro IsNot Nothing AndAlso _dados.HorarioRegistro.HasValue Then
                filtro.HorarioRegistro = _dados.HorarioRegistro.Value
            End If
            If _dados.HorarioRegistroI IsNot Nothing AndAlso _dados.HorarioRegistroI.HasValue Then
                filtro.HorarioRegistroI = _dados.HorarioRegistroI.Value
            End If
            If _dados.HorarioRegistroF IsNot Nothing AndAlso _dados.HorarioRegistroF.HasValue Then
                filtro.HorarioRegistroF = _dados.HorarioRegistroF.Value
            End If
            If _dados.HorarioLidaNula IsNot Nothing AndAlso _dados.HorarioLidaNula.HasValue Then
                filtro.HorarioLidaNula = _dados.HorarioLidaNula.Value
            End If
            If _dados.HorarioLida IsNot Nothing AndAlso _dados.HorarioLida.HasValue Then
                filtro.HorarioLida = _dados.HorarioLida.Value
            End If
            If _dados.HorarioLidaI IsNot Nothing AndAlso _dados.HorarioLidaI.HasValue Then
                filtro.HorarioLidaI = _dados.HorarioLidaI.Value
            End If
            If _dados.HorarioLidaF IsNot Nothing AndAlso _dados.HorarioLidaF.HasValue Then
                filtro.HorarioLidaF = _dados.HorarioLidaF.Value
            End If
            If _dados.Acao IsNot Nothing AndAlso _dados.Acao.HasValue Then
                filtro.Acao = _dados.Acao.Value
            End If
            If _dados.AcaoI IsNot Nothing AndAlso _dados.AcaoI.HasValue Then
                filtro.AcaoI = _dados.AcaoI.Value
            End If
            If _dados.AcaoF IsNot Nothing AndAlso _dados.AcaoF.HasValue Then
                filtro.AcaoF = _dados.AcaoF.Value
            End If
            If _dados.Erros IsNot Nothing AndAlso _dados.Erros.HasValue Then
                filtro.Erros = _dados.Erros.Value
            End If
            If _dados.ErrosI IsNot Nothing AndAlso _dados.ErrosI.HasValue Then
                filtro.ErrosI = _dados.ErrosI.Value
            End If
            If _dados.ErrosF IsNot Nothing AndAlso _dados.ErrosF.HasValue Then
                filtro.ErrosF = _dados.ErrosF.Value
            End If

            'BOUtil.EventMsg(String.Concat("Filtro: ", JsonConvert.SerializeObject(filtro)), EventLogEntryType.Warning, BOUtil.eEventID.APPService_BuscarMensagens)
#End Region

            Dim conversaMensagemList As List(Of BOConversaMensagem) = BOConversaMensagem.CarregaArray(filtro, _dados.NMaxReg, _dados.OrderByAsc)

            If conversaMensagemList IsNot Nothing AndAlso conversaMensagemList.Any() Then
                Dim oConversaMensagemRetornoList As New List(Of Object)

                'BOUtil.EventMsg(String.Concat("conversaMensagemList 012 Count: ", JsonConvert.SerializeObject(conversaMensagemList.Count)), EventLogEntryType.Warning, BOUtil.eEventID.APPService_BuscarMensagens)

                For Each oConversaMensagemTemp As BOConversaMensagem In conversaMensagemList
                    Dim conversaMensagem As New BOConversaMensagem With {
                        .CodConversaMensagem = IIf(oConversaMensagemTemp.CodConversaMensagem IsNot Nothing AndAlso oConversaMensagemTemp.CodConversaMensagem.HasValue, oConversaMensagemTemp.CodConversaMensagem.Value, Nothing),
                        .CodConversa = IIf(oConversaMensagemTemp.CodConversa IsNot Nothing AndAlso oConversaMensagemTemp.CodConversa.HasValue, oConversaMensagemTemp.CodConversa.Value, Nothing),
                        .Sentido = IIf(oConversaMensagemTemp.Sentido IsNot Nothing AndAlso oConversaMensagemTemp.Sentido.HasValue, oConversaMensagemTemp.Sentido.Value, Nothing),
                        .HorarioRegistro = IIf(oConversaMensagemTemp.HorarioRegistro IsNot Nothing AndAlso oConversaMensagemTemp.HorarioRegistro.HasValue, oConversaMensagemTemp.HorarioRegistro.Value, Nothing),
                        .Mensagem = oConversaMensagemTemp.Mensagem
                    }

                    If oConversaMensagemTemp.HorarioEnviada IsNot Nothing AndAlso oConversaMensagemTemp.HorarioEnviada.HasValue Then
                        conversaMensagem.HorarioEnviada = oConversaMensagemTemp.HorarioEnviada.Value
                    End If

                    If oConversaMensagemTemp.HorarioRecebida IsNot Nothing AndAlso oConversaMensagemTemp.HorarioRecebida.HasValue Then
                        conversaMensagem.HorarioRecebida = oConversaMensagemTemp.HorarioRecebida.Value
                    End If

                    If oConversaMensagemTemp.HorarioLida IsNot Nothing AndAlso oConversaMensagemTemp.HorarioLida.HasValue Then
                        conversaMensagem.HorarioLida = oConversaMensagemTemp.HorarioLida.Value
                    End If

                    Dim o As Object = New With {
                        Key .CodConversaMensagem = conversaMensagem.CodConversaMensagem,
                        Key .CodConversa = conversaMensagem.CodConversa,
                        Key .Sentido = conversaMensagem.Sentido,
                        Key .HorarioRegistro = conversaMensagem.HorarioRegistro,
                        Key .HorarioEnviada = conversaMensagem.HorarioEnviada,
                        Key .HorarioRecebida = conversaMensagem.HorarioRecebida,
                        Key .HorarioLida = conversaMensagem.HorarioLida,
                        Key .Mensagem = conversaMensagem.Mensagem
                    }

                    oConversaMensagemRetornoList.Add(o)
                Next

                'BOUtil.EventMsg(String.Concat("oConversaMensagemRetornoList Count: ", JsonConvert.SerializeObject(oConversaMensagemRetornoList.Count)), EventLogEntryType.Warning, BOUtil.eEventID.APPService_BuscarMensagens)

                retorno = JsonConvert.SerializeObject(oConversaMensagemRetornoList)

                'BOUtil.EventMsg(String.Concat("retorno: ", retorno), EventLogEntryType.Warning, BOUtil.eEventID.APPService_BuscarMensagens)
            End If
        End If

        Return retorno

    End Function
End Class