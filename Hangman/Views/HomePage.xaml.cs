using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangman.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Hangman.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            On<iOS>().SetUseSafeArea(true);
            InitializeComponent();
            ApplyAnimations();
        }

        private void ApplyAnimations()
        {
            var parentAnimation = new Animation();
            //forward animation
            var headAnimationForward = new Animation(v => head.TranslationY = v, 0, 50, Easing.SinInOut);
            var bodyAnimationForward = new Animation(v => body.TranslationY = v, 0, 50, Easing.SinInOut);
            var leftHandAnimationForward = new Animation(v => leftHand.TranslationY = v, 0, 50, Easing.SinInOut);
            var rightHandAnimationForward = new Animation(v => rightHand.TranslationY = v, 0, 50, Easing.SinInOut);
            var leftLegAnimationForward = new Animation(v => leftLeg.TranslationY = v, 0, 50, Easing.SinInOut);
            var rightLegAnimationForward = new Animation(v => rightLeg.TranslationY = v, 0, 50, Easing.SinInOut);

            parentAnimation.Add(0, 1, headAnimationForward);
            parentAnimation.Add(0, 1, bodyAnimationForward);
            parentAnimation.Add(0, 1, leftHandAnimationForward);
            parentAnimation.Add(0, 1, rightHandAnimationForward);
            parentAnimation.Add(0, 1, leftLegAnimationForward);
            parentAnimation.Add(0, 1, rightLegAnimationForward);

            parentAnimation.Commit(this, "ParentAnimation", 16, 1500, Easing.SinInOut, (v, c) =>
            {
            }, () => true);
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userNameEntry.Text))
            {
                entryFrame.BackgroundColor = Color.Red;
                ShakeAnimation();
                return;
            }

            Preferences.Set(PreferenceKeys.UserName, userNameEntry.Text.Trim());
            App.Current.MainPage = new GamePage(userNameEntry.Text.Trim());
        }

        private void ShakeAnimation()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await entryFrame.TranslateTo(-15, 0, 50);
                await entryFrame.TranslateTo(15, 0, 50);
                await entryFrame.TranslateTo(-10, 0, 50);
                await entryFrame.TranslateTo(10, 0, 50);
                await entryFrame.TranslateTo(-5, 0, 50);
                await entryFrame.TranslateTo(5, 0, 50);
                entryFrame.TranslationX = 0;
            });
        }

        void userNameEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            entryFrame.BackgroundColor = Color.Transparent;
        }
    }
}
