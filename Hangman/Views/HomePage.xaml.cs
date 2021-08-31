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

        private async void ApplyAnimations()
        {
            while (true)
            {
                await hangman.TranslateTo(0, 50, 1500, Easing.SinInOut);
                await hangman.TranslateTo(0, 0, 1500, Easing.SinInOut);
            }
        }

        void Button_Clicked(Object sender, EventArgs e)
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

        void userNameEntry_TextChanged(Object sender, TextChangedEventArgs e)
        {
            if (entryFrame.BackgroundColor == Color.Transparent)
                return;

            entryFrame.BackgroundColor = Color.Transparent;
        }
    }
}
