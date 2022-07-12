using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V7.App;
using Android.OS;
using Android.Runtime;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Interface.Fragments;
using SAMU192Core.Utils;

namespace SAMU192Droid.Interface.Activities
{
    internal class BaseActivity : AppCompatActivity
    {
        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = new Exception("CurrentDomainOnUnhandledException", e.ExceptionObject as Exception);
            Utils.Mensagem.Erro(ex);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
                StubUtilidades.SetCultureInfo();
                StubConexao.Carrega();
                StubCadastro.Carrega();
                StubLocalizacao.Carrega(Assets);
                StubMapa.Carrega(null);
            }
            catch(Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                if (permissions.Length > 0)
                {
                    switch (requestCode)
                    {
                        case 0: //GPS
                            if (grantResults[0] == Permission.Granted)
                                StubAppCenter.AppAnalytic(Enums.AnalyticsType.GPS_Aceito_Permissao.Value);
                            else
                                StubAppCenter.AppAnalytic(Enums.AnalyticsType.GPS_Negado_Permissao.Value);
                            break;
                        case 1: //Telefone
                            if (grantResults[0] == Permission.Granted)
                            {
                                if (StubWalkThrough.GetWalkThrough())
                                    StubTelefonia.Descarrega();
                                else
                                    StubTelefonia.FazerLigacao(this);

                                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Telefonia_Aceito_Permissao.Value);
                            }
                            else
                            {
                                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Telefonia_Negado_Permissao.Value);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                switch (requestCode)
                {
                    case 9900:
                        StubGPS.Carrega(this, null, null);
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

        public override void OnBackPressed()
        {
            try
            {
                if (StubUtilidades.GetLockBackButton())
                {
                    ReexibirFragmentTopo();
                    return;
                }
                
                //Último fragment não deve ser removido (evitar tela em branco)
                if (FragmentManager.BackStackEntryCount > 1)
                {
                    Utils.Interface.RetirarFragment(this);
                    if (FragmentManager.BackStackEntryCount == 1)
                    {
                        //Veio do Menu
                        if (StubUtilidades.GetFromMenu())
                        {
                            //Deve ser obrigatoriamente o Fragment Incial "Chamar" a base final do voltar
                            RedirecionaParaInicio();
                            return;
                        }
                    }

                    ReexibirFragmentTopo(true);
                }
                else 
                {
                    Fragment topo = Utils.Interface.FragmentTopo(this);
                    if (topo is WalkthroughFragment)
                    {
                        ((WalkthroughFragment)topo).voltaFragmento();
                    }
                    else
                    {
                        SairFavoritosRecriarActivity();
                        return;
                    }
                }

                this.SupportActionBar?.SetTitle(Utils.Interface.LastTitleResourceId());
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SairFavoritosRecriarActivity()
        {
            Fragment frag = Utils.Interface.FragmentTopo(this);
            if (frag is FavoritosFragment)
            {
                Utils.Interface.RetirarFragment(this);
                this.Recreate();
                return;
            }
        }

        private void RedirecionaParaInicio()
        {
            Utils.Interface.RetirarFragment(this);
            StubWalkThrough.LastSelectedFragmentMainActivity = new InicialFragment();
            Utils.Interface.EmpilharFragment((MainActivity)this, Resource.Id.tabFrameLayout1, StubWalkThrough.LastSelectedFragmentMainActivity, Resource.String.chamar_title, InicialFragment.TAG);
            this.SupportActionBar?.SetTitle(Resource.String.chamar_title);
        }

        private void ReexibirFragmentTopo(bool fluxoVoltar = false)
        {
            Fragment frag = Utils.Interface.FragmentTopo(this);
            if (frag is BaseFragment)
            {
                ((BaseFragment)frag).ShowDescendantsForAccessibility();

                if (fluxoVoltar)
                {
                    //Volta contador do enum do Walkthrough
                    if (frag is BaseWalkthroughFragment)
                        StubWalkThrough.VoltaContadorWalkthought();

                    //Se já passou pelo Termo e voltar... pára o voltar na tela "Ative GPS"
                    if (frag is Walkthrough2Fragment)
                        StubUtilidades.SetLockBackButton(true);

                    //Se já iniciou um Cadastro, deve trancar a volta até a 1ª tela de "Cadastro"
                    if (frag is CadastroFragment)
                        if (!StubUtilidades.GetFromMenu())
                            StubUtilidades.SetLockBackButton(true);
                }
            }
        }
    }
}