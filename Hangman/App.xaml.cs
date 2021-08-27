using System;
using Hangman.Utilities;
using Hangman.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hangman
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var username = Preferences.Get(PreferenceKeys.UserName, "");
            if (string.IsNullOrWhiteSpace(username))
            {
                MainPage = new HomePage();
            }
            else
            {
                MainPage = new GamePage(username);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
