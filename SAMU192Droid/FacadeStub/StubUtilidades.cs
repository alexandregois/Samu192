using Android.App;
using Android.Content;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192Core.Utils;
using System;
using System.Collections;
using System.Linq;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubUtilidades
    {
        static bool LockBackButton = false;
        static bool FromMenu = false;

        public static void SetCultureInfo()
        {
            FacadeUtilidades.SetCultureInfo();
        }

        public static void ValidarCodigoPIN(DateTime dtNow, string pin)
        {
            FacadeUtilidades.ValidarCodigoPIN(dtNow, pin);
        }

        public static string Gerar(DateTime dtNow)
        {
            return FacadeUtilidades.Gerar(dtNow);
        }

        internal static string RecuperaInstanceID()
        {
            string instanceID = FacadeUtilidades.RecuperaInstanceID();
            return instanceID;
        }

        internal static void SalvaInstanceID(string id)
        {
            FacadeUtilidades.SalvaInstanceID(id);
        }

        internal static void LigaServico(Action<ServidorDTO> callback)
        {
            SolicitarAutorizacaoMidiaDTO solicitacao = StubWebService.MontaSolicitacaoMidia();
            FacadeUtilidades.LigaServico(callback, solicitacao);
        }

        internal static void DesligaServico()
        {
            FacadeUtilidades.DesligaServico();
        }

        internal static bool AppEmProducao()
        {
            return FacadeUtilidades.AppEmProducao();
        }

        public static bool GetLockBackButton()
        {
            return LockBackButton;
        }

        public static void SetLockBackButton(bool val)
        {
            LockBackButton = val;
        }

        public static bool GetFromMenu()
        {
            return FromMenu;
        }

        public static void SetFromMenu(bool val)
        {
            FromMenu = val;
        }

        public static String[] MontaPacoteBuscaMensagem(Enums.BuscarMensagens buscarMensagens, DateTime data)
        {
            return FacadeUtilidades.MontaPacoteBuscaMensagem(buscarMensagens, data);
        }

        public static string[] DeserializaPacote(String s, bool isObjeto)
        {
            return FacadeUtilidades.DeserializaPacote(s, isObjeto);
        }

        public static String[] MontaPacotePermiteChat()
        {
            return FacadeUtilidades.MontaPacotePermiteChat();
        }

        public static void MontaPacoteShared(SolicitarAtendimentoChatDTO pacote)
        {
            try
            {

                string[] arrPacote = ((IEnumerable)pacote).Cast<object>()
                            .Select(x => x.ToString())
                            .ToArray();

                ISharedPreferences pref = Application.Context.GetSharedPreferences("PacoteDados", FileCreationMode.Private);
                ISharedPreferencesEditor edit = pref.Edit();
                edit.PutInt("arrPacote" + "_size", arrPacote.Length);

                for (int i = 0; i < arrPacote.Length; i++)
                    edit.PutString("arrPacote" + "_" + i, arrPacote[i]);

                edit.Commit();
                edit.Apply();

            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public static void GetPacoteShared()
        {

            var pref = Application.Context.GetSharedPreferences("PacoteDados", FileCreationMode.Private);

            int size = pref.GetInt("arrPacote" + "_size", 0);

            String[] array = new String[size];
            for (int i = 0; i < size; i++)
                array[i] = pref.GetString("arrPacote" + "_" + i, null);

            IEnumerable pacote = array.Cast<SolicitarAutorizacaoMidiaDTO>();

        }


    }
}