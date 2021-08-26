using System;
using System.Collections.Generic;
using Hangman.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Hangman.Views
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            BindingContext = new GamePageVM();
        }
    }
}
