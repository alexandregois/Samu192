Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class SolicitacaoMidiaV1
    Implements IOperacaoAPP192

    Private _dados As DCEnviarMidiaV1

    Public Sub New(dados As DCEnviarMidiaV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length <> 1 Then
            Throw New ApplicationException("Chamada inválida a SolicitacaoMidiaV1")
        End If

        If String.IsNullOrWhiteSpace(_dados.Identificador) Then
            Return False
        End If

        Dim CodChamado As Nullable(Of Integer) = APP192.BOAPP192.ChamadoParaAtrelarMidia(_dados.Identificador)

        If Not CodChamado.HasValue Then
            Return False
        End If

        Dim anexo As New BOAnexo
        Dim existe As Boolean = True
        Dim arquivo As String = _dados.NomeArquivo
        Dim num As Integer = 0

        While existe
            If Not anexo.CarregaAK1(_dados.Identificador, arquivo) Then
                existe = False
            Else
                'Se o nome já existe, adiciona um número sequencia ao nome, no mesmo molde do Windows
                num += 1
                If arquivo.Contains(".") Then
                    arquivo = String.Format("{0}({1}).{2}", _dados.NomeArquivo.Substring(0, _dados.NomeArquivo.LastIndexOf(".")), num.ToString, _dados.NomeArquivo.Substring(_dados.NomeArquivo.LastIndexOf(".") + 1))
                Else
                    arquivo = String.Format("{0}({1})", _dados.NomeArquivo, num.ToString)
                End If
            End If
        End While

        anexo = New BOAnexo With {
                .Anexo = params(0),
                .Data = BOMisc.DataHoraAtual,
                .MessageID = _dados.Identificador,
                .NomeAnexo = arquivo,
                .Origem = BOAnexo.eOrigemAnexo.APP192,
                .CodChamado = CodChamado
                }

        anexo.Salva()

        Return True

    End Function

End Class
