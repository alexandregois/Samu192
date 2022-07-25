using System;

namespace TRUE_SAPH_APP_FCM_WindowsService.Modelo
{
    /// <summary>
    /// Referência: https://firebase.google.com/docs/cloud-messaging/http-server-ref?hl=pt-br
    /// </summary>
    public class FCMRequest
    {
        /// <summary>
        /// Este parâmetro especifica o destinatário de uma mensagem.
        /// O valor pode ser o token de registro de um dispositivo, a chave de notificação de um grupo de dispositivos ou um único tópico(prefixado com /topics/).
        /// Para enviar a vários tópicos, use o parâmetro condition.
        /// </summary>
        public string FcmTokenOrTopic { get; set; }

        /// <summary>
        /// Define a prioridade da mensagem. Os valores válidos são "normal" e "high" (alta). Nas plataformas Apple, eles correspondem às prioridades 5 e 10 de APNs.
        /// Por padrão, as mensagens de notificação são enviadas com prioridade alta e as mensagens de dados com prioridade normal.A prioridade normal otimiza o consumo de bateria do app cliente e deve ser sempre usada, exceto quando a entrega imediata é necessária.Mensagens com prioridade normal podem ser recebidas pelo app com atraso não especificado.
        /// Uma mensagem com prioridade alta é enviada imediatamente, e o app pode exibir uma notificação.
        /// </summary>
        public bool IsHighPriority { get; set; }

        /// <summary>
        /// Nas plataformas Apple, use este campo para representar content-available no payload de APNs. 
        /// Quando uma notificação ou mensagem é enviada e este campo está definido como true, um app cliente inativo é ativado, e a mensagem é enviada por meio de APNs como uma notificação silenciosa e não por meio do servidor de conexão do FCM. 
        /// Não é garantido que essas notificações silenciosas em APNs sejam entregues, e isso pode depender de fatores como o usuário ativar o modo de baixo consumo, saída forçada do app etc. 
        /// No Android, as mensagens de dados ativam o app por padrão. Ainda não há compatibilidade com o Chrome.
        /// </summary>
        public bool IsContentAvailable { get; set; }

        /// <summary>
        /// Este parâmetro especifica os pares chave-valor predefinidos do payload da notificação, visíveis ao usuário. 
        /// Consulte detalhes no suporte ao payload de notificação. 
        /// Para ver mais informações sobre a mensagem de notificação e opções de mensagens de dados, consulte Tipos de mensagens. 
        /// Se houver um payload de notificação ou a opção content_available estiver definida como true em uma mensagem para um dispositivo da Apple, a mensagem vai ser enviada por APNs. 
        /// Caso contrário, vai ser enviada pelo FCM.
        /// </summary>
        public Notification NotificationBody { get; set; }

        /// <summary>
        /// Os pares de chave/valor de payload da mensagem são especificados nesse parâmetro.
        /// Este parâmetro especifica os pares de chave-valor personalizados do payload da mensagem.
        /// Por exemplo, com data:{"score":"3x1"}
        /// Não há limite para o número de parâmetros de chave-valor, mas há um limite de tamanho de mensagem total de 4000 bytes.
        /// </summary>
        public object DataBody { get; set; }

        /// <summary>
        /// Especifica os pares chave-valor predefinidos do payload da notificação, visíveis ao usuário.
        /// </summary>
        public class Notification
        {
            private static string _title;
            private static string _body;

            /// <summary>
            /// Especifica os pares chave-valor predefinidos do payload da notificação, visíveis ao usuário.
            /// </summary>
            /// <param name="title">O título da notificação.</param>
            /// <param name="body">O texto do corpo da notificação.</param>
            public Notification(string title, string body)
            {
                _title = title;
                _body = body;
            }

            /// <summary>
            /// O título da notificação.
            /// Este campo não está visível em smartphones e tablets.
            /// </summary>
            public string Title { get { return _title; } }

            /// <summary>
            /// O texto do corpo da notificação.
            /// </summary>
            public string Body { get { return _body; } }
        }
    }
}