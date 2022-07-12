using CoreGraphics;
using Foundation;
using MapKit;
using System;
using System.Drawing;
using System.Threading.Tasks;
using UIKit;

namespace SAMU192iOS.ViewControllers
{

    public interface INetworkConnection
    {
        bool IsConnected { get; }
        bool CheckNetworkConnection();
    }

    internal static class Utils
    {
        internal static class Colors
        {
            internal static UIColor Vermelho = UIColor.FromRGB(255, 0, 0);
            internal static UIColor VermelhoEscuro = UIColor.FromRGB(165, 17, 24);
            internal static UIColor Verde = UIColor.FromRGB(0, 255, 0);
            internal static UIColor Laranja = UIColor.FromRGB(244, 121, 32);
            internal static UIColor LaranjaEscuro = UIColor.FromRGB(159, 72, 9);
            internal static UIColor LaranjaClaro = UIColor.FromRGB(224, 110, 45);
            internal static UIColor AmareloEstrelas = UIColor.FromRGB(224, 121, 32);
            internal static UIColor Branco = UIColor.FromRGB(255, 255, 255);
            internal static UIColor Preto = UIColor.FromRGB(0, 0, 0);
            internal static UIColor Azul = UIColor.FromRGB(0, 255, 0);
            internal static UIColor Amarelo = UIColor.FromRGB(255, 242, 0);
            internal static UIColor Cinza = UIColor.FromRGB(219, 219, 219);
            internal static UIColor CinzaEscuro = UIColor.FromRGB(204, 204, 204);
            internal static UIColor CinzaBlack = UIColor.FromRGB(155, 155, 155);
            internal static UIColor CinzaClaro = UIColor.FromRGB(248, 248, 248);
        }
        internal static class Mascara
        {
            internal static string Unmask(string s)
            {
                return s.Replace(".", "").Replace("-", "")
                    .Replace("/", "").Replace("(", "")
                    .Replace(")", "").Replace(" ", "");
            }

            internal static string Mask(string s)
            {
                if (s.StartsWith("0"))
                    return string.Format("({0}) {1}.{2}", s.Substring(0, 3), s.Substring(3, 5), s.Substring(8, 4));
                else
                    return string.Format("({0}) {1}.{2}", s.Substring(0, 2), s.Substring(2, 5), s.Substring(7, 4));
            }
        }

        internal static class Mensagem
        {
            public delegate void AfterConfirm(object param);
            public delegate void AfterResponse(UINavigationController navController);

            public static void Erro(Exception ex)
            {
                if (FacadeStub.StubUtilidades.AppEmProducao())
                    FacadeStub.StubAppCenter.AppCrash(ex);
                else
                    MensagemErro(ex.ToString());
            }

            public static void Erro(NSErrorException ex)
            {
                if (FacadeStub.StubUtilidades.AppEmProducao())
                    FacadeStub.StubAppCenter.AppCrash(ex);
                else
                    MensagemErro(ex.ToString());
            }

            private static void MensagemErro(string texto)
            {
                var alert = new UIAlertView
                {
                    Title = "Erro",
                    Message = texto,
                    TintColor = new UIColor(255, 0, 0, 0)//vermelho
                };
                alert.AddButton("OK");
                alert.Show();
            }

            internal static UIAlertView Alerta(string texto, Action<object> AfterConfirm_Event, object param = null)
            {
                var alert = new UIAlertView
                {
                    Title = "Alerta",
                    Message = texto,
                    TintColor = new UIColor(255, 195, 0, 0) //amarelo
                };
                alert.AddButton("OK");
                alert.Clicked += (sender, buttonArgs) => {
                    AfterConfirm_Event(param);
                };
                alert.Show();
                return alert;
            }

            internal static void Aviso(string texto)
            {
                var alert = new UIAlertView
                {
                    Title = "Alerta",
                    Message = texto,
                    TintColor = new UIColor(255, 255, 255, 0) //branco
                };
                alert.AddButton("OK");
                alert.Show();
            }

            internal static void Notificacao(string texto)
            {
                var alert = new UIAlertView
                {
                    Title = "Alerta",
                    Message = texto,
                    TintColor = new UIColor(0, 195, 0, 0), //verde
                };
                alert.AddButton("OK");
                alert.Show();
            }

