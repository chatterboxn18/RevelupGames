using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Revedle;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MasterVelvet
{
	public class WordleGame : SerializedMonoBehaviour
	{
		[Title("Managers")]
		[SerializeField] private RVDResourceManager _resourceManager;
		[SerializeField] private RVDAesthetics _aestheticsManager;
		[SerializeField] private ToastManager<RVDToast> _toastManager;
		
		[Title("UI")]
		[SerializeField] private LetterBoxItem _letterBoxPrefab;
		[SerializeField] private Transform _lettersParent;
		[SerializeField] private Image _titleImage;
		[SerializeField] private TextMeshProUGUI _title;
		[SerializeField] private Image _backgroundImage;
		private List<string> _wordGrid = new List<string>();
		private int _totalLetters
		{
			get { return _totalWords * _wordLength; }
		}
		private int _totalWords = 6;
		private int _wordLength = 5;

		private int _wordIndexInList;
		private int _currentLetterIndex;
		private int _currentWordIndex;
		private int _wordLetterIndex;
		private string _currentWord;
		private bool _isOver; 

		private List<LetterBoxItem> _letterBoxes = new List<LetterBoxItem>();
		[SerializeField] private string _wordToGuess;
		[OdinSerialize] private Dictionary<string, Image> _keyboardDict = new Dictionary<string, Image>();
		[SerializeField] private List<Color> _colors = new List<Color>();

		[Title("Menu")] 
		[SerializeField] private BaseController _menuController;
		private const string AESTHETIC_KEY = "revefestivalDay";
		private int _currentAestheticIndex = 0;
		[SerializeField] private SimpleButton _statsButton;
		[SerializeField] private DaySelectItem _dayButton;
		[SerializeField] private RectTransform _dayButtonParent;
		private List<DaySelectItem> _dayButtonList = new List<DaySelectItem>();

		[Title("Success Screen")] [SerializeField]
		private List<TextMeshProUGUI> _successLetters = new List<TextMeshProUGUI>();
		[SerializeField] private BaseController _successScreen;
		[SerializeField] private TextMeshProUGUI _dykDescription;
		[SerializeField] private Image _dykCover;
		[SerializeField] private TextMeshProUGUI _successTitle;
		[SerializeField] private TextMeshProUGUI _successDescription;
		private string _shareString = "https://twitter.com/intent/tweet?text=";
		private string _missCharacter = "⬛";
		private string _almostCharacter = "🟥";
		private string _correctCharacter = "💗";
		private string _shareAnswerString = "";
		private string _safeAnswerString = "";
		private string _dykLink;
		private const string _winTitle = "CONGRATS";
		private const string _winDescription = "You guessed the word";
		private const string _loseTitle = "SORRY";
		private const string _loseDescription = "The word was";

		[Title("Stats")] 
		[SerializeField] private TextMeshProUGUI _plays;
		[SerializeField] private TextMeshProUGUI _winPercent;
		[SerializeField] private TextMeshProUGUI _streak;
		[SerializeField] private TextMeshProUGUI _bestStreak;
		[SerializeField] private TextMeshProUGUI _bestGuess;
		[SerializeField] private TextMeshProUGUI _guessDistribution;
		private const string PLAY_KEY = "plays";
		private const string WIN_KEY = "winPercent";
		private const string STREAK_KEY = "streak";
		private const string DATE_KEY = "lastDatePlayed";
		private const string BESTSTREAK_KEY = "bestStreak";
		private const string GUESS_KEY = "guessDistribution";
		private const string PLAYS_KEY = "gamePlay";
		private const string GUESSES_KEY = "wordGuesses";
		
		//for word selection
		private const string GUESSED_WORDS_KEY = "wordsGottenCorrect";

		//wordslist
		private const string WORDS_KEY = "wordsList";
		private int _todayIndex = 0;
		
		[Title("Debug")]
		[SerializeField] private int DEBUGINDEX;
		
		[DllImport("__Internal")]
		private static extern void CopyToClipboard(int month, int day, string array, string guesses);
		
		[DllImport("__Internal")]
		private static extern void OpenLink(string link);

		private void Awake()
		{
			if (!PlayerPrefs.HasKey(AESTHETIC_KEY))
				PlayerPrefs.SetInt(AESTHETIC_KEY, 0);
			PrepareAesthetics(PlayerPrefs.GetInt(AESTHETIC_KEY));
		}

		private void PrepareAesthetics(int index, bool isOverride = false)
		{
			var day = _aestheticsManager.DayAsthetics[index];
			_backgroundImage.color = day.Background;
			if (string.IsNullOrEmpty(day.Title))
			{
				_titleImage.gameObject.SetActive(true);
				_titleImage.sprite = day.TitleImage;
				_title.gameObject.SetActive(false);
			}
			else
			{
				_title.text = day.Title;
				_title.color = day.TitleColor;
				_title.gameObject.SetActive(true);
				_titleImage.gameObject.SetActive(false);
			}

			foreach (var value in _keyboardDict.Values)
			{
				if (isOverride)
					value.color = day.KeyColor;
				else if (!_colors.Contains(value.color))
					value.color = day.KeyColor;

			}
		}

		private void PrepareWord(RVDResourceManager.WordEntry entry)
		{
			if (_isOver)
			{
				_isOver = false;
			}
			
			// should always reset if selected
			ResetWord();
			PrepareAesthetics(PlayerPrefs.GetInt(AESTHETIC_KEY), true);
			
			_wordToGuess = entry.Word;
			_wordLength = _wordToGuess.Length;
			_dykDescription.text = entry.Description + "\nClick here to see more.";
			_dykLink = entry.Link;
			_dykCover.sprite = _resourceManager.AlbumCovers[entry.SpriteIndex];
		}

		private void PrepareWordList()
		{
			var counter = 0;
			var list = "";
			if (PlayerPrefs.HasKey(GUESSED_WORDS_KEY))
			{
				list = PlayerPrefs.GetString(GUESSED_WORDS_KEY);
			}
			foreach (var item in _resourceManager.WordEntryController.JsonContent.List)
			{
				var day = Instantiate(_dayButton, _dayButtonParent);
				day.SetDay("Day " + (counter +1), list.Contains(item.Word), counter);
				day.Evt_SelectWord += ButtonEvt_SetWord;
				_dayButtonList.Add(day);
				counter++;
			}
		}

		private IEnumerator Start()
		{
			while (!RVDResourceManager.IsReady)
			{
				yield return null;
			}
			//Getting today's word by subtracting today with the first day
			/*var index = DateTime.Today.Date - new DateTime(2022, 3, 1);
			
			var item = [index.Days];
			
			_todayIndex = entry.Days;
			
			// set up all the words in menu
			{
				
			}*/

			PrepareWordList();
			
			// Creates all the letters for the first time
			for (var i = 0; i < _totalLetters; i++)
			{
				var letter = Instantiate(_letterBoxPrefab, _lettersParent);
				_letterBoxes.Add(letter);
			}

			// REMOVED SO IT'S NOT DATE DEPENDENT
			/*if (PlayerPrefs.HasKey(DATE_KEY))
			{
				var date = DateTime.Parse(PlayerPrefs.GetString(DATE_KEY));
				if (date.Date == DateTime.Today.Date)
				{
					_isOver = true;
					//if (PlayerPrefs.HasKey(PLAYS_KEY))
					//{
					//	Debug.Log(PlayerPrefs.GetString(PLAYS_KEY));
					//	WriteShareString(PlayerPrefs.GetString(PLAYS_KEY));
					//}
					ParseGuessString();
					DisplayPlayerStats();
					Evt_OnSuccess();
				}
				else
				{
					_statsButton.SetVisibility(false);
				}
			}
			else
			{
				_statsButton.SetVisibility(false);
			}*/
		}

		private void ParseGuessString()
		{
			// needs to have established word length before calling this function
			if (!PlayerPrefs.HasKey(GUESSES_KEY)) return;
			var words = PlayerPrefs.GetString(GUESSES_KEY);
			for (var i = 1; i < words.Length + 1; i++)
			{
				Evt_UpdateKey(words[i-1].ToString().ToLower());
				if (i % _wordLength == 0)
				{
					var correct =CheckAnswer();
					_currentWordIndex++;
					_shareAnswerString += "\n";
				}
			}
		}
		
		private void WriteShareString(string seq)
		{
			_shareAnswerString = "";
			for (var j = 0; j < (seq.Length/_wordLength); j++){
				for (var i = 0; i < _wordLength; i++){
					if (seq[i + (_wordLength * j)] == '0') 
						_shareAnswerString += _missCharacter;
					if (seq[i + (_wordLength * j)] == '1') 
						_shareAnswerString += _almostCharacter;
					if (seq[i + (_wordLength * j)] == '2') 
						_shareAnswerString += _correctCharacter;

				}
				_shareAnswerString+= "\n";
			}
		}
		
		private void UpdatePlayerStats(bool isWin)
		{
			//also keep a copy of gameplay
			PlayerPrefs.SetString(PLAYS_KEY, _safeAnswerString);
			
			if (!PlayerPrefs.HasKey(PLAY_KEY)) PlayerPrefs.SetInt(PLAY_KEY, 1);
			else
			{
				var newNumber = PlayerPrefs.GetInt(PLAY_KEY) + 1; 
				PlayerPrefs.SetInt(PLAY_KEY, newNumber);
			}
			if (!PlayerPrefs.HasKey(WIN_KEY)) PlayerPrefs.SetInt(WIN_KEY, isWin ? 1:0);
			else
			{
				var newNumber = PlayerPrefs.GetInt(WIN_KEY) + 1; 
				PlayerPrefs.SetInt(WIN_KEY, newNumber);
				_winPercent.text = (newNumber / PlayerPrefs.GetInt(PLAY_KEY)).ToString();
			}

			if (!PlayerPrefs.HasKey(STREAK_KEY))
			{
				var win = isWin ? 1 : 0;
				PlayerPrefs.SetInt(STREAK_KEY, win);
				PlayerPrefs.SetString(DATE_KEY, DateTime.Today.ToShortDateString());
				PlayerPrefs.SetInt(BESTSTREAK_KEY, win);
			}
			else
			{
				var date = DateTime.Parse(PlayerPrefs.GetString(DATE_KEY));
				if ((DateTime.Today.Date - date.Date).Days == 1)
				{
					var newStreak =PlayerPrefs.GetInt(STREAK_KEY) + 1;
					PlayerPrefs.SetInt(STREAK_KEY, newStreak);
				}
				else
				{
					PlayerPrefs.SetInt(STREAK_KEY, isWin ? 1: 0);
				}
				
				//check if the streak is better than best streak
				var best = PlayerPrefs.GetInt(BESTSTREAK_KEY);
				var current = PlayerPrefs.GetInt(STREAK_KEY);
				if (best < current)
				{
					PlayerPrefs.SetInt(BESTSTREAK_KEY, current);
				}
				
				PlayerPrefs.SetString(DATE_KEY, DateTime.Today.ToShortDateString()); //updating the Date
			}

			if (!PlayerPrefs.HasKey(GUESS_KEY)) {PlayerPrefs.SetString(GUESS_KEY, "0,0,0,0,0,0");}
			if (isWin)
			{
				var guessList = PlayerPrefs.GetString(GUESS_KEY).Split(',').ToList();
				var number = int.Parse(guessList[_currentWordIndex]);
				guessList[_currentWordIndex] = (number + 1).ToString();
				var returnString = "";
				for (var i = 0; i < 5; i++)
				{
					returnString += guessList[i] + ",";
					//returnString += (i + ":" + guessList[i] + "\t");
				}

				returnString += guessList[5];
				PlayerPrefs.SetString(GUESS_KEY, returnString);
			}

			DisplayPlayerStats();
		}

		private void DisplayPlayerStats()
		{
			_plays.text = PlayerPrefs.GetInt(PLAY_KEY).ToString();
			_winPercent.text = (PlayerPrefs.GetInt(WIN_KEY) / PlayerPrefs.GetInt(PLAY_KEY) * 100).ToString();
			_streak.text = PlayerPrefs.GetInt(STREAK_KEY).ToString();
			_bestStreak.text = PlayerPrefs.GetInt(BESTSTREAK_KEY).ToString();
			_bestGuess.text = PlayerPrefs.GetInt(WIN_KEY).ToString();
			_guessDistribution.text = CreateGuessTextLine();
		}
		
		private string CreateGuessTextLine()
		{
			var guessList = PlayerPrefs.GetString(GUESS_KEY).Split(',');
			var returnString = "";
			for (var i = 1; i < 7; i++)
			{
				if (i == 6)
				{
					returnString += (i + ":" + guessList[i-1]);
					break;
				}
				returnString += (i + ":" + guessList[i-1] + "\t");
			}
			return returnString;
		}

		private void Evt_OnSuccess()
		{
			_isOver = true;
			var word = "";
			for (var ind = 0; ind < _letterBoxes.Count; ind++)
			{
				var character = _letterBoxes[ind].Letter;
				if (string.IsNullOrEmpty(character)) break;
				word += character;
			}
			
			// stats 
			//PlayerPrefs.SetString(GUESSES_KEY, word);
			for (var i = 0; i < _wordLength; i++)
			{
				_successLetters[i].text = _wordToGuess[i].ToString();
			}
			_statsButton.SetVisibility(true);
			StartCoroutine(TurnOnSuccess());
		}

		private IEnumerator TurnOnSuccess()
		{
			yield return new WaitForSeconds(0.5f);
			_successScreen.TransitionOn();
		}
		
		private void Evt_UpdateKey(string letter)
		{
			_letterBoxes[_currentLetterIndex].SetLetter(letter);
			_wordLetterIndex++;
			_currentLetterIndex++;
			_currentWord += letter;
		}

		private void ResetWord()
		{
			_currentWord = "";
			_currentLetterIndex = 0;
			_currentWordIndex = 0;
			_wordLetterIndex = 0;
			foreach (var letter in _letterBoxes)
			{
				letter.UpdateBox(MRVEnum.LetterState.None);
				letter.SetLetter(String.Empty);
			}
		}

		private bool CheckAnswer()
		{
			var checkIndex = _currentWordIndex * _wordLength;
			var correctIndexs = new List<int>();
			var kindaIndexs = new List<int>();
			var correctWord = new List<string>();
			for (var i = 0; i < _wordLength; i++)
			{
				if (_letterBoxes[checkIndex].Letter == _wordToGuess[i].ToString())
				{
					correctIndexs.Add(i);
				}
				else
				{
					correctWord.Add(_wordToGuess[i].ToString());
				}
				checkIndex++;
			}

			checkIndex = _currentWordIndex * _wordLength; // restart checkIndex
			for (var i = 0; i < _wordLength; i++)
			{
				if (correctIndexs.Contains(i))
				{
					checkIndex++;
					continue;
				}
				if (correctWord.Contains(_letterBoxes[checkIndex].Letter))
				{
					kindaIndexs.Add(i);
					correctWord.Remove(_letterBoxes[checkIndex].Letter); // remove to avoid duplicates
				}
				checkIndex++;
			}
			
			checkIndex = _currentWordIndex * _wordLength; // restart checkIndex
			for (var r = 0; r < _wordLength; r++)
			{
				var letter = _letterBoxes[checkIndex].Letter;
				if (correctIndexs.Contains(r))
				{
					// for share
					_shareAnswerString += _correctCharacter;
					_safeAnswerString+=2;
					
					_letterBoxes[checkIndex].UpdateBox(MRVEnum.LetterState.Correct, 0.1f * r +0.2f);
					
					_keyboardDict[letter].color = _colors[0];
				}
				else if (kindaIndexs.Contains(r))
				{
					// for share
					_shareAnswerString += _almostCharacter;
					_safeAnswerString+=1;
					
					_letterBoxes[checkIndex].UpdateBox(MRVEnum.LetterState.Close, 0.1f * r +0.2f);
					
					if (_keyboardDict[letter].color != _colors[0])
						_keyboardDict[letter].color = _colors[1];
				}
				else
				{
					// for share
					_shareAnswerString += _missCharacter;
					_safeAnswerString+=0;
					
					_letterBoxes[checkIndex].UpdateBox(MRVEnum.LetterState.Wrong, 0.1f * r +0.2f);
					
					if (_keyboardDict[letter].color != _colors[0] && _keyboardDict[letter].color != _colors[1])
						_keyboardDict[letter].color = _colors[2];
				}

				checkIndex++;
			}

			return correctIndexs.Count == _wordLength;
		}
		
		private void AddWordToCorrectWordList()
		{
			if (!PlayerPrefs.HasKey(GUESSED_WORDS_KEY))
			{
				PlayerPrefs.SetString(GUESSED_WORDS_KEY, _currentWord);
			}
			else
			{
				var list = PlayerPrefs.GetString(GUESSED_WORDS_KEY);
				if (!list.Contains(_currentWord))
				{
					list += "," + _currentWord;
					PlayerPrefs.SetString(GUESSED_WORDS_KEY, list);
				}
			}
			
			_dayButtonList[_wordIndexInList].UpdateDay(true);
		}
		
		private void AddWordKey()
		{
			if (!PlayerPrefs.HasKey(WORDS_KEY))
			{
				PlayerPrefs.SetString(WORDS_KEY, _todayIndex.ToString());
			}
			else
			{
				var list = PlayerPrefs.GetString(WORDS_KEY);
				list += "," + _todayIndex;
				PlayerPrefs.SetString(WORDS_KEY, list);
			}
		}
		
		private void UpdateSuccessText(bool isWin)
		{
			_successTitle.text = isWin ? _winTitle :_loseTitle;
			_successDescription.text = isWin ?  _winDescription:_loseDescription;
		}
		
		/////////////////////
		/// Button Events ///
		/////////////////////
		
		public void ButtonEvt_Enter()
		{
			if (_isOver) return;
			if (_wordLetterIndex != _wordLength)
				return;

			if (!_resourceManager.LegalWords.Contains(_currentWord))
			{
				_toastManager.ShowToast(RVDToast.WrongWord);
				return;
			}

			/*var checkIndex = _currentWordIndex * _wordLength;
			var correctCounter = 0;
			for (var i = 0; i < _wordLength; i++)
			{
				var list = "fjdsklf".ToList();
				var letter = _letterBoxes[checkIndex].Letter;
				if (_wordToGuess[i].ToString() == letter)
				{
					// for share
					_shareAnswerString += _correctCharacter;
					_safeAnswerString+=2;
					
					_letterBoxes[checkIndex].UpdateBox(MRVEnum.LetterState.Correct);
					_keyboardDict[letter].color = _colors[0];
					correctCounter++;
				}
				else if (_wordToGuess.Contains(letter))
				{
					// for share
					_shareAnswerString += _almostCharacter;
					_safeAnswerString+=1;
					
					_letterBoxes[checkIndex].UpdateBox(MRVEnum.LetterState.Close);
					if (_keyboardDict[letter].color != _colors[0])
						_keyboardDict[letter].color = _colors[1];
				}
				else
				{
					// for share
					_shareAnswerString += _missCharacter;
					_safeAnswerString+=0;
					
					_letterBoxes[checkIndex].UpdateBox(MRVEnum.LetterState.Wrong);
					_keyboardDict[letter].color = _colors[2];
				}
				checkIndex++;
			}*/

			var isComplete = CheckAnswer();
			
			if (isComplete)
			{
				AddWordToCorrectWordList();
				//AddWordKey();
				//UpdatePlayerStats(true);
				UpdateSuccessText(true);
				Evt_OnSuccess();
				return;
			}
			
			// for share
			_shareAnswerString += "\n";

			if (_currentLetterIndex == _totalLetters)
			{
				//UpdatePlayerStats(false);
				UpdateSuccessText(false);
				Evt_OnSuccess();
				Debug.Log("The game is lost");
			}
			
			_currentWordIndex++;
			_wordLetterIndex = 0;
			_currentWord = "";
		}
		
		public void ButtonEvt_Delete()
		{
			if (_isOver) return;
			if (_wordLetterIndex == 0)
				return;
			_currentLetterIndex--;
			_wordLetterIndex--;
			_currentWord = _currentWord.Substring(0, _currentWord.Length - 1);
			_letterBoxes[_currentLetterIndex].SetLetter("");
		}

		public void ButtonEvt_Menu()
		{
			_menuController.TransitionOn();
		}
		
		public void ButtonEvt_SelectAesthetics(int index)
		{
			PlayerPrefs.SetInt(AESTHETIC_KEY, index);
			PrepareAesthetics(index);
			if (!string.IsNullOrEmpty(_wordToGuess))
				_menuController.TransitionOff();
			else
				_toastManager.ShowToast(RVDToast.ChangeTheme);
		}
		
		public void ButtonEvt_EnterKey(string letter)
		{
			if (_isOver) return;
			if (_wordLetterIndex == _wordLength)
				return;
			Evt_UpdateKey(letter);
		}
		
				
		public void ButtonEvt_OpenDYKLink()
		{
#if UNITY_EDITOR
			Application.OpenURL(_dykLink);
#elif UNITY_WEBGL
			OpenLink(_dykLink);
#endif
		}

		public void ButtonEvt_OpenShareLink()
		{
			var index = ((_currentWordIndex + 1) > 6) ? "X" : (_currentWordIndex + 1).ToString();
#if UNITY_EDITOR
			GUIUtility.systemCopyBuffer = DateTime.Now.Month + "/" + DateTime.Now.Day + " Revedle " + index + "/6\n" + _shareAnswerString + "https://thelamgoesmoo.github.io/revedle #RedVelvet #revedle";
#else
			Debug.Log(_shareAnswerString);
			var date = DateTime.Today.Date;
			CopyToClipboard(date.Month, date.Day, _shareAnswerString, index);
			//_shareString += DateTime.Now.Month + "/" + DateTime.Now.Day + " Revedle " + (_currentWordIndex +1) + "/6. Play at https://thelamgoesmoo.github.io/revedle #RedVelvet #revedle";
			//Application.OpenURL(_shareString);
#endif
			_toastManager.ShowToast(RVDToast.CopyToClipboard);
		}
		
		public void ButtonEvt_ReturnToMenu()
		{
			_successScreen.TransitionOff();
			_menuController.TransitionOn();
		}

		public void ButtonEvt_SetWord(int index)
		{
			_wordIndexInList = index;
			PrepareWord(_resourceManager.WordEntryController.JsonContent.List[index]);
			_menuController.TransitionOff();
		}

	}
}