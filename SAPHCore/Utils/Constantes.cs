using System;

namespace SAMU192Core.Utils
{
    public class Constantes
    {
        /*
         * IMPORTANTE
         * 
         * AS CONSTANTES MARCADAS COM ATENÇÂO DEVEM SER OBSERVADAS COM CUIDADO
         * POIS OS VALORES SÃO DIFERENTES PARA PUBLICACAO NA UNIMED E NA TRUE
         * 
         */

        //TODO: Coordenada default deve ser conforme o servidor da área de cobertura??
        public const double DEFAULT_LATITUDE = -30.0277012345678, DEFAULT_LONGITUDE = -51.2287012345678; //Default (quando sem GPS) Prefeitura de Porto Alegre

        public const int SERVICE_OPEN_TIMEOUT_SECONDS = 15;
        public const int SERVICE_CLOSE_TIMEOUT_SECONDS = 30;
        public const int SERVICE_SEND_TIMEOUT_SECONDS = 30;
        public const int SERVICE_RECEIVE_TIMEOUT_SECONDS = 10;

        public const int REFRESH_PARAMETROS_MINUTOS = 5;
        public const int REFRESH_ACOMOPANHAMENTOS_SECONDS = 5;

        public const int DROID_MAX_VIDEO_SIZE_KB = 4000;
        public const int DROID_IMG_MAX_DIMENSION = 1800;

        /*
         * A T E N Ç Ã O - P R O D U Ç Ã O
         */
        public const bool APP_EM_PRODUCAO = true;
        public const bool APPCENTER_ANALYTICS_ACTIVATED = true;
        public const bool APPCENTER_CRASHES_ACTIVATED = true;
        public const string APP_CENTER_ANDROID = "6124f07d-9dbf-41ad-9d6b-6862fd0d6412";//Homolg
        public const string APP_CENTER_IOS = "af92f131-ec72-4e48-a3d3-95de938be524";//Homolg

        /*
         * A T E N Ç Ã O - T E L E F O N E
         */
        //public const string NUMERO_TELEFONE = "05133276093"; //TRUE Homolog
        public const string NUMERO_TELEFONE = "05133276048"; //TRUE Homolog
        //public const string NUMERO_TELEFONE = "192"; //SAMU - Homolog/Producao

        /*
         * A T E N Ç Ã O - G E O C O D E F A R M
         */

        public const string GEOCODE_KEY = "8c8c0e82-0d0e49409de8-dd0a0d968f28"; //TRUE

        public const string GEOCODE_URL_API = "https://www.geocode.farm/v3/xml/forward/?addr={0}&country=br&lang=pt&count=1&key={1}"; //ttps://www.geocode.farm/v3/xml/forward/?addr=/*/
        public const string GEOCODE_URL_API_REV = "https://www.geocode.farm/v3/xml/reverse/?lat={0}&lon={1}&country=br&lang=pt&count=1&key={2}"; //ttps://www.geocode.farm/v3/xml/reverse/?country=br&lang=pt&count=1&";        

        /*
        *   HERE API
        */
        public const string GEOCODEHERE_APPID = "hoPMJwFM9TPDw87d9bFc"; //samu192Geo
        public const string GEOCODEHERE_APPCODE = "cgSRNyaPCSqyWvcxM9RtRw"; //samu192Geo
        public const string GEOCODEHERE_URL_API = "https://geocoder.api.here.com/6.2/geocode.json?app_id={0}&app_code={1}&searchtext={2}";
        public const string GEOCODEHERE_URL_API_REV = "https://reverse.geocoder.api.here.com/6.2/reversegeocode.json?app_id={0}&app_code={1}&mode=retrieveAddresses&prox={2},{3},{4}&maxresults=20";
        public const string GEOCODEHERE_DISTANCIA_REV = "250";

    }
}
