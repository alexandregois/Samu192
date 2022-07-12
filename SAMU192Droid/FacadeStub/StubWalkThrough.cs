using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using SAMU192Droid.Interface;
using SAMU192Droid.Interface.Activities;
using SAMU192Droid.Interface.Fragments;

namespace SAMU192Droid.FacadeStub
{
    public static class StubWalkThrough
    {
        public static eWalkThrough CurrentWalkThroughFragment { get; set; }
        static eEstado estado = eEstado.Walk;
        static bool WalkThrough = false;
        static bool InWalkThrough = false;

        public static BaseFragment LastSelectedFragmentMainActivity = null;

        public enum eWalkThrough
        {
            Termo = 0,
            AtiveGps = 1,
            Cadastro = 2,
            Favoritos = 3,
            Ligacao = 4,
            Regiao = 5,
            Sabedoria = 6
        }
        
        public static void AtualizaContadorWalkThroughFragment()
        {
            CurrentWalkThroughFragment++;
            if ((int)CurrentWalkThroughFragment > Enum.GetNames(typeof(eWalkThrough)).Length - 1)
                CurrentWalkThroughFragment = 0;
        }

        public static void VoltaContadorWalkthought()
        {
            CurrentWalkThroughFragment = (eWalkThrough)((int)CurrentWalkThroughFragment - 1);
        }

        public static WalkthroughFragment SelectFragment()
        {
            WalkthroughFragment f;

            switch (CurrentWalkThroughFragment)
            {
                case eWalkThrough.Termo:
                    StubUtilidades.SetLockBackButton(true);
                    f = new Walkthrough1Fragment();
                    break;
                case eWalkThrough.AtiveGps:
                    StubUtilidades.SetLockBackButton(true);
                    f = new Walkthrough2Fragment();
                    break;
                case eWalkThrough.Cadastro:
                    StubUtilidades.SetLockBackButton(false);
                    f = new Walkthrough3Fragment();
                    break;
                case eWalkThrough.Favoritos:
                    StubUtilidades.SetLockBackButton(false);
                    f = new Walkthrough4Fragment();
                    break;
                case eWalkThrough.Ligacao:
                    StubUtilidades.SetLockBackButton(false);
                    f = new Walkthrough5Fragment();
                    break;
                case eWalkThrough.Regiao:
                    StubUtilidades.SetLockBackButton(false);
                    f = new Walkthrough6Fragment();
                    break;
                case eWalkThrough.Sabedoria:
                    StubUtilidades.SetLockBackButton(false);
                    f = new Walkthrough7Fragment();
                    break;
                default:
                    StubUtilidades.SetLockBackButton(true);
                    f = new Walkthrough1Fragment();
                    break;
            }
            return f;
        }

