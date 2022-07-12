using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System.ServiceModel.Security;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;

namespace SAMU192Service
{
    public partial class SAMU192Service : ServiceBase
    {
        private ServiceHost serviceHost = null;
        private ServiceHost ServiceHost { get => serviceHost; set => serviceHost = value; }

        public SAMU192Service()
        {
            InitializeComponent();
        }       

        protected override void OnStart(string[] args)
        {
#if DEBUG
            Debugger.Launch();
#endif
            try
            {
                if (ServiceHost != null)
                    ServiceHost.Close();
                //ServiceHost = new ServiceHost(typeof(SAMU192ServiceWCF));

                //ServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode =
                //        X509CertificateValidationMode.Custom;
                //ServiceHost.Credentials.ClientCertificate.Authentication.CustomCertificateValidator =
                //    new MyX509CertificateValidator("CN=Contoso.com");
                //ServiceHost.Open();
                using (ServiceHost = new ServiceHost(typeof(SAMU192ServiceWCF)))
                {
                    /*serviceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = 
                        X509CertificateValidationMode.Custom;
                    serviceHost.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = 
                        new MyX509CertificateValidator("CN=Contoso.com");*/
                    ServiceHost.Open();
                }
            }
            catch (Exception ex)
            {
                string sSource;
                string sLog;

                sSource = "SAMU192 Service";
                sLog = "Application";

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, ex.ToString(), EventLogEntryType.Error, 234);

                this.ExitCode = 1;
                this.Stop();
            }
        }

        //public class MyX509CertificateValidator : X509CertificateValidator
        //{
        //    string allowedIssuerName;

        //    public MyX509CertificateValidator(string allowedIssuerName)
        //    {
        //        if (allowedIssuerName == null)
        //        {
        //            throw new ArgumentNullException("allowedIssuerName");
        //        }

        //        this.allowedIssuerName = allowedIssuerName;
        //    }

        //    public override void Validate(X509Certificate2 certificate)
        //    {
        //        // Check that there is a certificate.
        //        if (certificate == null)
        //        {
        //            throw new ArgumentNullException("certificate");
        //        }

        //        // Check that the certificate issuer matches the configured issuer.
        //        if (allowedIssuerName != certificate.IssuerName.Name)
        //        {
        //            throw new SecurityTokenValidationException
        //              ("Certificate was not issued by a trusted issuer");
        //        }
        //    }
        //}

      

        protected override void OnStop()
        {
            if (ServiceHost != null)
            {
                ServiceHost.Close();
                ServiceHost = null;
            }
        }
    }
}
