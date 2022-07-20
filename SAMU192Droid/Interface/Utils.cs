using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using SAMU192Droid.Interface.Activities;
using SAMU192Droid.Interface.Fragments;

namespace SAMU192Droid.Interface
{
    internal static class Utils
    {

        internal static readonly string Atualizacao = "Atualizacao";

        internal static class VersaoAndroid
        {
            public static bool QualquerLollipop { get { return (Build.VERSION.SdkInt == BuildVersionCodes.Lollipop || Build.VERSION.SdkInt == BuildVersionCodes.LollipopMr1); } }
        }

        internal static class Mensagem
        {
            public static void Erro(System.Exception ex)
            {
                if (FacadeStub.StubUtilidades.AppEmProducao())
                    FacadeStub.StubAppCenter.AppCrash(ex);
                else
                    MensagemErro(ex.ToString());
            }

            public static void Erro(Java.Lang.Exception ex)
            {
                if (FacadeStub.StubUtilidades.AppEmProducao())
                    FacadeStub.StubAppCenter.AppCrash(ex);
                else
                    MensagemErro(ex.ToString());
            }

            private static void MensagemErro(string msg)
            {
                string texto = msg;
                texto = texto != null && texto.Length > 180 ? texto.Substring(0, 180) : texto;
                Toast toast = Toast.MakeText(Application.Context, texto, ToastLength.Long);
                TextView toastMessage = (TextView)toast.View.FindViewById(Android.Resource.Id.Message);
                toastMessage.SetBackgroundColor(Color.Red);
                toastMessage.SetTextColor(Color.White);
                toast.View.SetBackgroundColor(Color.Red);
                toast.Show();
            }

            public static void Aviso(string texto)
            {
                texto = texto != null && texto.Length > 180 ? texto.Substring(0, 180) : texto;
                Toast toast = Toast.MakeText(Application.Context, texto, ToastLength.Long);
                //TextView toastMessage = (TextView)toast.View.FindViewById(Android.Resource.Id.Message);
                toast.Show();
            }

            public static void Alerta(string texto)
            {
                texto = texto != null && texto.Length > 180 ? texto.Substring(0, 180) : texto;
                Toast toast = Toast.MakeText(Application.Context, texto, ToastLength.Long);
                //TextView toastMessage = (TextView)toast.View.FindViewById(Android.Resource.Id.Message);
                //toastMessage.SetBackgroundColor(Color.Yellow);
                //toastMessage.SetTextColor(Color.Black);
                //toast.View.SetBackgroundColor(Color.Yellow);
                toast.Show();
            }

            internal static void Notificacao(string texto)
            {
                texto = texto != null && texto.Length > 180 ? texto.Substring(0, 180) : texto;
                Toast toast = Toast.MakeText(Application.Context, texto, ToastLength.Long);
                //TextView toastMessage = (TextView)toast.View.FindViewById(Android.Resource.Id.Message);
                //toastMessage.SetBackgroundColor(Color.Green);
                //toastMessage.SetTextColor(Color.White);
                //toast.View.SetBackgroundColor(Color.Green);
                toast.Show();
            }

            internal static class Dialogs
            {
                internal static Dialog Progresso(Activity activity, string texto, Action onCancel, bool cancelOnBackPress = false, bool cancelOnOutsideTouch = false)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(activity);

                    View view = activity.LayoutInflater.Inflate(Resource.Layout.progresso, null);
                    view.FindViewById<TextView>(Resource.Id.loading_msg).SetText(texto, TextView.BufferType.Normal);
                    alert.SetView(view);
                    Dialog dialog = alert.Create();
                    alert.SetCancelable(cancelOnBackPress);
                    dialog.SetCanceledOnTouchOutside(cancelOnOutsideTouch);
                    dialog.Show();
                    return dialog;
                }

                private class DialogInterfaceOnKeyListener : Java.Lang.Object, IDialogInterfaceOnKeyListener
                {
                    public bool OnKey(IDialogInterface dialog, [GeneratedEnum] Keycode keyCode, KeyEvent e)
                    {
                        if (keyCode == Keycode.Back)
                            return true;
                        return false;
                    }
                }