        internal static void SelectFragmentOnMainActivity(MainActivity mainActivity)
        {
            try
            {

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(mainActivity);
                ISharedPreferencesEditor editor = prefs.Edit();
                var chat = prefs.GetInt("chat", 0);


                Utils.Interface.LimparPilhaFragments(mainActivity);
                switch (Estado)
                {
                    case eEstado.Walk:
                        break;
                    case eEstado.Cadastro:
                        LastSelectedFragmentMainActivity = new CadastroFragment();
                        Utils.Interface.EmpilharFragment(mainActivity, Resource.Id.tabFrameLayout1, LastSelectedFragmentMainActivity, Resource.String.cadastro_title, CadastroFragment.TAG);
                        break;
                    case eEstado.Favoritos:
                        LastSelectedFragmentMainActivity = new FavoritosFragment();
                        Utils.Interface.EmpilharFragment(mainActivity, Resource.Id.tabFrameLayout1, LastSelectedFragmentMainActivity, Resource.String.favoritos_title, FavoritosFragment.TAG);
                        break;
                    case eEstado.Pronto:

                        //Muda de acordo com o tipo de usuário (chat ou não).
                        if (chat == 0)
                        {
                            LastSelectedFragmentMainActivity = new InicialFragment();
                            Utils.Interface.EmpilharFragment(mainActivity, Resource.Id.tabFrameLayout1, LastSelectedFragmentMainActivity, Resource.String.chamar_title, InicialFragment.TAG);
                        }
                        else
                        {
                            LastSelectedFragmentMainActivity = new Inicial2Fragment();
                            Utils.Interface.EmpilharFragment(mainActivity, Resource.Id.tabFrameLayout1, LastSelectedFragmentMainActivity, Resource.String.chamar_title, Inicial2Fragment.TAG);
                        }

                            break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public static int SelectFragmentLayout()
        {
            switch (CurrentWalkThroughFragment)
            {
                case eWalkThrough.Termo:
                    return Resource.Layout.walkthrough1;
                case eWalkThrough.AtiveGps:
                    return Resource.Layout.walkthrough2;
                case eWalkThrough.Cadastro:
                    return Resource.Layout.walkthrough3;
                case eWalkThrough.Favoritos:
                    return Resource.Layout.walkthrough4;
                case eWalkThrough.Ligacao:
                    return Resource.Layout.walkthrough5;
                case eWalkThrough.Regiao:
                    return Resource.Layout.walkthrough6;
                case eWalkThrough.Sabedoria:
                    return Resource.Layout.walkthrough7;
                default:
                    return Resource.Layout.walkthrough1;
            }
        }

        public static int SelectFragmentImageView(int index)
        {
            switch (index)
            {
                case 1:
                    return Resource.Id.walk_page_1_iv;
                case 2:
                    return Resource.Id.walk_page_2_iv;
                case 3:
                    return Resource.Id.walk_page_3_iv;
                case 4:
                    return Resource.Id.walk_page_4_iv;
                case 5:
                    return Resource.Id.walk_page_5_iv;
                case 6:
                    return Resource.Id.walk_page_6_iv;
                default:
                    return -1;
            }
        }

        public static eEstado Estado
        {
            get
            {
                if (estado == eEstado.Pronto)
                    return estado;
                else
                if (!StubCadastro.RecuperaAceiteTermo().Aceite)
                    return eEstado.Termo;
                else
                if (!StubCadastro.ExisteCadastro())
                    return eEstado.Walk;
                else
                    return eEstado.Pronto;
            }
            set => estado = value;
        }

        public enum eEstado
        {
            Termo = 0,
            Walk = 1,
            Cadastro = 2,
            Favoritos = 3,
            Ligacao = 4,
            Pronto = 5,
            Fora = 6
        }

        public static void DirecionaWalkthrough(Activity activity, StubWalkThrough.eWalkThrough index)
        {
            StubWalkThrough.CurrentWalkThroughFragment = index;
            Utils.Interface.SubstituirFragment((MainActivity)activity, Resource.Id.walk_fl, SelectFragment(), Resource.String.chamar_title, InicialFragment.TAG);
        }

        public static void RetornaWalkthrough(Activity activity)
        {
            VoltaContadorWalkthought();
            Utils.Interface.SubstituirFragment((MainActivity)activity, Resource.Id.walk_fl, SelectFragment(), Resource.String.chamar_title, InicialFragment.TAG);
        }


        public static void ProssegueWalkthrough(Activity activity)
        {
            AtualizaContadorWalkThroughFragment();
            Utils.Interface.SubstituirFragment((MainActivity)activity, Resource.Id.walk_fl, SelectFragment(), Resource.String.chamar_title, InicialFragment.TAG);
        }

        internal static void EncerraWalkthrough(MainActivity activity, bool limparPilha = true)
        {
            SetWalkThrough(false);
            Estado = eEstado.Pronto;
            if (limparPilha)
                Utils.Interface.LimparPilhaFragments(activity);
            activity.SetContentView(Resource.Layout.main);
            Utils.Interface.EmpilharFragment(activity, Resource.Id.tabFrameLayout1, new InicialFragment(), Resource.String.chamar_title, InicialFragment.TAG);
        }

        internal static void EncerraFluxoWalkthrough(MainActivity activity)
        {
            SetWalkThrough(false);
            Estado = eEstado.Pronto;
            activity.SetContentView(Resource.Layout.main);
        }

        public static bool GetWalkThrough()
        {
            return WalkThrough;
        }

        public static void SetWalkThrough(bool walkThrough)
        {
            WalkThrough = walkThrough;
        }

        public static bool GetStartInWalkThrough()
        {
            return InWalkThrough;
        }

        public static void SetStartInWalkThrough(bool inWalkThrough)
        {
            InWalkThrough = inWalkThrough;
        }       
    }
}