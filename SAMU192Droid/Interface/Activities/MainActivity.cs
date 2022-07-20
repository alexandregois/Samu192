using System;
using Android.OS;
using Android.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Views;
using Firebase.Iid;
using SAMU192Droid.Interface.Fragments;
using SAMU192Core.DTO;
using SAMU192Droid.FacadeStub;
using Android.Content;
using SAMU192Droid.Servicos;

namespace SAMU192Droid.Interface.Activities
{
    [Activity(Label = "CHAMAR 192", Icon = "@drawable/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, MainLauncher = false, 
        Exported = true,
     //Configurações para suprimir evento OnCreate quando telefone é rotacionado
     ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    internal class MainActivity : BaseActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        EnderecoDTO enderecoCriadoUsuario;
        const string TAG = "MainActivity";
        public static bool IsActive = true;

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        internal EnderecoDTO EnderecoCriadoUsuario {
            get => enderecoCriadoUsuario;
            set => enderecoCriadoUsuario = value;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                StubAppCenter.Inicializa();

                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.main);


                SetAlarmForBackgroundServices(this);


                var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                if (toolbar != null)
                {
                    SetSupportActionBar(toolbar);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                    SupportActionBar.SetHomeButtonEnabled(false);
                }

                drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
                drawerLayout.AddDrawerListener(drawerToggle);
                drawerToggle.SyncState();

                navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
                navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
                navigationView.Menu.SetGroupCheckable(0, false, true);

                // verifica se Google Play Services está disponível
                StubPushNotifications.ServicoDisponivel(this);
                
                if (string.IsNullOrEmpty(StubUtilidades.RecuperaInstanceID()))
                    StubUtilidades.SalvaInstanceID(FirebaseInstanceId.Instance.Id);

                //aplicativos em execução no Android 8.0 (nível de API 26) ou superior devem criar um canal de notificação para publicar suas notificações
                CreateNotificationChannel();

                StubWalkThrough.SelectFragmentOnMainActivity(this);
                
                if (StubWalkThrough.GetWalkThrough())
                {
                    SetContentView(Resource.Layout.walkthrough);
                    WalkthroughFragment fragment = StubWalkThrough.SelectFragment();
                    Utils.Interface.EmpilharFragment(this, Resource.Id.walk_fl, fragment, Resource.String.walkthrough_title, fragment.TAG);
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications SAMU 192",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            try
            {
                StubUtilidades.SetFromMenu(true);
                if (StubWalkThrough.Estado == StubWalkThrough.eEstado.Pronto)
                {
                    if (!ValidaSelecaoMenu(e.MenuItem.ItemId))
                    {
                        drawerLayout.CloseDrawer((int)GravityFlags.Left, true);
                        return;
                    }

                    StubGPS.StopLocationManager();
                    StubWalkThrough.LastSelectedFragmentMainActivity.HideDescendantsForAccessibility();
                    Utils.Interface.LimparPilhaFragments2(this);

                    if (e.MenuItem.ItemId == Resource.Id.nav_chamar_samu)
                        Utils.Interface.EmpilharFragment(this, Resource.Id.tabFrameLayout1, new InicialFragment(), Resource.String.chamar_title, InicialFragment.TAG);
                    if (e.MenuItem.ItemId == Resource.Id.nav_cadastro)
                        Utils.Interface.EmpilharFragment(this, Resource.Id.tabFrameLayout1, new CadastroFragment(), Resource.String.cadastro_title, CadastroFragment.TAG);
                    if (e.MenuItem.ItemId == Resource.Id.nav_favoritos)
                        Utils.Interface.EmpilharFragment(this, Resource.Id.tabFrameLayout1, new FavoritosFragment(), Resource.String.favoritos_title, FavoritosFragment.TAG);
                    if (e.MenuItem.ItemId == Resource.Id.nav_quando_acionar_samu)
                        Utils.Interface.EmpilharFragment(this, Resource.Id.tabFrameLayout1, new QuandoFragment(), Resource.String.quando_title, QuandoFragment.TAG);
                    if (e.MenuItem.ItemId == Resource.Id.nav_sobre)
                        Utils.Interface.EmpilharFragment(this, Resource.Id.tabFrameLayout1, new SobreFragment(), Resource.String.sobre_title, SobreFragment.TAG);
                }
                drawerLayout.CloseDrawer((int)GravityFlags.Left, true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private bool ValidaSelecaoMenu(int menuItemId)
        {
            if (menuItemId == Resource.Id.nav_chamar_samu && Utils.Interface.FragmentTopo(this) is InicialFragment)
                return false;
            else
            if (menuItemId == Resource.Id.nav_cadastro && Utils.Interface.FragmentTopo(this) is CadastroFragment)
                return false;
            else
            if (menuItemId == Resource.Id.nav_cadastro && Utils.Interface.FragmentTopo(this) is Cadastro2Fragment)
                return false;
            else
            if (menuItemId == Resource.Id.nav_favoritos && Utils.Interface.FragmentTopo(this) is FavoritosFragment)
                return false;
            else
            if (menuItemId == Resource.Id.nav_favoritos && Utils.Interface.FragmentTopo(this) is FavoritosNomeFragment)
                return false;
            else
            if (menuItemId == Resource.Id.nav_quando_acionar_samu && Utils.Interface.FragmentTopo(this) is QuandoFragment)
                return false;
            else
            if (menuItemId == Resource.Id.nav_sobre && Utils.Interface.FragmentTopo(this) is SobreFragment)
                return false;

            return true;
        }

        public static void SetAlarmForBackgroundServices(Context context)
        {

            var alarmIntent = new Intent(context.ApplicationContext, typeof(AlarmReceiver));
            var broadcast = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.NoCreate);
            if (broadcast == null)
            {
                var pendingIntent = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, 0);
                var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), 15000, pendingIntent);
            }
        }

    }    
}