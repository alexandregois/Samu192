Imports System.ServiceModel
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
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

            Assert.IsFalse(BOMisc.Mudou(original.Identificador, original.Identificador))
            Assert.IsTrue(dados(0) = DCSolicitarAutorizacaoMidiaV1.VERSAO)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

