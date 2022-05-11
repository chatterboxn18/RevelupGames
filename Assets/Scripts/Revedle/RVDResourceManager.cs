using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Revedle
{
	public class RVDResourceManager : ResourceHandler
	{
		[SerializeField] private bool _isBuilder;
		private string _path = "Revedle/wordGuessList.json";
		private string _fiveWordList = "Revedle/wordList.txt";
		private JSONDataController<Words> _jsonController;

		private string _thumbNailPath = "Revedle/revedle-thumbnails.png";
		public List<Sprite> AlbumCovers = new List<Sprite>();
		public JSONDataController<Words> WordEntryController => _jsonController;

		private List<string> _legalWords = new List<string>();
		public List<string> LegalWords => _legalWords;

		[Serializable]
		public class Words
		{
			public List<WordEntry> List = new List<WordEntry>();
		}

		[Serializable]
		public class WordEntry
		{
			public string Word;
			public string Description;
			public string Link;
			public int SpriteIndex;
		}

		private static bool _isReady;
		public static bool IsReady => _isReady; 
		
		private IEnumerator Start()
		{
			_jsonController = new JSONDataController<Words>();
			yield return GetJson(_path, (s =>
			{
				_jsonController.RetrieveJson(_path, s);
			}));
			yield return GetJson(_fiveWordList, s =>
			{
				_legalWords = s.Split(',').ToList();
				/*var index = 0;
				var currentWord = "";
				foreach (var letter in s)
				{
					if (index == 5)
					{
						_legalWords.Add(currentWord);
						index = 0;
						currentWord = "";
					}
					currentWord += letter;
					index++;
				}*/
			});
			
			yield return CutSprites(_thumbNailPath, 200f, 200f, list =>
			{
				AlbumCovers = list;
			});

			_isReady = true;
		}

		private IEnumerator GetJson(string localPath, Action<string> onComplete)
		{
			var request = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, localPath));
			yield return request.SendWebRequest();
			onComplete(request.downloadHandler.text);
		}
	}
}