                internal class OnCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
                {
                    private Action Acao;
                    public OnCancelListener(Action acao)
                    {
                        Acao = acao;
                    }                    

                    public void OnCancel(IDialogInterface dialog)
                    {
                        Acao();
                    }
                }

                internal static Dialog AlertaSimples(Activity activity, string msg)
                {
                    return AlertaSimples(activity, activity.GetString(Resource.String.alerta), msg, "Ok");
                }

                internal static Dialog AlertaSimples(Activity activity, int msgID)
                {
                    return AlertaSimples(activity, activity.GetString(Resource.String.alerta), activity.GetString(msgID), "Ok");
                }

                internal static Dialog AlertaSimples(Activity activity, string msg, Action action)
                {
                    return AlertaSimples(activity, activity.GetString(Resource.String.alerta), msg, "Ok", action);
                }

                static Dialog AlertaSimples(Activity activity, string title, string msg, string cmd, Action action = null)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(activity);
                    alert.SetTitle(title);
                    alert.SetMessage(msg);
                    if (action == null)
                        alert.SetNeutralButton(cmd, (s, e) => { });
                    else
                        alert.SetNeutralButton(cmd, (s, e) => action());

                    Dialog dialog = alert.Create();
                    dialog.Show();
                    return dialog;
                }

                internal static Dialog AlertaSimNao(Activity activity, string title, string msg, string cmdYes, string cmdNo, Action actionYes, Action actionNo)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(activity);
                    alert.SetTitle(title);
                    alert.SetMessage(msg);

                    alert.SetPositiveButton(cmdYes, (s, e) => actionYes());
                    alert.SetNegativeButton(cmdNo, (s, e) => actionNo());

