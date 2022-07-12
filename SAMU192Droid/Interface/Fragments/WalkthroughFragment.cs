using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using SAMU192Core.DTO;
using SAMU192Core.Utils;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Interface.Activities;

namespace SAMU192Droid.Interface.Fragments
{
    public class WalkthroughFragment : BaseFragment
    {
        #region Campos de tela
        protected View view;
        protected Button cmdNext;
        #endregion
        int currentLayout;
        public string TAG = "WalkthroughFragment";
        Activity activityAux;
        bool podePedirAutorizacaoGPS = true, pedidoAutorizacaoGPSRealizado = false, gpsPermitido = true;
        Dialog ativeGPS;

        public override void HideDescendantsForAccessibility()
        {
            cmdNext.Visibility = ViewStates.Gone;
        }

        public override void ShowDescendantsForAccessibility()
        {
            cmdNext.Visibility = ViewStates.Visible;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                activityAux = this.Activity;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                if (StubWalkThrough.GetWalkThrough())
                {
                    currentLayout = SelectFragmentLayout();
                    view = inflater.Inflate(currentLayout, null);

                    LoadScreenControls();

                    Utils.Interface.ScrollUp(Activity);
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        private void LoadScreenControls()
        {
            cmdNext = view.FindViewById<Button>(Resource.Id.walknext_cmd);
            cmdNext.Click += cmdNext_Click;

            LoadImageViews();

            TextView tv = view.FindViewById<TextView>(Resource.Id.walk_fechar_tv);
            if (tv != null)
            {
                tv.Clickable = true;
                tv.Click += (s, e) =>
                {
                    try
                    {
                        StubWalkThrough.EncerraWalkthrough((MainActivity)activityAux, false);
                        StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Fechado.Value, this.TAG);
                    }
                    catch (Exception ex)
                    {
                        Utils.Mensagem.Erro(ex);
                    }
                };
            }
        }

        private void LoadImageViews()
        {
            if (StubWalkThrough.CurrentWalkThroughFragment > 0)
            {
                List<ImageView> imagens = new List<ImageView>();
                for (int i = 1; i < 7; i++)
                    imagens.Add(LoadImageView(i));

                imagens[(int)StubWalkThrough.CurrentWalkThroughFragment - 1].SetBackgroundResource(Resource.Drawable.walk_page_current);
            }
        }

        private ImageView LoadImageView(int index)
        {
            ImageView image = view.FindViewById<ImageView>(StubWalkThrough.SelectFragmentImageView(index));
            if (image != null)
            {
                image.SetBackgroundResource(Resource.Drawable.walk_page_other);
                image.Clickable = true;
                image.Click += (s, e) => TrocaFragment(index); 
            }
            return image;
        }

        private void TrocaFragment(int index)
        {
            if (StubWalkThrough.CurrentWalkThroughFragment != (StubWalkThrough.eWalkThrough)index)
                StubWalkThrough.DirecionaWalkthrough((MainActivity)Activity,(StubWalkThrough.eWalkThrough)index);
        }

        public static int SelectFragmentLayout()
        {
            switch (StubWalkThrough.CurrentWalkThroughFragment)
            {
                case StubWalkThrough.eWalkThrough.Termo:
                    return Resource.Layout.walkthrough1;
                case StubWalkThrough.eWalkThrough.AtiveGps:
                    return Resource.Layout.walkthrough2;
                case StubWalkThrough.eWalkThrough.Cadastro:
                    return Resource.Layout.walkthrough3;
                case StubWalkThrough.eWalkThrough.Favoritos:
                    return Resource.Layout.walkthrough4;
                case StubWalkThrough.eWalkThrough.Ligacao:
                    return Resource.Layout.walkthrough5;
                case StubWalkThrough.eWalkThrough.Regiao:
                    return Resource.Layout.walkthrough6;
                case StubWalkThrough.eWalkThrough.Sabedoria:
                    return Resource.Layout.walkthrough7;
                default:
                    return Resource.Layout.walkthrough1;
            }
        }

        public void voltaFragmento()
        {
            try
            {
                this.HideDescendantsForAccessibility();
                StubWalkThrough.RetornaWalkthrough(this.Activity);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            try
            {
                int resourceId = StubWalkThrough.SelectFragmentLayout();
                this.HideDescendantsForAccessibility();
                switch (resourceId)
                {
                    case Resource.Layout.walkthrough1:
                        AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                        alert.SetTitle("TERMOS E CONDIÇÕES GERAIS DE USO DO APLICATIVO");
                        alert.SetMessage(Activity.GetString(Resource.String.termo));
                        alert.SetNegativeButton("Recusar", (senderAlert, args) =>
                        {
                            try
                            {
                                return;
                            }
                            catch (Exception ex)
                            {
                                Utils.Mensagem.Erro(ex);
                            }
                        });
                        alert.SetPositiveButton("Aceitar", (senderAlert, args) =>
                        {
                            try
                            {
                                StubCadastro.SalvaAceiteTermo(new TermoDTO(true));
                                StubWalkThrough.ProssegueWalkthrough(this.Activity);
                                StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Termo_Aceite.Value);
                            }
                            catch (Exception ex)
                            {
                                Utils.Mensagem.Erro(ex);
                            }
                        });
                        alert.SetOnCancelListener(new Utils.Mensagem.Dialogs.OnCancelListener(()=> {
                            try
                            {
                                this.ShowDescendantsForAccessibility();
                            }
                            catch (Exception ex)
                            {
                                Utils.Mensagem.Erro(ex);
                            }
                        }));
                        Dialog dialog = alert.Create();
                        dialog.Show();
                        break;

                    case Resource.Layout.walkthrough2:
                        gpsPermitido = true;

                        string[] PermissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
                        foreach (string permission in PermissionsLocation)
                        {
                            if (Utils.VersaoAndroid.QualquerLollipop)
                                gpsPermitido &= Android.Support.V4.Content.ContextCompat.CheckSelfPermission(Activity.ApplicationContext, permission) == Permission.Granted;
                            else
                                gpsPermitido &= Activity.CheckSelfPermission(permission) == Permission.Granted;
                        }

                        var teste = gpsPermitido;
                        if (!gpsPermitido && podePedirAutorizacaoGPS)
                        {
                            podePedirAutorizacaoGPS = false;
                            pedidoAutorizacaoGPSRealizado = true;
                            Activity.RequestPermissions(PermissionsLocation, (int)Enums.RequestPermissionCode.GPS);
                        }

                        if (gpsPermitido)
                            StubGPS.Carrega(this.Activity, null, null);

                        StubWalkThrough.ProssegueWalkthrough(this.Activity);
                        break;

                    case Resource.Layout.walkthrough3:
                        StubCadastro.Carrega();
                        StubWalkThrough.ProssegueWalkthrough(this.Activity);
                        break;

                    case Resource.Layout.walkthrough5:
                        StubTelefonia.Carrega(null,null, this.Activity.GetSystemService("phone"));
                        StubTelefonia.VerificarPermissao(this.Activity);
                        StubWalkThrough.ProssegueWalkthrough(this.Activity);
                        break;

                    case Resource.Layout.walkthrough7:
                        StubWalkThrough.EncerraWalkthrough((MainActivity)this.Activity);
                        StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Completado.Value);
                        break;

                    default:
                        StubWalkThrough.ProssegueWalkthrough(this.Activity);
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void GPSStatus(bool ativo)
        {
            if (ativo)
            {
                if (ativeGPS != null)
                {
                    ativeGPS.Dismiss();
                    ativeGPS = null;
                }
            }
            else
            {
                if (ativeGPS == null || !ativeGPS.IsShowing)
                {
                    //llEndereco InProgress.Visibility = ViewStates.Gone;
                }
            }
        }
    }
}