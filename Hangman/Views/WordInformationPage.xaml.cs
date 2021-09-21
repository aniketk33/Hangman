using System;
using System.Collections.Generic;
using Hangman.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Hangman.Views
{
    public partial class WordInformationPage : ContentPage
    {
        WordInformationVM vm;
        public WordInformationPage(string word)
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            BindingContext = vm = new WordInformationVM(word);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }
    }
}
