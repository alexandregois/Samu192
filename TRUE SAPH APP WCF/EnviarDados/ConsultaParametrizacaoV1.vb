Imports Newtonsoft.Json
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class ConsultaParametrizacaoV1
    Implements IOperacaoAPP192

    Private ReadOnly _dados As DCConsultarParametrizacaoV1

    Public Sub New(dados As DCConsultarParametrizacaoV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a DCConsultarParametrizacaoV1")
        End If

        Dim oParametroBoolean As Boolean?

        Dim resp As New DCConsultarParametrizacaoV1.Parametrizacao

        If String.IsNullOrWhiteSpace(_dados.Identificador) Then
            Throw New ApplicationException("DCConsultarParametrizacaoV1 identificador não informado.")
        End If

        oParametroBoolean = BOParametro.ExtraiValorBoolean(eParametros.APP192PermiteAtendimentoChat).GetValueOrDefault(False)
        If oParametroBoolean Then
            If APP192.BOAPP192.EmAtendimentoChat(_dados.Identificador) Then
                resp.PermiteAtendimentoChat = DCConsultarParametrizacaoV1.Parametrizacao.eSituacaoAtendimentoChat.EmAtendimento
            Else
                resp.PermiteAtendimentoChat = DCConsultarParametrizacaoV1.Parametrizacao.eSituacaoAtendimentoChat.Permitido
            End If
        Else
            resp.PermiteAtendimentoChat = DCConsultarParametrizacaoV1.Parametrizacao.eSituacaoAtendimentoChat.NaoPermitido
        End If

        Return JsonConvert.SerializeObject(resp)

    End Function

End Class
