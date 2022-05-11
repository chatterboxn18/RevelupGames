using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCard : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Image _outline;
	private MatchCardGame.Value _value;
	private Vector2 _position = new Vector2();
	public Action<MatchCardGame.Value, MatchCard> Evt_Match;

	private bool _isSelected;

	public void Setup(MatchCardGame.Value value, int row, int column)
	{
		_value = value;
		_position = new Vector2() { x = row, y = column };
	}

	private void Reset()
	{
		_isSelected = false; 
		_outline.gameObject.SetActive(false);
	}

	public void ButtonEvt_Match()
	{
		if (!_isSelected)
		{
			Evt_Match(_value, this);
			_isSelected = true;
			Evt_Selected();
		}
	}

	private void Evt_Selected()
	{
		_outline.gameObject.SetActive(true);
	}
}
