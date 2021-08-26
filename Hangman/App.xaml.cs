using System;
using Hangman.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hangman
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new GamePage();
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