                    Dialog dialog = alert.Create();
                    dialog.Show();
                    return dialog;
                }             
            }
        }

        public static class Interface
        {
            internal class Cores
            {
                internal static Color GetByID(int resourceId, Context context)
                {
                    int colorId;
                    if (VersaoAndroid.QualquerLollipop)
                        colorId = Android.Support.V4.Content.ContextCompat.GetColor(context, resourceId);
                    else
                        colorId = context.GetColor(resourceId);
                    return new Color(colorId);
                }
            }

            internal class CalendarioFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
            {
                public static readonly string TAG = "X:" + typeof(CalendarioFragment).Name.ToUpper();

                Action<DateTime> _dateSelectedHandler = delegate { };

                public static CalendarioFragment NewInstance(Action<DateTime> onDateSelected)
                {
                    CalendarioFragment frag = new CalendarioFragment();
                    frag._dateSelectedHandler = onDateSelected;
                    return frag;
                }

                public override Dialog OnCreateDialog(Bundle savedInstanceState)
                {
                    DateTime currently = DateTime.Now;
                    DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                                   this,
                                                                   currently.Year,
                                                                   currently.Month - 1,
                                                                   currently.Day);
                    return dialog;
                }

                public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
                {
                    DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
                    Log.Debug(TAG, selectedDate.ToLongDateString());
                    _dateSelectedHandler(selectedDate);
                }
            }

            internal static void IniciaNovaActivity(Activity activity, Type type, int requestCode, Context context, Bundle param = null)
            {
                var intent = new Intent(context, type);
                intent.PutExtra("MainActivity_EnderecoCriadoUsuario", param);
                activity.StartActivityForResult(intent, requestCode, param);
            }

            internal static void CreatePhoneTextView2(Activity activity, LinearLayout ll, string phone)
            {
                TextView tv = new TextView(activity);
                tv.Text = phone;
                tv.TextSize = 21;
                tv.Typeface = Typeface.DefaultBold;
                tv.SetBackgroundColor(Cores.GetByID(Resource.Color.branco, activity.ApplicationContext));
                tv.SetTextColor(Cores.GetByID(Resource.Color.laranja, activity.ApplicationContext));
                ll.AddView(tv);
            }

            internal static LinearLayout CreateFavoritoLinearLayout(Activity activity, LinearLayout ll, string nome, string endereco, string textoBotao, bool selected)
            {
                LayoutInflater inflater = LayoutInflater.From(activity);
                LinearLayout llFavoritoCell = (LinearLayout)inflater.Inflate(Resource.Layout.inicial_favoritos_cell, null);
                TextView tvNome = (TextView)llFavoritoCell.FindViewById(Resource.Id.lv_favoritos_cell_radio_tv_1);
                tvNome.Text = nome;

                TextView tvEndereco = (TextView)llFavoritoCell.FindViewById(Resource.Id.lv_favoritos_cell_radio_tv_2);
                tvEndereco.Text = endereco;

                ImageView ivRadio = (ImageView)llFavoritoCell.FindViewById(Resource.Id.inicial_favoritos_cell_radio_iv);
                ivRadio.SetBackgroundResource(selected ? Android.Resource.Drawable.RadiobuttonOnBackground : Android.Resource.Drawable.RadiobuttonOffBackground);

                Button cmdSelecionar = (Button)llFavoritoCell.FindViewById(Resource.Id.inicial_favoritos_cell_radio_cmd);
                cmdSelecionar.Visibility = ViewStates.Visible;
                cmdSelecionar.Text = textoBotao;

                llFavoritoCell.Selected = selected;
                return llFavoritoCell;
            }

            private static List<int> Titles = new List<int>();
            internal static int LastTitleResourceId()
            {
                int id = Resource.String.chamar_title;
                if (Titles.Count > 0)
                    id = Titles[Titles.Count - 1];
                return id;
            }

            internal static void EmpilharFragment(Activities.MainActivity activity, int resourceId, Fragment fragment, int resourceIdForTitle, string tag, Bundle bundle = null)
            {
                if (FragmentTopoTag(activity) == tag && tag.ToUpper() != "INICIAL")
                    return;

                fragment.Arguments = bundle;
                var transaction = activity.FragmentManager.BeginTransaction();
                transaction.Add(resourceId, fragment, tag);
                transaction.AddToBackStack(tag);
                transaction.CommitAllowingStateLoss();
                activity.SupportActionBar.SetDisplayShowTitleEnabled(true);
                activity.SupportActionBar?.SetTitle(resourceIdForTitle);
                Titles.Add(resourceIdForTitle);
            }

            internal static void SubstituirFragment(Activities.MainActivity activity, int resourceId, Fragment fragment, int resourceIdForTitle, string tag, Bundle bundle = null)
            {
                Fragment oldFrag = FragmentTopo(activity);

                fragment.Arguments = bundle;
                var transaction = activity.FragmentManager.BeginTransaction();
                transaction.Replace(resourceId, fragment, tag);
                activity.FragmentManager.PopBackStack();
                transaction.AddToBackStack(tag);
                transaction.CommitAllowingStateLoss();
                activity.SupportActionBar?.SetTitle(resourceIdForTitle);
                if (Titles.Count > 0)
                    Titles.RemoveAt(Titles.Count - 1);
                Titles.Add(resourceIdForTitle);
                oldFrag.Dispose();
                oldFrag = null;
                GC.Collect();

            }

            internal static void RetirarFragment(Activity activity, int? resourceIdForNewTitle = null)
            {
                var transaction = activity.FragmentManager.BeginTransaction();
                transaction.Remove(FragmentTopo(activity));
                activity.FragmentManager.PopBackStackImmediate();
                transaction.CommitAllowingStateLoss();
                if (resourceIdForNewTitle.HasValue)
                    activity.SetTitle(resourceIdForNewTitle.Value);
                if (Titles.Count > 0)
                    Titles.RemoveAt(Titles.Count - 1);
            }

            internal static void LimparPilhaFragments(Activity activity)
            {
                var transaction = activity.FragmentManager.BeginTransaction();
                for (int i = 1; i <= activity.FragmentManager.BackStackEntryCount; i++)
                {
                    transaction.Remove(FragmentTopo(activity));
                    activity.FragmentManager.PopBackStackImmediate();
                }
                transaction.CommitAllowingStateLoss();
                Titles.Clear();
            }

            internal static void LimparPilhaFragments2(Activity activity)
            {
                var transaction = activity.FragmentManager.BeginTransaction();
                for (int i = 1; i < activity.FragmentManager.BackStackEntryCount; i++)
                {
                    transaction.Remove(FragmentTopo(activity));
                    activity.FragmentManager.PopBackStackImmediate();
                    Titles.RemoveAt(i-1);
                }
                transaction.CommitAllowingStateLoss();
            }

            internal static void LimparPilhaFragments3(Activity activity)
            {
                var transaction = activity.FragmentManager.BeginTransaction();
                while(activity.FragmentManager.BackStackEntryCount> 0)
                {
                    transaction.Remove(FragmentTopo(activity));
                    activity.FragmentManager.PopBackStackImmediate();
                }
                Titles.Clear();
                transaction.CommitAllowingStateLoss();
            }


            internal static string FragmentTopoTag(Activity activity)
            {
                if (activity.FragmentManager.BackStackEntryCount > 0)
                {
                    var fragment = activity.FragmentManager.GetBackStackEntryAt(activity.FragmentManager.BackStackEntryCount - 1);
                    if (fragment != null)
                        return fragment.Name;
                }
                return "";
            }

            internal static Fragment FragmentTopo(Activity activity)
            {
                if (activity.FragmentManager.BackStackEntryCount > 0)
                {
                    var bse = activity.FragmentManager.GetBackStackEntryAt(activity.FragmentManager.BackStackEntryCount - 1);
                    if (bse != null)
                    {
                        Fragment f = activity.FragmentManager.FindFragmentByTag(bse.Name);
                        if (f == null)
                            f = activity.FragmentManager.FindFragmentById(bse.Id);
                        return f;
                    }
                }
                return null;
            }

            internal static void ScrollUp(Activity activity)
            {
                ScrollView sv = activity.FindViewById<ScrollView>(Resource.Id.walk_sv);
                sv.PageScroll(FocusSearchDirection.Up);
                sv.PageScroll(FocusSearchDirection.Up);
                sv.PageScroll(FocusSearchDirection.Up);
            }

            internal static void FechaTeclado(Activity activityAux)
            {
                if (activityAux.CurrentFocus != null)
                {
                    InputMethodManager inputMethodManager = (InputMethodManager)activityAux.GetSystemService(Android.Content.Context.InputMethodService);
                    inputMethodManager.HideSoftInputFromWindow(activityAux.CurrentFocus.WindowToken, 0);
                }
            }

        }

        internal class Mascara : Java.Lang.Object, ITextWatcher
        {
            private readonly EditText _editText;
            private readonly string _mask;
            bool isUpdating;
            string old = "";

            internal Mascara(EditText editText, string mask)
            {
                _editText = editText;
                _mask = mask;
            }

            internal static string Unmask(string s)
            {
                return s.Replace(".", "").Replace("-", "")
                    .Replace("/", "").Replace("(", "")
                    .Replace(")", "").Replace(" ", "");
            }

            public void AfterTextChanged(IEditable s)
            {
            }

            public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
            {
            }

            public void OnTextChanged(ICharSequence s, int start, int before, int count)
            {
                string str = Unmask(s.ToString());
                string mascara = "";

                if (isUpdating)
                {
                    old = str;
                    isUpdating = false;
                    return;
                }

                int i = 0;

                foreach (var m in _mask.ToCharArray())
                {
                    if (m != '#' && str.Length > old.Length)
                    {
                        mascara += m;
                        continue;
                    }
                    try
                    {
                        mascara += str[i];
                    }
                    catch (System.Exception ex)
                    {
                        break;
                    }
                    i++;
                }

                isUpdating = true;

                _editText.Text = mascara;

                _editText.SetSelection(mascara.Length);
            }
        }

        internal static class Ferramentas
        {
            internal static Bitmap ResizeBitmap(Bitmap original, int limit)
            {
                double ratio = System.Math.Max(original.Width, original.Height) / (double)limit;
                if (ratio > 1)
                    return Bitmap.CreateScaledBitmap(original, (int)(original.Width / ratio), (int)(original.Height / ratio), false);
                else
                    return original;
            }

            internal static void EnviaBroadcast(int tipo, int codChamado, bool fromBackground)
            {
                Intent intent = new Intent(Utils.Atualizacao);
                intent.PutExtra("tipo", tipo);
                intent.PutExtra("codChamado", codChamado);
                intent.PutExtra("fromBackground", fromBackground);
                LocalBroadcastManager.GetInstance(Application.Context).SendBroadcast(intent);
            }
        }
    }
}