using System;
using System.Net;
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
                try
                {
                    var result = await new HttpServices().GetWordMeaning(Word);
                    if (result.Item2 is not HttpStatusCode.OK)
                        return;
                    DesignUIWithServiceResult(result.Item1);
                }
                catch (Exception ex)
                {

                }
            });
        }

        private void DesignUIWithServiceResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return;            
        }
    }
}
