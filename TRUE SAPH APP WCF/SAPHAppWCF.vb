Imports System.IO
Imports SAMU192InterfaceService
Imports SAMU192InterfaceService.DataContracts
Imports SAPHBO

Public Class SAPHAppWCF
    Implements ISAMU192ServiceWCF

    Public Function EnviarDados(dados() As String) As Boolean Implements ISAMU192ServiceWCF.EnviarDados

        Try

            AjustesIniciais()

            Dim info As IOperacaoAPP192 = DecifraObjeto(dados)
            If info IsNot Nothing Then
                Return info.Processar()
            Else
                BOUtil.EventMsg("Objeto não identificado", EventLogEntryType.Error, BOUtil.eEventID.APPService_EnviarDados)
                Return False
            End If

        Catch ex As Exception
            BOUtil.EventMsg(ex.ToString, EventLogEntryType.Error, BOUtil.eEventID.APPService_EnviarDados)
            Return False
        End Try

    End Function

    Public Function EnviarMidia(dados() As String, midia() As Byte) As Boolean Implements ISAMU192ServiceWCF.EnviarMidia

        Try

            AjustesIniciais()

            Dim info As IOperacaoAPP192 = DecifraObjeto(dados)
            If info IsNot Nothing Then
                Return info.Processar(midia)
            Else
                BOUtil.EventMsg("Objeto não identificado", EventLogEntryType.Error, BOUtil.eEventID.APPService_EnviarMidia)
                Return False
            End If

        Catch ex As Exception
            BOUtil.EventMsg(ex.ToString, EventLogEntryType.Error, BOUtil.eEventID.APPService_EnviarMidia)
            Return False
        End Try

    End Function

    Public Function EnviarMensagens(dados As String()) As String Implements ISAMU192ServiceWCF.EnviarMensagens

        Dim retorno As String = String.Empty

        Try

            AjustesIniciais()

            Dim info As IOperacaoAPP192 = DecifraObjeto(dados)

            If info IsNot Nothing Then
                Return info.Processar()
            Else
                'BOUtil.EventMsg("Objeto não identificado", EventLogEntryType.Error, BOUtil.eEventID.APPService_BuscarMensagens)
            End If

        Catch ex As Exception
            'BOUtil.EventMsg(ex.ToString, EventLogEntryType.Error, BOUtil.eEventID.APPService_BuscarMensagens)
        End Try

        Return retorno

    End Function

    Public Function BuscarMensagens(dados As String()) As String Implements ISAMU192ServiceWCF.BuscarMensagens

        Dim retorno As String = String.Empty

        Try

            AjustesIniciais()

            Dim info As IOperacaoAPP192 = DecifraObjeto(dados)

            If info IsNot Nothing Then
                Return info.Processar()
            Else
                BOUtil.EventMsg("Objeto não identificado", EventLogEntryType.Error, BOUtil.eEventID.APPService_BuscarMensagens)
            End If

        Catch ex As Exception
            BOUtil.EventMsg(ex.ToString, EventLogEntryType.Error, BOUtil.eEventID.APPService_BuscarMensagens)
        End Try

        Return retorno

    End Function

    Public Function ConsultaParametrizacao(dados As String()) As String Implements ISAMU192ServiceWCF.ConsultarParametrizacao

        Dim retorno As String = String.Empty

        Try

            AjustesIniciais()

            Dim info As IOperacaoAPP192 = DecifraObjeto(dados)

            If info IsNot Nothing Then
                Return info.Processar()
            Else
                BOUtil.EventMsg("Objeto não identificado", EventLogEntryType.Error, BOUtil.eEventID.APPService_ConsultaParametrizacao)
            End If

        Catch ex As Exception
            BOUtil.EventMsg(ex.ToString, EventLogEntryType.Error, BOUtil.eEventID.APPService_ConsultaParametrizacao)
        End Try

        Return retorno

    End Function

    Public Function DecifraObjeto(dados() As String) As IOperacaoAPP192

        If dados Is Nothing OrElse dados.Length = 0 OrElse String.IsNullOrWhiteSpace(dados(0)) Then
            Return Nothing
        End If

        Select Case dados(0)
            Case DCEnviarMidiaV1.VERSAO
                Return New SolicitacaoMidiaV1(New DCEnviarMidiaV1(dados))
            Case DCSolicitarAtendimentoV1.VERSAO
                Return New SolicitacaoAtendimentoV1(New DCSolicitarAtendimentoV1(dados))
            Case DCSolicitarAtendimentoChatV1.VERSAO
                Return New SolicitacaoAtendimentoChatV1(New DCSolicitarAtendimentoChatV1(dados))
            Case DCSolicitarAutorizacaoMidiaV1.VERSAO
                Return New SolicitacaoAutorizacaoMidiaV1(New DCSolicitarAutorizacaoMidiaV1(dados))
            Case DCEnviarMensagemV1.VERSAO
                Return New EnviaMensagemV1(New DCEnviarMensagemV1(dados))
            Case DCAtualizarMensagemV1.VERSAO
                Return New AtualizaMensagemV1(New DCAtualizarMensagemV1(dados))
            Case DCBuscarMensagemV1.VERSAO
                Return New BuscaMensagemV1(New DCBuscarMensagemV1(dados))
            Case DCConsultarParametrizacaoV1.VERSAO
                Return New ConsultaParametrizacaoV1(New DCConsultarParametrizacaoV1(dados))
        End Select

        Return Nothing

    End Function

    Private _conexaoInicializada As Boolean = False

    Private Sub AjustesIniciais()

        If Not _conexaoInicializada Then

            Dim arquivo As String = String.Empty
            Try
                BOUtil.EventSource = "TRUE SAPH APP WCF"
                Dim pathExe As String = Path.GetDirectoryName(New Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath)
                arquivo = Path.Combine(pathExe, "language\language.xml")
                TrueML.ML.Inicializa(arquivo)
                BOUtil.InicializaBancos()
                _conexaoInicializada = True

            Catch ex As Exception
                Throw New ApplicationException(String.Format("{0} - {1}", arquivo, ex.ToString))
            End Try
        End If

    End Sub

End Class