            internal static Task<int> Questao(string titulo, string texto, AfterResponse afterResponse_Event, UINavigationController navController, params string[] textoBotoes)
            {
                var tcs = new TaskCompletionSource<int>();
                var alert = new UIAlertView
                {
                    Title = titulo,
                    Message = texto,
                    TintColor = new UIColor(248, 248, 248, 0), //cinza
                };
                foreach (string s in textoBotoes)
                {
                    alert.AddButton(s);
                }
                alert.Clicked += (s, e) =>
                {
                    tcs.TrySetResult((int)e.ButtonIndex);
                };
                alert.WillDismiss += (s, e) =>
                {
                    if ((int)e.ButtonIndex == 1)
                        afterResponse_Event(navController);
                };
                alert.Show();
                return tcs.Task;
            }
        }

        internal static class Touch
        {
            internal static void ConfiguraEventosTouchDaTela(UIView view, UITouch touch, Action DeslizouPraDireita_Event, Action DeslizouPraEsquerda_Event, Action DeslizouPraCima_Event, Action DeslizouPraBaixo_Event)
            {
                if (touch != null)
                {
                    var location = touch.LocationInView(view);
                    var prevLocation = touch.PreviousLocationInView(view);
                    nfloat difX = location.X - prevLocation.X;

                    if (difX > 0)
                    {
                        //foi para direita
                        if (difX > 25)
                        {
                            DeslizouPraDireita_Event();
                            return;
                        }
                    }
                    else
                    {
                        //foi para esquerda
                        if (difX < -25)
                        {
                            DeslizouPraEsquerda_Event();
                            return;
                        }
                    }

                    nfloat difY = location.Y - prevLocation.Y;
                    if (difY > 0)
                    {
                        //foi para cima
                        if (difY > 25)
                        {
                            DeslizouPraCima_Event();
                            return;
                        }
                    }
                    else
                    {
                        //foi para baixo
                        if (difY < -25)
                        {
                            DeslizouPraBaixo_Event();
                            return;
                        }
                    }
                }
            }
        }

        internal static class Interface
        {
            const string OCULTAR_TECLADO = "▼";

            internal static UIView ConfiguraView(UIView view, float height)
            {
                float fNovaLargura = (float)view.Frame.Width;
                float fNovaAltura = (float)(height);

                view.Frame = new RectangleF((float)view.Frame.X, (float)view.Frame.Y, fNovaLargura, fNovaAltura);
                return view;
            }

            static internal UITableViewCell ConfiguraTableViewCell(UITableViewCell view, float height)
            {
                float fNovaLargura = (float)view.Frame.Width;
                float fNovaAltura = (float)(height);

                view.Frame = new RectangleF((float)view.Frame.X, (float)view.Frame.Y, fNovaLargura, fNovaAltura);

                return view;
            }

            internal static UIScrollView ConfiguraScrollView(UIScrollView scrollviewOne, UIView view, float height)
            {
                UIScrollView ScrollView = scrollviewOne;

                scrollviewOne.ScrollEnabled = true;
                scrollviewOne.ShowsVerticalScrollIndicator = true;
                scrollviewOne.UserInteractionEnabled = true;
                scrollviewOne.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

                float fNovaLargura = (float)view.Frame.Width;
                float fNovaAltura = (float)(height);
                scrollviewOne.Frame = new RectangleF((float)scrollviewOne.Frame.X, (float)scrollviewOne.Frame.Y, fNovaLargura, fNovaAltura);
                scrollviewOne.ContentSize = new CGSize(scrollviewOne.Frame.Width, fNovaAltura);
                return scrollviewOne;
            }

            internal static UINavigationController GetNavigationController()
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                UINavigationController navController = ((RootViewController)window.RootViewController).NavController;
                return navController;
            }

            internal static UINavigationController GetNavigationController(UIViewController currentView)
            {
                UINavigationController navController;
                if (currentView.ParentViewController is UINavigationController)
                {
                    navController = currentView.ParentViewController as UINavigationController;
                }
                else
                {
                    if (currentView.NavigationController != null)
                        navController = currentView.NavigationController;
                    else
                    {
                        navController = GetNavigationController(currentView.ParentViewController);
                    }
                }
                return navController;
            }

            internal static void VoltarViewController(bool root, bool animated, UIViewController currentView)
            {
                var navController = GetNavigationController(currentView);
                if (navController != null)
                {
                    if (root)
                        navController.PopToRootViewController(animated);
                    else
                        navController.PopViewController(animated);
                }
            }
            internal static void VoltarViewController(bool root, bool animated, UINavigationController navController)
            {
                if (navController != null)
                {
                    if (root)
                        navController.PopToRootViewController(animated);
                    else
                        navController.PopViewController(animated);
                }
            }

