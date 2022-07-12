Imports System.ServiceModel
Imports SAMU192InterfaceService

Public Class Parametros

    'Public Const SERVICE_URI As String = "http://localhost:8733/Design_Time_Addresses/TRUE_SAPH_APP_WCF/Service1/"
    Public Const SERVICE_URI As String = "http://localhost:30043/TRUE_SAPH_APP_WCF/Service1/" 'RPNETO
    'Public Const SERVICE_URI As String = "http://interno.true.com.br:30043/TRUE_SAPH_APP_WCF/Service1/" 'RPNETO
    Public Const SERVICE_USERNAME As String = "unimedpoa.ws" 'TRUE
    Public Const SERVICE_PASSWORD As String = "D25mes06" 'TRUE

    Public Shared Function CriaCanal() As ISAMU192ServiceWCF

        If SERVICE_URI.ToLower.StartsWith("https") Then

            Dim binding As New BasicHttpsBinding() With {
            .MaxReceivedMessageSize = 3000000,
            .CloseTimeout = New TimeSpan(0, 10, 0),
            .OpenTimeout = New TimeSpan(0, 10, 0),
            .SendTimeout = New TimeSpan(0, 10, 0),
            .ReceiveTimeout = New TimeSpan(0, 10, 0)}

            binding.Security.Mode = BasicHttpsSecurityMode.Transport
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic

            Dim endpoint As New EndpointAddress(SERVICE_URI)
            Dim cf As ChannelFactory(Of ISAMU192ServiceWCF) = New ChannelFactory(Of ISAMU192ServiceWCF)(
            binding,
            endpoint)

            cf.Credentials.UserName.UserName = SERVICE_USERNAME
            cf.Credentials.UserName.Password = SERVICE_PASSWORD

            Return cf.CreateChannel()
        Else

            Dim cf As ChannelFactory(Of ISAMU192ServiceWCF) = New ChannelFactory(Of ISAMU192ServiceWCF)(
                New BasicHttpBinding() With {.MaxReceivedMessageSize = 3000000},
                New EndpointAddress(SERVICE_URI))

            Return cf.CreateChannel()
        End If

    End Function



End Class
