using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Hangman.Model;
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
        private int attemptsLeft = 8;
        private bool _wordGuessed;
        private List<string> WordList;
        private ObservableCollection<string> currentWordList;
        private string WordToBeGuessed;
        private string currentWord = "";
        private bool nextButtonVisibility;
        #endregion

        #region Properties
        public ObservableCollection<AlphabetModel> AlphabetList { get => alphabetList; set => SetProperty(ref alphabetList, value); }
        public int AttemptsLeft { get => attemptsLeft; set => SetProperty(ref attemptsLeft, value); }
        public ObservableCollection<string> CurrentWordList { get => currentWordList; set => SetProperty(ref currentWordList, value); }
        public string CurrentWord { get => currentWord; set => SetProperty(ref currentWord, value); }
        public bool NextButtonVisibility { get => nextButtonVisibility; set => SetProperty(ref nextButtonVisibility, value); }
        #endregion

        #region Command
        public Command<AlphabetModel> SelectAlphabetCommand => new((data) =>
        {
            if (_selectedAlphabets.Contains(data.Alphabet) || AttemptsLeft <= 0 || _wordGuessed)            
                return;            

            data.AlphabetColor = Color.FromHex("#092850");
            data.FrameBgColor = Color.White;
            _selectedAlphabets.Add(data.Alphabet);
            AttemptsLeft--;
            ReplaceWithSelectedAlphabets(data.Alphabet.ToLower());
            if (AttemptsLeft <= 0 && !_wordGuessed)            
                NextButtonVisibility = true;
            
        });

        public Command RefreshWordCommand => new(() =>
        {
            AttemptsLeft = 8;
            NextButtonVisibility = false;
            _wordGuessed = false;
            _selectedAlphabets.Clear();
            GetAlphabetList();
            GetTheWordToBeGuessed();
        });
        #endregion

        public GamePageVM()
        {
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

            _selectedWords.Add(WordToBeGuessed);
            for (int i = 0; i < WordToBeGuessed.Length; i++)
            {
                CurrentWord += "__ ";
            }
            CurrentWord = CurrentWord.TrimEnd();
        }

        private void ReplaceWithSelectedAlphabets(string alphabet)
        {
            try
            {
                if (!WordToBeGuessed.Contains(alphabet))
                    return;
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

            NextButtonVisibility = true;
            _wordGuessed = true;

        }

        #endregion
    }
}
