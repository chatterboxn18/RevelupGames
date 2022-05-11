using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchCardGame : MonoBehaviour
{
	public enum Value
	{
		Pink =0, 
		Yellow = 1, 
		Blue =2, 
		Green =3, 
		Purple = 4
	}
	
	private int _rows;
	private int _columns;
	private int _matchNumber = 2;
	private int _level = 1;
	private int _numColors = 5;
	[SerializeField] private MatchCard _cardPrefab;
	private List<Value> _matchList = new List<Value>();

	private float _timer = 0f;
	[SerializeField] private Image _timerImage;
	private float _totalTime = 5f;
	private bool _isGameEnd;

	[SerializeField] private GridLayoutGroup _gridLayout;

	private void SetUp()
	{
		_columns = 5;
		_rows = 4;
		var size = _gridLayout.GetComponent<RectTransform>().rect.size;
		var width = Mathf.RoundToInt(size.x / _columns);
		var height = Mathf.RoundToInt(size.y / _rows);
		SetList();
		_gridLayout.cellSize = new Vector2(width, height);
		for (var i = 0; i < _rows; i++)
		{
			for (var j = 0; j < _columns; j++)
			{
				var card = Instantiate(_cardPrefab);
				//card.Setup();
			}
		}
	}

	private void SetList()
	{
		var total = _rows * _columns;
		var dict = new Dictionary<Value, int>();
		for (var i =0 ; i < _numColors; i++)
		{
			dict.Add((Value) i, 0);
		}

		while (dict.Count > 0)
		{
			var random = Random.Range(0, dict.Count);
			var keys = dict.Keys.ToArray();
			var value = keys[random];
			dict[value]++;
			_matchList.Add(value);
			if (dict[value] >= 4)
			{
				dict.Remove(value);
			}
			
		}
	}

	private void Update()
	{
		if (_timer >= _totalTime)
		{
			_isGameEnd = true;
			
			// game over
		}
		if (!_isGameEnd)
		{
			_timer += Time.deltaTime;
			_timerImage.fillAmount = _timer / _totalTime;
		}
	}

	public void Evt_MakeMatch()
	{
		_totalTime += 0.5f; // add half a second when make successful match
	}
}
