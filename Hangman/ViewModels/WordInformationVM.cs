using System;
using System.Threading.Tasks;
using Hangman.Utilities;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Hangman.ViewModels
{
    public class WordInformationVM : ObservableObject
    {
        //private string word;

        //public string Word { get => word; set => SetProperty(ref word, value); }
        public string Word { get; set; }

        public WordInformationVM(string word)
        {
            Word = word;
        }

        public void OnAppearing()
        {
            Task.Run(async() =>
            {
                var result = await new HttpServices().GetWordMeaning(Word);
            });
        }

        
    }
}
