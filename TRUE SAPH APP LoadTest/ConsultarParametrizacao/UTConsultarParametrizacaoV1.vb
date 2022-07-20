Imports System.ServiceModel
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Newtonsoft.Json
Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO


<TestClass()> Public Class UnitTestConsultarParametrizacaoV1

    <TestMethod()> Public Sub APP_DCConsultarParametrizacaoV1()

        Try

            Dim dados As String()

            Dim original As New DCConsultarParametrizacaoV1
            With original
                .Identificador = "ABCDE"
            End With

            dados = original.Dados

            Dim copia = New DCConsultarParametrizacaoV1(dados)

            Assert.IsFalse(BOMisc.Mudou(original.Identificador, copia.Identificador))
            Assert.IsTrue(dados(0) = DCConsultarParametrizacaoV1.VERSAO)

            Dim cliente As ISAMU192ServiceWCF = Parametros.CriaCanal()

            Dim resp = cliente.ConsultarParametrizacao(copia.Dados)
            Dim params As DCConsultarParametrizacaoV1.Parametrizacao = JsonConvert.DeserializeObject(resp, GetType(DCConsultarParametrizacaoV1.Parametrizacao))

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

