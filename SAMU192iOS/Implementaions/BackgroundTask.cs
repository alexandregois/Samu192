using UIKit;
using CoreGraphics;
using System;
using System.Threading;
using SAMU192Core.Interfaces;
using SAMU192Core.Exceptions;

namespace SAMU192iOS.Implementations
{
    internal class BackgroundTask : BackGroundTaskAbstract
    {
        public BackgroundTask(object screen, bool withOverlay, Action onPreExecute, Action<CancellationToken> onRunInBackGround, Action onPostExecute, Action onCancel, Action<Exception> onError, Action<ValidationException> onValidationException, string message = "", int cancelAfterMiliseconds = 60000, object scrollView = null) : 
            base(screen, withOverlay, onPreExecute, onRunInBackGround, onPostExecute, onCancel, onError, onValidationException, message, cancelAfterMiliseconds, scrollView)
        { }

        public override void StartOverlay()
        {
            if (WithOverlay)
            {
                new Foundation.NSObject().InvokeOnMainThread(() =>
                {
                    var bounds = UIScreen.MainScreen.Bounds;
                    CGRect rect;
                    if (ScrollView != null)
                    {
                        UIScrollView _scrollView = ((UIScrollView)ScrollView);

                        _scrollView.SetContentOffset(new CGPoint(_scrollView.ContentOffset.X, _scrollView.ContentOffset.Y), true);

                        rect = new CGRect(_scrollView.Bounds.X, 0, _scrollView.Bounds.Width, _scrollView.Bounds.Height + _scrollView.ContentOffset.Y);
                        Overlay = new LoadingOverlay(rect, Interrupt, Message);
                        _scrollView.Add((LoadingOverlay)Overlay);
                        _scrollView.ScrollEnabled = false;
                    }
                    else
                    {
                        UIView view = ((UIView)Screen);
                        rect = new CGRect(view.Bounds.X - bounds.X, view.Bounds.Y - bounds.Y, view.Bounds.Width, view.Bounds.Height);
                        Overlay = new LoadingOverlay(rect, Interrupt, Message);
                        view.Add((LoadingOverlay)Overlay);
                    }
                });
            }
        }

        public override void HideOverlay()
        {
            if (WithOverlay)
            {
                new Foundation.NSObject().InvokeOnMainThread(() =>
                {
                    ((LoadingOverlay)Overlay).Hide();
                    if (ScrollView != null)
                    {
                        UIScrollView _scrollView = ((UIScrollView)ScrollView);
                        _scrollView.ScrollEnabled = true;
                    }
                });
            }
        }

        public override void OnPreExecute()
        {
            MyOnPreExecute();
        }

        public override void OnPostExecute()
        {
            HideOverlay();
            MyOnPostExecute();
        }

        public override void OnCancel()
        {
            HideOverlay();
            MyOnCancel();
        }

        public override void OnError(Exception ex)
        {
            HideOverlay();
            MyOnError(ex);
        }

        public override void OnValidationException(ValidationException vex)
        {
            HideOverlay();
            MyOnValidationException(vex);
        }

        private class LoadingOverlay : UIView
        {
            UIActivityIndicatorView activitySpinner;
            UILabel loadingLabel;
            //UIButton cancelButton;
            Action OnCancel;

            public LoadingOverlay(CGRect frame, Action onCancel, string message) : base(frame)
            {
                OnCancel = onCancel;
                BackgroundColor = UIColor.Black;
                Alpha = 0.75f;
                AutoresizingMask = UIViewAutoresizing.All;

                nfloat labelHeight = 25;
                nfloat labelWidth = Frame.Width - 20;

                nfloat centerX = Frame.Width / 2;
                nfloat centerY = (Frame.Height / 2);

                activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
                activitySpinner.Frame = new CGRect(
                    centerX - (activitySpinner.Frame.Width / 2),
                    centerY - activitySpinner.Frame.Height - 20,
                    activitySpinner.Frame.Width,
                    activitySpinner.Frame.Height);
                activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
                AddSubview(activitySpinner);
                activitySpinner.StartAnimating();

                loadingLabel = new UILabel(new CGRect(
                    centerX - (labelWidth / 2),
                    centerY + 10,
                    labelWidth,
                    labelHeight
                    ));
                loadingLabel.BackgroundColor = UIColor.Clear;
                loadingLabel.TextColor = UIColor.White;
                loadingLabel.Text = string.IsNullOrEmpty(message) ? "Carregando Dados..." : message;
                loadingLabel.TextAlignment = UITextAlignment.Center;
                loadingLabel.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
                AddSubview(loadingLabel);

                //cancelButton = new UIButton(new CGRect(centerX - (60), centerY + 60, 120, 35));
                //cancelButton.SetTitle("Interromper", UIControlState.Normal);
                //cancelButton.Layer.CornerRadius = 8;
                //cancelButton.TouchUpInside += CancelButton_TouchUpInside;
                //cancelButton.Alpha = 1.0f;
                //cancelButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
                //cancelButton.BackgroundColor = UIColor.FromRGB(0, 64, 26).ColorWithAlpha(1.0f);
                //AddSubview(cancelButton);
            }

            private void CancelButton_TouchUpInside(object sender, EventArgs e)
            {
                this.OnCancel();
            }
            
            internal void Hide()
            {
                UIView.Animate(
                    0.5, // duration
                    () => { Alpha = 0; },
                    () => { RemoveFromSuperview(); }
                );
            }
        }
    }
}