Imports System.ServiceModel
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO


<TestClass()> Public Class UnitTestSolicitarAutorizacaoMidiaV1

    <TestMethod()> Public Sub APP_DCSolicitarAutorizacaoMidiaV1()

        Try

            Dim dados As String()

            Dim original As New DCSolicitarAutorizacaoMidiaV1
            With original
                .Identificador = "ABCDE"
            End With

            dados = original.Dados

            Dim copia = New DCSolicitarAutorizacaoMidiaV1(dados)

            Assert.IsFalse(BOMisc.Mudou(original.Identificador, copia.Identificador))
            Assert.IsTrue(dados(0) = DCSolicitarAutorizacaoMidiaV1.VERSAO)

            Dim cliente As ISAMU192ServiceWCF = Parametros.CriaCanal()

            Assert.IsTrue(cliente.EnviarDados(copia.Dados))


        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

