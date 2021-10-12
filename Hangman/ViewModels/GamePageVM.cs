using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Hangman.Model;
using Hangman.Views;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Hangman.ViewModels
{
    public class GamePageVM : ObservableObject
    {
        #region Variables
        private ObservableCollection<AlphabetModel> alphabetList;
        private HashSet<string> _selectedAlphabets;
        private HashSet<string> _selectedWords;
        private int attemptsLeft = 1;
        private List<string> WordList;
        private ObservableCollection<string> currentWordList;
        private string WordToBeGuessed;
        private string currentWord = "";
        private bool nextButtonVisibility;
        private Color poleBase = Color.FromHex("#d7dbf5");
        private Color pole = Color.FromHex("#d7dbf5");
        private Color rodHoldingRope = Color.FromHex("#d7dbf5");
        private Color rope = Color.FromHex("#d7dbf5");
        private Brush head = new SolidColorBrush(Color.FromHex("#d7dbf5"));
        private Color body = Color.FromHex("#d7dbf5");
        private Color leftHand = Color.FromHex("#d7dbf5");
        private Color rightHand = Color.FromHex("#d7dbf5");
        private Color leftLeg = Color.FromHex("#d7dbf5");
        private Color rightLeg = Color.FromHex("#d7dbf5");
        private double translateTo = 0;
        private int _colorHangman = 1;
        private bool wordGuessed;
        private Color messageColor;
        private string message;
        private bool displayMessage;
        #endregion

        #region Properties
        public ObservableCollection<AlphabetModel> AlphabetList { get => alphabetList; set => SetProperty(ref alphabetList, value); }
        public int AttemptsLeft { get => attemptsLeft; set => SetProperty(ref attemptsLeft, value); }
        public ObservableCollection<string> CurrentWordList { get => currentWordList; set => SetProperty(ref currentWordList, value); }
        public string CurrentWord { get => currentWord; set => SetProperty(ref currentWord, value); }
        public bool NextButtonVisibility { get => nextButtonVisibility; set => SetProperty(ref nextButtonVisibility, value); }
        public bool WordGuessed { get => wordGuessed; set => SetProperty(ref wordGuessed, value); }
        //colors for hangman
        public Color PoleBase { get => poleBase; set => SetProperty(ref poleBase, value); }
        public Color Pole { get => pole; set => SetProperty(ref pole, value); }
        public Color RodHoldingRope { get => rodHoldingRope; set => SetProperty(ref rodHoldingRope, value); }
        public Color Rope { get => rope; set => SetProperty(ref rope, value); }
        public Brush Head { get => head; set => SetProperty(ref head, value); }
        public Color Body { get => body; set => SetProperty(ref body, value); }
        public Color LeftHand { get => leftHand; set => SetProperty(ref leftHand, value); }
        public Color RightHand { get => rightHand; set => SetProperty(ref rightHand, value); }
        public Color LeftLeg { get => leftLeg; set => SetProperty(ref leftLeg, value); }
        public Color RightLeg { get => rightLeg; set => SetProperty(ref rightLeg, value); }
        public double TranslateTo { get => translateTo; set => SetProperty(ref translateTo, value); }
        public string Username { get; set; }
        public string Message { get => message; set => SetProperty(ref message, value); }
        public Color MessageColor { get => messageColor; set => SetProperty(ref messageColor, value); }
        public bool DisplayMessage { get => displayMessage; set => SetProperty(ref displayMessage, value); }
        #endregion

        #region Command
        public Command<AlphabetModel> SelectAlphabetCommand => new((data) =>
        {
            if (_selectedAlphabets.Contains(data.Alphabet) || _colorHangman >= 10 || WordGuessed)
                return;

            data.AlphabetColor = Color.FromHex("#092850");
            data.FrameBgColor = Color.White;
            _selectedAlphabets.Add(data.Alphabet);
            ReplaceWithSelectedAlphabets(data.Alphabet.ToLower());
            if (_colorHangman >= 10 && !WordGuessed)
            {
                DisplayMessage = true;
                Message = "His ghost will haunt you now xD";
                MessageColor = Color.FromHex("#b52f2f");
                CurrentWord = WordToBeGuessed;
                NextButtonVisibility = true;
            }
        });

        public Command RefreshWordCommand => new(() =>
        {
            _colorHangman = 1;
            NextButtonVisibility = false;
            WordGuessed = false;
            DisplayMessage = false;
            _selectedAlphabets.Clear();
            GetAlphabetList();
            GetTheWordToBeGuessed();
            ResetHangmanColors();
        });

        public AsyncCommand WordInfoCommand => new(async () =>
         {
             await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new WordInformationPage(WordToBeGuessed)));
         }, allowsMultipleExecutions: false);


        #endregion

        public GamePageVM(string username)
        {
            Username = username;
            Initializevariables();
            GetJsonData();
            GetAlphabetList();
            GetTheWordToBeGuessed();
        }

        #region Methods

        private void Initializevariables()
        {
            AlphabetList = new();
            WordList = new();
            _selectedAlphabets = new();
            _selectedWords = new();
            CurrentWordList = new();
        }

        private void GetJsonData()
        {
            try
            {
                string jsonFileName = "jsonword.json";
                var assembly = typeof(GamePageVM).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
                using (var reader = new StreamReader(stream))
                {
                    var jsonString = reader.ReadToEnd();
                    //Converting JSON Array Objects into generic list    
                    WordList = JsonConvert.DeserializeObject<List<string>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void GetAlphabetList()
        {
            var tempList = new List<AlphabetModel>();
            for (int i = 65; i <= 90; i++)
            {
                //Add the characters
                tempList.Add(new AlphabetModel
                {
                    Alphabet = Convert.ToChar(i).ToString(),
                    FrameBgColor = Color.FromHex("#092850"),
                    AlphabetColor = Color.White
                });
            }
            AlphabetList = new(tempList);
        }

        private void GetTheWordToBeGuessed()
        {
        randomWord:
            var randIndex = new Random().Next(WordList.Count);
            WordToBeGuessed = WordList[randIndex];
            CurrentWord = "";

            if (WordToBeGuessed.Length > 8 || _selectedWords.Contains(WordToBeGuessed))
                goto randomWord;

            var randomIndexSet = NumberOfTextToBeFilled(WordToBeGuessed.Length);

            _selectedWords.Add(WordToBeGuessed);
            for (int i = 0; i < WordToBeGuessed.Length; i++)
            {
                var missingWord = "__" + " ";
                if (randomIndexSet.Contains(i))
                {
                    missingWord = WordToBeGuessed[i] + " ";
                }
                CurrentWord += missingWord;
            }
            CurrentWord = CurrentWord.TrimEnd();
        }

        private void ReplaceWithSelectedAlphabets(string alphabet)
        {
            try
            {
                if (!WordToBeGuessed.Contains(alphabet))
                {
                    ColorHangman(_colorHangman);
                    _colorHangman++;
                    return;
                }
                int charIndex = 0;
                var charIndexList = new List<int>();
                var currwordArr = CurrentWord.Split(' ');
                foreach (var data in WordToBeGuessed.ToList())
                {
                    if (data.ToString() == alphabet)
                    {
                        charIndexList.Add(charIndex);
                        currwordArr[charIndex] = alphabet;
                    }
                    charIndex++;
                }
                CurrentWord = "";
                foreach (var data in currwordArr)
                {
                    CurrentWord += data + " ";
                }
                CurrentWord = CurrentWord.TrimEnd();
                CheckTheGuessedWord(CurrentWord.Replace(" ", ""));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void CheckTheGuessedWord(string currentWord)
        {
            if (currentWord != WordToBeGuessed)
                return;

            SuccessHangmanColor();
            NextButtonVisibility = true;
            WordGuessed = true;
            DisplayMessage = true;
            Message = "YAYYY!!. You saved the guy.";
            MessageColor = Color.FromHex("#70cf8a");
        }

        private void ColorHangman(int count)
        {
            switch (count)
            {
                case 1:
                    PoleBase = Color.FromHex("#092850");
                    break;
                case 2:
                    Pole = Color.FromHex("#092850");
                    break;
                case 3:
                    RodHoldingRope = Color.FromHex("#092850");
                    Rope = Color.FromHex("#092850");
                    break;
                case 4:
                    Head = new SolidColorBrush(Color.FromHex("#092850"));
                    break;
                case 5:
                    Body = Color.FromHex("#092850");
                    break;
                case 6:
                    LeftHand = Color.FromHex("#092850");
                    break;
                case 7:
                    RightHand = Color.FromHex("#092850");
                    break;
                case 8:
                    LeftLeg = Color.FromHex("#092850");
                    break;
                case 9:
                    RightLeg = Color.FromHex("#092850");
                    break;
            }
        }

        private void ResetHangmanColors()
        {
            TranslateTo = 0;
            PoleBase = Color.FromHex("#d7dbf5");
            Pole = Color.FromHex("#d7dbf5");
            RodHoldingRope = Color.FromHex("#d7dbf5");
            Rope = Color.FromHex("#d7dbf5");
            Head = new SolidColorBrush(Color.FromHex("#d7dbf5"));
            Body = Color.FromHex("#d7dbf5");
            LeftHand = Color.FromHex("#d7dbf5");
            RightHand = Color.FromHex("#d7dbf5");
            LeftLeg = Color.FromHex("#d7dbf5");
            RightLeg = Color.FromHex("#d7dbf5");
        }

        private void SuccessHangmanColor()
        {
            TranslateTo = 50;
            Head = new SolidColorBrush(Color.FromHex("#70cf8a"));
            Body = Color.FromHex("#70cf8a");
            LeftHand = Color.FromHex("#70cf8a");
            RightHand = Color.FromHex("#70cf8a");
            LeftLeg = Color.FromHex("#70cf8a");
            RightLeg = Color.FromHex("#70cf8a");
        }

        private HashSet<int> NumberOfTextToBeFilled(int textLength)
        {
            var hashset = new HashSet<int>();
            if (textLength <= 4)
            {
                hashset.Add(new Random().Next(3));
            }
            else if (textLength <= 5 && textLength <= 6)
            {
                hashset.Add(new Random().Next(1, 3));
                hashset.Add(new Random().Next(4, textLength));
            }
            else if (textLength <= 7 && textLength <= 8)
            {
                hashset.Add(new Random().Next(1, 3));
                hashset.Add(new Random().Next(4, 6));
                hashset.Add(new Random().Next(6, textLength));
            }

            return hashset;
        }

        #endregion
    }

}