using System;
using System.Threading.Tasks;
using Hangman.Utilities;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Hangman.ViewModels
{
    public class WordInformationVM : ObservableObject
    {
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
