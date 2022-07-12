Imports System.ServiceModel
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO


<TestClass()> Public Class UnitTestSolicitarAtendimentoV1

    <TestMethod()> Public Sub APP_DCSolicitarAtendimentoV1()


        Try

            Dim dados As String()

            Dim original As New DCSolicitarAtendimentoV1
            With original
                .Bairro = "Wallaby Way"
                .Cidade = "Sydney"
                .Complemento = ""
                .DataNascimento = Now.AddYears(-(Now.Day + Now.Month))
                .FCMRegistration = "ABCDE:ABCDE"
                .Identificador = "ABCDE"
                .Logradouro = "P Sherman"
                .Latitude = -29.9999352
                .Longitude = -51.1535568
                .LatitudeApp = -30.0404364
                .LongitudeApp = -51.1934393
                .Nome = "Caio Rolando da Rocha"
                .Numero = 42
                .OutraPessoa = False
                .Queixa = "Desorientado"
                .Referencia = "Eu sei, mas eu precisava de um bairro"
                .Sexo = "M"
                .Sistema = SAMU192InterfaceService.Utils.Enums.SistemaOperacional.Android
                .Telefone1 = "51987654321"
                .Telefone2 = ""
            End With

            dados = original.Dados

            Dim copia = New DCSolicitarAtendimentoV1(dados)

            Assert.IsFalse(BOMisc.Mudou(original.Bairro, original.Bairro))
            Assert.IsFalse(BOMisc.Mudou(original.Cidade, original.Cidade))
            Assert.IsFalse(BOMisc.Mudou(original.Complemento, original.Complemento))
            Assert.IsFalse(BOMisc.Mudou(original.DataNascimentoStr, original.DataNascimentoStr))
            Assert.IsFalse(BOMisc.Mudou(original.FCMRegistration, original.FCMRegistration))
            Assert.IsFalse(BOMisc.Mudou(original.Identificador, original.Identificador))
            Assert.IsFalse(BOMisc.Mudou(original.Logradouro, original.Logradouro))
            Assert.IsFalse(BOMisc.Mudou(original.LatitudeStr, original.LatitudeStr))
            Assert.IsFalse(BOMisc.Mudou(original.LongitudeStr, original.LongitudeStr))
            Assert.IsFalse(BOMisc.Mudou(original.LatitudeAppStr, original.LatitudeAppStr))
            Assert.IsFalse(BOMisc.Mudou(original.LongitudeAppStr, original.LongitudeAppStr))
            Assert.IsFalse(BOMisc.Mudou(original.Nome, original.Nome))
            Assert.IsFalse(BOMisc.Mudou(original.Numero, original.Numero))
            Assert.IsFalse(BOMisc.Mudou(original.OutraPessoa, original.OutraPessoa))
            Assert.IsFalse(BOMisc.Mudou(original.Queixa, original.Queixa))
            Assert.IsFalse(BOMisc.Mudou(original.Referencia, original.Referencia))
            Assert.IsFalse(BOMisc.Mudou(original.Sexo, original.Sexo))
            Assert.IsFalse(BOMisc.Mudou(original.Sistema, original.Sistema))
            Assert.IsFalse(BOMisc.Mudou(original.Telefone1, original.Telefone1))
            Assert.IsFalse(BOMisc.Mudou(original.Telefone2, original.Telefone2))
            Assert.IsTrue(dados(0) = DCSolicitarAtendimentoV1.VERSAO)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

