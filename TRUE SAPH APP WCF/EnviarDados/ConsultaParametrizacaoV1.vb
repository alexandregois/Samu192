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

        Dim DCConsultarParametrizacaoV1 As New DCConsultarParametrizacaoV1.Parametrizacao

        oParametroBoolean = BOParametro.ExtraiValorBoolean(eParametros.PermiteAtendimentoChat)

        If oParametroBoolean IsNot Nothing AndAlso oParametroBoolean.HasValue Then
            DCConsultarParametrizacaoV1.PermiteAtendimentoChat = IIf(oParametroBoolean.Value, True, False)
        End If

        Dim retorno As String = JsonConvert.SerializeObject(DCConsultarParametrizacaoV1)

        Return retorno

    End Function
End Class
