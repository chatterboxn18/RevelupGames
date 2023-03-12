using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchBoard : MonoBehaviour
{
	[SerializeField] private Transform _parentBoard;
	[SerializeField] private Vector2 _boardSize;
	[SerializeField] private Tile _tilePrefab;
	[SerializeField] private List<ContentData> _tileDatabase = new List<ContentData>();
	public Tile[,] Board;
	private void Start()
	{
		Board = new Tile[(int)_boardSize.x, (int)_boardSize.y];
		for (var i = 0; i < _boardSize.x; i++)
		{
			for (var j = 0; j < _boardSize.y; j++)
			{
				var randomIndex = Random.Range(0, 5);
				var tile = Instantiate(_tilePrefab, _parentBoard);
				tile.SetTile(i, j, _tileDatabase[randomIndex]);
				tile.name = $"Tile {i}, {j}";
				Board[i, j] = tile;
			}
		}
	}
}
