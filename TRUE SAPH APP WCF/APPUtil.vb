Imports System.ServiceModel.Channels
Imports SAPHBO

Public Class APPUtil

    Public Shared Function GetClientIP() As String

        Try
            Dim context As OperationContext = OperationContext.Current
            Dim prop As MessageProperties = context.IncomingMessageProperties
            Dim endpoint As RemoteEndpointMessageProperty = prop(RemoteEndpointMessageProperty.Name)
            If endpoint.Address.Length > 39 Then
                Throw New ApplicationException(String.Format("O valor {0} não aparenta ser IP v4 ou v6 válido!", endpoint.Address))
            End If
            Return endpoint.Address
        Catch ex As Exception
            BOUtil.EventMsg(String.Format("Não foi possível obter o endereço da conexão! {0}", ex.ToString), EventLogEntryType.Error, BOUtil.eEventID.AppService_WCFGetIPClientError)
            Return String.Empty
        End Try

    End Function


End Class
