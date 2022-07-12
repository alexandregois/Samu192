Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

<TestClass()> Public Class UnitTestEnviarMidiaV1

    <TestMethod()> Public Sub APP_DCEnviarMidiaV1()


        Try

            Dim dados As String()

            Dim original As New DCEnviarMidiaV1
            With original
                .Identificador = "ABCDE"
                .TipoMidia = SAMU192InterfaceService.Utils.Enums.TipoMidia.Foto
            End With

            dados = original.Dados

            Dim copia = New DCEnviarMidiaV1(dados)

            Assert.IsFalse(BOMisc.Mudou(original.Identificador, original.Identificador))
            Assert.IsFalse(BOMisc.Mudou(original.TipoMidiaStr, original.TipoMidiaStr))
            Assert.IsTrue(dados(0) = DCEnviarMidiaV1.VERSAO)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class
