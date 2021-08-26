using System;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Hangman.Model
{
    public class AlphabetModel : ObservableObject
    {
        private Color frameBgColor;
        private Color alphabetColor;

        public string Alphabet { get; set; }
        public Color AlphabetColor { get => alphabetColor; set => SetProperty(ref alphabetColor, value); }
        public Color FrameBgColor { get => frameBgColor; set => SetProperty(ref frameBgColor, value); }

        public AlphabetModel()
        {
        }
    }
}
