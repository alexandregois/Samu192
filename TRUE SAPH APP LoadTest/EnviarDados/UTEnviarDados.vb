Imports System.ServiceModel
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports System.IO


<TestClass()> Public Class UnitTestEnviarDados

    <TestMethod()> Public Sub APP_EnviarDados()


        Try

            Dim cliente As ISAMU192ServiceWCF = Parametros.CriaCanal()

            '.Bairro = "PETRÓPOLIS",
            '.Cidade = "PORTO ALEGRE",

            Dim dados As New DCSolicitarAtendimentoV1 With {
                .Bairro = "Alvorada",
                .Cidade = "Batatais",
                .Complemento = "",
                .DataNascimento = Now.AddYears(-(Now.Day + Now.Month)).AddMonths(-Now.Minute).AddDays(-Now.Second),
                .FCMRegistration = "ABCDE:ABCDE",
                .Identificador = "ABCDE",
                .Logradouro = "Montenegro",
                .Latitude = -29.9999352,
                .Longitude = -51.1535568,
                .LatitudeApp = -30.0404364,
                .LongitudeApp = -51.1934393,
                .Nome = "Caio Rolando da Rocha",
                .Numero = "145",
                .OutraPessoa = False,
                .Queixa = "Desorientado",
                .Referencia = "Atrás da lixeira",
                .Sexo = "M",
                .Sistema = SAMU192InterfaceService.Utils.Enums.SistemaOperacional.Android,
                .Telefone1 = "51981598288",
                .Telefone2 = "",
                .UF = "RS"
            }

            Dim resp As Boolean = cliente.EnviarDados(dados.Dados)
            Assert.IsTrue(resp)

            Dim dados2 As New DCSolicitarAutorizacaoMidiaV1 With {.Identificador = "ABCDE"}
            resp = cliente.EnviarDados(dados2.Dados)
            Assert.IsTrue(resp)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

