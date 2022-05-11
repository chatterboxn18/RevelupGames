using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Revedle
{
	public class RVDBuilder : MonoBehaviour
	{
		[SerializeField] private TMP_InputField _wordInput;
		[SerializeField] private TMP_InputField _descriptionInput;
		[SerializeField] private TMP_InputField _linkInput;
		[SerializeField] private SimpleButtonAssign _buttonAssign;
		[SerializeField] private Transform _buttonParent; 
		[SerializeField] private RVDResourceManager _resourceManager;

		private int _currentIndex;
		
		private IEnumerator Start()
		{
			while (!RVDResourceManager.IsReady) yield return null;
			var index = 0;
			foreach (var sprite in _resourceManager.AlbumCovers)
			{
				var button = Instantiate(_buttonAssign, _buttonParent);
				button.SetImage(sprite, index.ToString());
				var currentIndex = index;
				button.GetComponent<SimpleButton>().Evt_BasicEvent_Up += () =>
				{
					_currentIndex = currentIndex;
				};
				index++;
			}

			/*var list = _resourceManager.WordEntryController.JsonContent.List;
			Debug.Log("Item count: " + list.Count);
			var randomize = new List<int>();
			for (var i = 0; i < list.Count; i++)
			{
				randomize.Add(i);
			}

			var newList = new List<RVDResourceManager.WordEntry>();
			randomize = randomize.Randomize();
			foreach (var inx in randomize)
			{
				newList.Add(list[inx]);
			}

			var word = new RVDResourceManager.Words()
			{
				List = newList
			};
			_resourceManager.WordEntryController.UpdateJsonContent(word, "Revedle/wordGuessList.json");*/
		}

		public void ButtonEvt_Create()
		{
			if (string.IsNullOrEmpty(_wordInput.text))
			{
				return;
			}

			var wordList = _resourceManager.WordEntryController.JsonContent;

			var word = new RVDResourceManager.WordEntry()
			{
				Word = _wordInput.text,
				Description = _descriptionInput.text,
				Link =  _linkInput.text,
				SpriteIndex = _currentIndex
			};
			wordList.List.Add(word);
			_resourceManager.WordEntryController.UpdateJsonContent(wordList, "Revedle/wordGuessList.json");
		}
	}
}