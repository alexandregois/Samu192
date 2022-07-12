using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using SAMU192Core.DTO;
using SAMU192Droid.FacadeStub;

namespace SAMU192Droid.Interface.Activities
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/MyTheme.Splash")]
    internal class SplashActivity : BaseActivity
    {
        bool alreadyPassToOpenWalkThrough = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.splash);
                Inicializa();
            }
            catch(Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private async void Inicializa()
        {
            var task = new Task(() =>
            {
                Task.Delay(3000);
                var termo = StubCadastro.RecuperaAceiteTermo();
                StubWalkThrough.SetWalkThrough(false);
                StubWalkThrough.SetStartInWalkThrough(false);
                if (termo.Aceite)
                {
                    CadastroDTO cadastro = StubCadastro.RecuperaCadastro();
                    if (!StubCadastro.ExisteCadastro(cadastro) && !alreadyPassToOpenWalkThrough)
                    {
                        alreadyPassToOpenWalkThrough = true;
                        StubWalkThrough.CurrentWalkThroughFragment = StubWalkThrough.eWalkThrough.AtiveGps;
                        StubWalkThrough.SetWalkThrough(true);
                        StubWalkThrough.SetStartInWalkThrough(true);
                    }
                }
                else
                {
                    StubWalkThrough.SetWalkThrough(true);
                    StubWalkThrough.SetStartInWalkThrough(true);
                }

                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            });
            task.Start();
        }
    }
}