            internal static MKMapView ConfiguraMKMapView(MKMapView oMapa, UIView view, EventHandler<MKMapViewChangeEventArgs> OMapa_RegionChanged)
            {
                oMapa.ShowsUserLocation = true;
                oMapa.ZoomEnabled = true;
                oMapa.ScrollEnabled = true;
                oMapa.UserInteractionEnabled = true;
                oMapa.UserLocation.Title = "Minha Localização";
                oMapa.UserLocation.Subtitle = "Onde você está";
                oMapa.RotateEnabled = false;
                oMapa.RegionChanged += OMapa_RegionChanged;

                //Pino centralizador
                MKPointAnnotation centro = new MKPointAnnotation();
                UIImageView image = new UIImageView(new MKPinAnnotationView(centro, "centro").Image);
                //Os valores -8 e -10 abaixo são offset da base do pino na imagem
                image.Center = new CGPoint(oMapa.Center.X + 8, (view.Frame.Height / 2) + image.Image.Size.Height + 20);
                view.Add(image);

                return oMapa;
            }

            /// <summary>
            /// usado para rotação do telefone (retrato/paisagem)
            /// </summary>
            /// <param name="view"></param>
            /// <param name="newSize"></param>
            internal static void ReposicionaPino(UIView view, CGSize newSize)
            {
                //base.ViewWillTransitionToSize(toSize, coordinator);
                //Utils.Interface.ReposicionaPino(this.View, toSize);

                var image = view.Subviews[8] as UIImageView;

                nfloat y = 0, x = (newSize.Width / 2) + 8;
                if (newSize.Height > newSize.Width)
                    y = (newSize.Height / 2) + image.Image.Size.Height + 20;
                else
                    y = (newSize.Height / 2) + image.Image.Size.Height - 10;
                
                image.RemoveFromSuperview();
                image.Center = new CGPoint(x, y);
                view.Add(image);
            }

            internal static void ConfiguraTextField(UITextField ptextField, UIScrollView scrollView, int plength, nfloat height, string hint = "")
            {
                float previousYoffset = (float)scrollView.ContentOffset.Y;
                ptextField.EditingDidBegin += (object sender, EventArgs e) =>
                {
                    previousYoffset = (float)scrollView.ContentOffset.Y;
                    scrollView.SetContentOffset(new PointF((float)0, (float)ptextField.Frame.Y - 70), true);
                    if (ptextField.Text == hint)
                    {
                        ptextField.Text = string.Empty;
                    }
                    ptextField.TextColor = UIColor.Black;
                };

                ptextField.TextColor = UIColor.Gray;

                var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

                ptextField.ShouldChangeCharacters = (UITextField textField, NSRange range, string replacementString) =>
                {
                    // Calculate new length
                    var length = textField.Text.Length - range.Length + replacementString.Length;
                    return length <= plength;
                };

                UIToolbar oToolbarTelefone = new UIToolbar();
                oToolbarTelefone.BarStyle = UIBarStyle.Default;
                oToolbarTelefone.Translucent = false;
                oToolbarTelefone.BackgroundColor = Colors.Laranja;
                oToolbarTelefone.SizeToFit();

                UIBarButtonItem doneButtonTelefone = new UIBarButtonItem(OCULTAR_TECLADO, UIBarButtonItemStyle.Done, (s, e) =>
                {
                    scrollView.SetContentOffset(new PointF((float)scrollView.ContentOffset.X, previousYoffset), true); //(float)scrollView.ContentOffset.Y
                    if (ptextField.Text == string.Empty || ptextField.Text == hint)
                    {
                        ptextField.Text = hint;
                        ptextField.TextColor = UIColor.LightGray;
                    }
                    else
                    {
                        ptextField.TextColor = UIColor.Black;
                    }
                    ptextField.ResignFirstResponder();
                });
                doneButtonTelefone.AccessibilityLabel = "Ocultar Teclado";
                oToolbarTelefone.SetItems(new[] { flexibleSpace, doneButtonTelefone }, true);

                ptextField.InputAccessoryView = oToolbarTelefone;

                ptextField.ShouldReturn += delegate
                {
                    ptextField.EndEditing(true);
                    if (ptextField.Text == string.Empty || ptextField.Text == hint)
                    {
                        ptextField.Text = hint;
                        ptextField.TextColor = UIColor.Gray;
                    }
                    else
                    {
                        ptextField.TextColor = UIColor.Black;
                    }
                    return true;
                };

                ptextField.EditingDidEnd += (object sender, EventArgs e) =>
                {
                    ptextField.ResignFirstResponder();
                };
            }

