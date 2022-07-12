Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO.APP192

Public Class SolicitacaoAtendimentoV1
    Implements IOperacaoAPP192

    Private ReadOnly _dados As DCSolicitarAtendimentoV1

    Public Sub New(dados As DCSolicitarAtendimentoV1)

        _dados = dados

    End Sub

    Public Function Processar(ParamArray params() As Object) As Object Implements IOperacaoAPP192.Processar

        If params.Length > 0 Then
            Throw New ApplicationException("Chamada inválida a DCSolicitarAtendimentoV1")
        End If

        If _dados.SolicitacaoPorConversa.HasValue AndAlso _dados.SolicitacaoPorConversa.Value Then
            Dim appConversa As New BOAPPConversa With {
                .Bairro = _dados.Bairro.GetValueOrDefault(),
                .Chave1 = BOAppUtil.ChaveTelefone(_dados.Telefone1.GetValueOrDefault()),
                .Chave2 = BOAppUtil.ChaveTelefone(_dados.Telefone2.GetValueOrDefault()),
                .Cidade = _dados.Cidade.GetValueOrDefault(),
                .Complemento = _dados.Complemento.GetValueOrDefault(),
                .DataNascimento = _dados.DataNascimento,
                .FcmRegistration = _dados.FCMRegistration.GetValueOrDefault(),
                .Identificador = _dados.Identificador.GetValueOrDefault(),
                .Latitude = _dados.Latitude,
                .LatitudeApp = _dados.LatitudeApp,
                .Logradouro = _dados.Logradouro.GetValueOrDefault(),
                .Longitude = _dados.Longitude,
                .LongitudeApp = _dados.LongitudeApp,
                .Nome = _dados.Nome.GetValueOrDefault(),
                .Numero = _dados.Numero.GetValueOrDefault(),
                .OutraPessoa = _dados.OutraPessoa,
                .Queixa = _dados.Queixa.GetValueOrDefault(),
                .Referencia = _dados.Referencia.GetValueOrDefault(),
                .Sexo = _dados.Sexo.GetValueOrDefault(),
                .Sistema = _dados.Sistema,
                .Telefone1 = _dados.Telefone1.GetValueOrDefault(),
                .Telefone2 = _dados.Telefone2.GetValueOrDefault(),
                .UF = _dados.UF.GetValueOrDefault,
                .EnderecoIP = APPUtil.GetClientIP()
                }

            If String.IsNullOrWhiteSpace(appConversa.UF) Then
                appConversa.UF = "RS"
            End If

            If appConversa.Sexo.ToUpper <> "M" And appConversa.Sexo.ToUpper <> "F" Then
                appConversa.Sexo = ""
            End If

            appConversa.Salva()

            Return True
        Else
            Dim lig As New BOAPPLigacao With {
                .Bairro = _dados.Bairro.GetValueOrDefault(),
                .Chave1 = BOAppUtil.ChaveTelefone(_dados.Telefone1.GetValueOrDefault()),
                .Chave2 = BOAppUtil.ChaveTelefone(_dados.Telefone2.GetValueOrDefault()),
                .Cidade = _dados.Cidade.GetValueOrDefault(),
                .Complemento = _dados.Complemento.GetValueOrDefault(),
                .DataNascimento = _dados.DataNascimento,
                .FcmRegistration = _dados.FCMRegistration.GetValueOrDefault(),
                .Identificador = _dados.Identificador.GetValueOrDefault(),
                .Latitude = _dados.Latitude,
                .LatitudeApp = _dados.LatitudeApp,
                .Logradouro = _dados.Logradouro.GetValueOrDefault(),
                .Longitude = _dados.Longitude,
                .LongitudeApp = _dados.LongitudeApp,
                .Nome = _dados.Nome.GetValueOrDefault(),
                .Numero = _dados.Numero.GetValueOrDefault(),
                .OutraPessoa = _dados.OutraPessoa,
                .Queixa = _dados.Queixa.GetValueOrDefault(),
                .Referencia = _dados.Referencia.GetValueOrDefault(),
                .Sexo = _dados.Sexo.GetValueOrDefault(),
                .Sistema = _dados.Sistema,
                .Telefone1 = _dados.Telefone1.GetValueOrDefault(),
                .Telefone2 = _dados.Telefone2.GetValueOrDefault(),
                .UF = _dados.UF.GetValueOrDefault,
                .EnderecoIP = APPUtil.GetClientIP()
            }

            If String.IsNullOrWhiteSpace(lig.UF) Then
                lig.UF = "RS"
            End If

            If lig.Sexo.ToUpper <> "M" And lig.Sexo.ToUpper <> "F" Then
                lig.Sexo = ""
            End If

            lig.Salva()

            Return True
        End If

    End Function
End Class