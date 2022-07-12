Imports System.ServiceModel
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports System.IO


<TestClass()> Public Class UnitTestEnviarMidia

    <TestMethod()> Public Sub APP_EnviarMidia()


        Try

            Dim cliente As ISAMU192ServiceWCF = Parametros.CriaCanal()

            Dim nome As String = "911.png"

            Dim dados As New DCEnviarMidiaV1 With {.Identificador = "ABCDE", .TipoMidia = Utils.Enums.TipoMidia.Foto, .NomeArquivo = nome}
            Dim arquivo As Byte() = File.ReadAllBytes("e:\rpneto\Imagens\x.jpg")

            Dim resp As Boolean = cliente.EnviarMidia(dados.Dados, arquivo)
            Assert.IsTrue(resp)

        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub

End Class