            internal static void ConfiguraTextView(UITextView ptextView, UIScrollView scrollView, float offsetY, string placeHolder)
            {
                CGPoint offset = scrollView.ContentOffset;
                float previousYoffset = (float)scrollView.ContentOffset.Y;
                ptextView.Changed += (object sender, EventArgs e) =>
                {
                    //scrollView.SetContentOffset(new PointF((float)0, offsetY), true);
                };

                ptextView.Started += (object sender, EventArgs e) =>
                {
                    previousYoffset = (float)scrollView.ContentOffset.Y;
                    if (ptextView.Text == placeHolder)
                        ptextView.Text = string.Empty;

                    float v = ((float)ptextView.Frame.Y + 300);
                    scrollView.SetContentOffset(new PointF((float)0, v), true);
                };

                var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

                UIToolbar oToolbarTelefone = new UIToolbar();
                oToolbarTelefone.BarStyle = UIBarStyle.Default;
                oToolbarTelefone.Translucent = false;
                oToolbarTelefone.BackgroundColor = Colors.Laranja;
                oToolbarTelefone.SizeToFit();

                UIBarButtonItem doneButtonTelefone = new UIBarButtonItem(OCULTAR_TECLADO, UIBarButtonItemStyle.Done, (s, e) =>
                {
                    scrollView.SetContentOffset(new PointF((float)scrollView.ContentOffset.X, previousYoffset), true);
                    if (ptextView.Text == string.Empty || ptextView.Text == placeHolder)
                    {
                        ptextView.Text = placeHolder;
                        ptextView.TextColor = UIColor.Gray;
                    }
                    else
                    {
                        ptextView.TextColor = UIColor.Black;
                    }
                    ptextView.ResignFirstResponder();
                });
                doneButtonTelefone.AccessibilityLabel = "Ocultar Teclado";
                oToolbarTelefone.SetItems(new[] { flexibleSpace, doneButtonTelefone }, true);

                ptextView.InputAccessoryView = oToolbarTelefone;

                ptextView.Ended += delegate
                {
                    ptextView.EndEditing(true);
                    if (ptextView.Text == string.Empty || ptextView.Text == placeHolder)
                    {
                        ptextView.Text = placeHolder;
                        ptextView.TextColor = UIColor.Gray;
                    }
                    else
                    {
                        ptextView.TextColor = UIColor.Black;
                    }
                    //if (ptextView.Text == string.Empty)
                    //    ptextView.Text = placeHolder;

                    //scrollView.SetContentOffset(new PointF((float)offset.X, previousYoffset), true);

                    //ptextView.EndEditing(true);
                };
            }

            internal static void ConfiguraSearchBar(UISearchBar searchBar, EventHandler SrcEndereco_SearchButtonClicked, EventHandler<UISearchBarTextChangedEventArgs> SrcEndereco_TextChanged, EventHandler SrcEndereco_OnEditingStarted, EventHandler SrcEndereco_OnEditingStopped)
            {
                searchBar.SearchButtonClicked += SrcEndereco_SearchButtonClicked;

                var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

                UIToolbar oToolbarTelefone = new UIToolbar();
                oToolbarTelefone.BarStyle = UIBarStyle.Default;
                oToolbarTelefone.Translucent = false;
                oToolbarTelefone.BackgroundColor = Colors.Laranja;
                oToolbarTelefone.SizeToFit();
                UIBarButtonItem doneButtonTelefone = new UIBarButtonItem(OCULTAR_TECLADO, UIBarButtonItemStyle.Done, (s, e) =>
                {
                    searchBar.ResignFirstResponder();
                });
                doneButtonTelefone.AccessibilityLabel = "Ocultar Teclado";
                oToolbarTelefone.SetItems(new[] { flexibleSpace, doneButtonTelefone }, true);

                searchBar.InputAccessoryView = oToolbarTelefone;
                searchBar.TextChanged += SrcEndereco_TextChanged;
                searchBar.OnEditingStarted += SrcEndereco_OnEditingStarted;
                searchBar.OnEditingStopped += SrcEndereco_OnEditingStopped;
            }

