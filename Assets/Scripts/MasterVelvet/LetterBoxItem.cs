using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MasterVelvet
{
	
	public class LetterBoxItem : MonoBehaviour
	{
		[SerializeField] private Image _imageFill; 
		[SerializeField] private TextMeshProUGUI _letterText;
		public string Letter => _letterText.text;
		[SerializeField] private List<Color> _colors;

		public void UpdateBox(MRVEnum.LetterState state, float time = 0.2f)
		{
			switch (state)
			{
				case MRVEnum.LetterState.Correct:
					
					LeanTween.value(_imageFill.gameObject, setColorCallback, Color.white,  _colors[0], time);
					//_imageFill.color = _colors[0];
					break;
				case MRVEnum.LetterState.Close:
					LeanTween.value(_imageFill.gameObject, setColorCallback, Color.white,  _colors[1], time);
					//_imageFill.color = _colors[1];
					break;
				case MRVEnum.LetterState.Wrong:
					LeanTween.value(_imageFill.gameObject, setColorCallback, Color.white,  _colors[2], time);
					//_imageFill.color = _colors[2];
					break;
			}

			_letterText.color = Color.white;
		}

		private void setColorCallback( Color c )
		{
			_imageFill.color = c;
 
			var tempColor = _imageFill.color;
			tempColor.a = 1f;
			_imageFill.color = tempColor;
		}

		public void SetLetter(string letter)
		{
			_letterText.text = letter;
		}
	}
}