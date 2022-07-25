using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TRUE_SAPH_APP_FCM_WindowsService.Modelo;

namespace TRUE_SAPH_APP_FCM_WindowsService
{
    public class FCMSender
    {
        private string _senderID;
        private string _serverKey;
        private IWebProxy _proxy;

        public FCMSender(IWebProxy proxy, string senderID, string serverKey)
        {
            _proxy = proxy;
            _senderID = senderID;
            _serverKey = serverKey;
        }

        public async Task<FCMReturn> SendData(FCMRequest fcmRequest)
        {
            FCMReturn resp = new FCMReturn(false, string.Empty);

            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Proxy = _proxy;
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                dynamic firebaseBody = new ExpandoObject();
                firebaseBody.to = fcmRequest.FcmTokenOrTopic;
                firebaseBody.priority = (fcmRequest.IsHighPriority ? "high" : "normal");
                firebaseBody.content_available = fcmRequest.IsContentAvailable;

                if (fcmRequest.NotificationBody != null)
                {
                    firebaseBody.notification = new
                    {
                        title = fcmRequest.NotificationBody.Title,
                        body = fcmRequest.NotificationBody.Body
                    };
                }

                if (fcmRequest.DataBody != null)
                {
                    firebaseBody.data = new
                    {
                        serializedJson = JsonConvert.SerializeObject(fcmRequest.DataBody)
                    };
                }

                string json = JsonConvert.SerializeObject(firebaseBody);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", _serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", _senderID));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                dynamic ret = JsonConvert.DeserializeObject(tReader.ReadToEnd());

                                if (ret.failure == "1")
                                {
                                    resp.Error = true;
                                    resp.Message = ret.results[0].error;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resp = new FCMReturn(true, ex.Message);
            }

            return await Task.FromResult(resp);
        }
    }
}