            internal static void ConfiguraClearTextSearchBarForAccessibility(UIView v, string texto)
            {
                try
                {
                    if (v.Subviews != null && v.Subviews.Length > 0)
                    {
                        if (v.Subviews != null && v.Subviews.Length > 0)
                        {
                            UIView subV1 = v.Subviews[0];
                            if (subV1.Subviews != null && subV1.Subviews.Length > 0)
                            {
                                UIView subV2 = subV1.Subviews[1];
                                if (subV2.Subviews != null && subV2.Subviews.Length > 1)
                                {
                                    UIButton clearButton = ((UIButton)subV2.Subviews[2]);
                                    clearButton.AccessibilityLabel = texto;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    //dummy;
                }
            }

            internal static void ExibirViewController(UIViewController currentViewController, string viewControllerName)
            {
                var StoryBoard = UIStoryboard.FromName("MainStoryboard", null);
                var viewController = currentViewController.Storyboard.InstantiateViewController(viewControllerName) as UIViewController;

                if (viewController != null)
                {
                    UINavigationController nav = (currentViewController.NavigationController == null ? new UINavigationController() : currentViewController.NavigationController);
                    nav.ShowDetailViewController(viewController, null);
                }
                viewController.Dispose();
                viewController = null;
            }

            internal static void EmpurrarViewController(UIViewController currentView, string viewControllerName, UINavigationController navController = null)
            {
                if (currentView == null && navController == null)
                    throw new Exception("Falha ao abrir tela");

                UINavigationController navigationController;
                navigationController = (navController != null ? navController : GetNavigationController());
                var vc = currentView.Storyboard.InstantiateViewController(viewControllerName);
                navigationController.PushViewController(vc, true);
            }

            internal static void FecharViewControllerAtual(bool animated)
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                window.RootViewController.DismissViewController(animated, null);
            }

            internal static void FecharViewControllerAtual(UINavigationController navController, bool animated)
            {
                navController.DismissViewController(animated, null);
            }

            internal static void ResizeLabelFontSize(UILabel label, int initialFontSize, UIFontWeight fontWeight)
            {
                CGRect frame = label.Frame;
                label.AdjustsFontSizeToFitWidth = true;

                UIFont font = UIFont.SystemFontOfSize(initialFontSize, fontWeight);
                CGSize size = label.Text.StringSize(font);
                while (frame.Height < (size.Height) || frame.Width < (size.Width / label.Lines))
                {
                    initialFontSize--;
                    font = UIFont.SystemFontOfSize(initialFontSize, fontWeight);
                    size = label.Text.StringSize(font);
                }

                label.Font = UIFont.SystemFontOfSize(initialFontSize, fontWeight);
            }

            internal static void AlteraEstadoButton(UIButton btn, bool enabled)
            {
                btn.Enabled = enabled;
                btn.BackgroundColor = enabled ? Colors.Laranja : Colors.LaranjaEscuro;
                btn.SetTitleColor(enabled ? UIColor.White : UIColor.Gray, UIControlState.Normal);
            }

            internal static void AlteraCorButton(UIButton btn, bool enabled)
            {
                btn.BackgroundColor = enabled ? Colors.LaranjaEscuro : Colors.Laranja;
                btn.Font = UIFont.FromName(enabled ? "Helvetica Bold" : "Helvetica", btn.Font.PointSize);
            }

            internal static void AlteraEstadoButtonRound(UIButton btn, bool enabled)
            {
                btn.Enabled = enabled;
                btn.TintColor = enabled ? Colors.Laranja : Colors.LaranjaEscuro;
            }

            internal static void OpenWalkThrough(BaseViewController viewController)
            {
                ExibirViewController(viewController, "WalkThroughViewControllerGPS");
            }

            internal static void OpenWelcome(BaseViewController viewController)
            {
                ExibirViewController(viewController, "WelcomeViewController");
            }

            internal static void AlteraEstadoSwitchButton(UISwitch swt, UILabel lbl, bool enabled)
            {
                swt.Enabled = enabled;
                swt.OnTintColor = enabled ? Colors.Laranja : UIColor.Gray;
                lbl.TextColor = (enabled ? Colors.Laranja : UIColor.LightGray);
            }

            internal static void AlteraCorETituloButton(UIButton btn, string title, UIColor color)
            {
                btn.SetTitle(title, UIControlState.Normal);
                btn.BackgroundColor = color;
            }
        }

        internal static class Conversores
        {
            public static DateTime NSDateToDateTime(NSDate date)
            {
                DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
                DateTime currentDate = reference.AddSeconds(date.SecondsSinceReferenceDate);
                DateTime localDate = currentDate.ToLocalTime();
                return localDate;
            }

            public static NSDate DateTimeToNSDate(DateTime date)
            {
                return (NSDate)DateTime.SpecifyKind(date.Date, DateTimeKind.Local);
            }
        }
    }
}