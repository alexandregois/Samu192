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

            'Dim dados2 As New DCSolicitarAutorizacaoMidiaV1 With {.Identificador = "ABCDE"}
            'resp = cliente.EnviarDados(dados2.Dados)
            'Assert.IsTrue(resp)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

    <TestMethod()> Public Sub APP_EnviarDadosChat()


        Try

            Dim cliente As ISAMU192ServiceWCF = Parametros.CriaCanal()

            '.Bairro = "PETRÓPOLIS",
            '.Cidade = "PORTO ALEGRE",

            Dim dados As New DCSolicitarAtendimentoChatV1 With {
                .Bairro = "Petropolis",
                .Cidade = "Porto Alegre",
                .Complemento = "",
                .DataNascimento = Now.AddYears(-(Now.Day + Now.Month)).AddMonths(-Now.Minute).AddDays(-Now.Second),
                .FCMRegistration = "12345:12345",
                .Identificador = "12345",
                .Logradouro = "Montenegro",
                .Latitude = -29.9999352,
                .Longitude = -51.1535568,
                .LatitudeApp = -30.0404364,
                .LongitudeApp = -51.1934393,
                .Nome = "",
                .Numero = "145",
                .OutraPessoa = False,
                .Queixa = "Caiu da banheira",
                .Referencia = "",
                .Sexo = "",
                .Sistema = SAMU192InterfaceService.Utils.Enums.SistemaOperacional.Android,
                .Telefone1 = "51981598288",
                .Telefone2 = "",
                .UF = "RS"
            }

            Dim resp As Boolean = cliente.EnviarDados(dados.Dados)
            Assert.IsTrue(resp)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

    <TestMethod()> Public Sub APP_EnviarDadosChatErro()


        Try

            Dim cliente As ISAMU192ServiceWCF = Parametros.CriaCanal()

            '.Bairro = "PETRÓPOLIS",
            '.Cidade = "PORTO ALEGRE",

            Dim dados As New DCSolicitarAtendimentoChatV1
            With dados
                .Telefone1 = "51995816448"
                .Telefone2 = "5133276000"
                .Nome = "Solicitante PNE"
                .Sexo = "M"
                .DataNascimento = New Date(1999, 9, 9)
                .OutraPessoa = False
                .LongitudeApp = -29.16687
                .LatitudeApp = -51.1801
                .Sistema = SAMU192InterfaceService.Utils.Enums.SistemaOperacional.Android
                .Identificador = "dBDpWCSc4PY"
                .FCMRegistration = "dBDpWCSc4PY:APA91bHKyZjF2R13OWvDN4JA5o_ORrSEDXuN2O9_jf3t8DuMqDfad5oYrAN7Lz_QVy27VZq9d2eZ8dOKxswAUJ75"
            End With

            Dim resp As Boolean = cliente.EnviarDados(dados.Dados)
            Assert.IsTrue(resp)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

