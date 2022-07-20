Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO
Imports SAPHBO.APP192
Imports SAPHPBO.Enumeradores

Public Class SolicitacaoAtendimentoChatV1
    Implements IOperacaoAPP192

    Private ReadOnly _dados As DCSolicitarAtendimentoChatV1

    Public Sub New(dados As DCSolicitarAtendimentoChatV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a DCSolicitarAtendimentoChatV1")
        End If

        Try

            Dim appConversaCompletada As New BOAPPConversaCompletada With {
                .Nome = _dados.Nome.GetValueOrDefault(),
                .Sexo = _dados.Sexo.GetValueOrDefault(),
                .DataNascimento = _dados.DataNascimento,
                .Cidade = _dados.Cidade.GetValueOrDefault(),
                .Bairro = _dados.Bairro.GetValueOrDefault(),
                .Logradouro = _dados.Logradouro.GetValueOrDefault(),
                .Numero = _dados.Numero.GetValueOrDefault(),
                .Complemento = _dados.Complemento.GetValueOrDefault(),
                .Referencia = _dados.Referencia.GetValueOrDefault(),
                .Latitude = _dados.Latitude,
                .Longitude = _dados.Longitude,
                .Queixa = _dados.Queixa.GetValueOrDefault(),
                .FcmRegistration = _dados.FCMRegistration.GetValueOrDefault(),
                .Identificador = _dados.Identificador.GetValueOrDefault(),
                .LatitudeApp = _dados.LatitudeApp,
                .LongitudeApp = _dados.LongitudeApp,
                .OutraPessoa = _dados.OutraPessoa,
                .Sistema = _dados.Sistema,
                .Telefone1 = _dados.Telefone1.GetValueOrDefault(),
                .Telefone2 = _dados.Telefone2.GetValueOrDefault(),
                .EnderecoIP = APPUtil.GetClientIP(),
                .UF = _dados.UF.GetValueOrDefault
                }

            If String.IsNullOrWhiteSpace(appConversaCompletada.UF) Then
                appConversaCompletada.UF = "RS"
            End If

            If appConversaCompletada.Sexo.ToUpper <> "M" And appConversaCompletada.Sexo.ToUpper <> "F" Then
                appConversaCompletada.Sexo = ""
            End If

            Dim usuario As New BOUsuario
            usuario.Carrega(eUsuariosEspeciais.SAPHAdmin)

            BOChamado.AbreSolicitacaoSocorroChat(usuario, appConversaCompletada)

            Return True

        Catch ex As Exception
            BOUtil.EventMsg(ex.ToString, EventLogEntryType.Error, BOUtil.eEventID.APPService_EnviarDados)
            Return False
        End Try

    End Function

End Class