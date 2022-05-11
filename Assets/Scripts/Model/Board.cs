using UnityEngine;

namespace SGChess
{
	public class Board : MonoBehaviour
	{
		public enum ChessClass
		{
			King,
			Queen,
			Rook,
			Bishop,
			Knight,
			Pawn
		}

		public class Tile
		{
			public int Value;
			public ChessClass Class;
			public int IsBlack;
		}

		private Grid<Tile> _currentGrid;
		
		private void Start()
		{
			_currentGrid = new Grid<Tile>(8, 8, 300);
			for (var i = 0; i < _currentGrid.GridObjects.GetLength(0); i++)
			{
				for (var j = 0; j < _currentGrid.GridObjects.GetLength(1); j++)
				{
					Debug.Log("The value for " + i + "," + j + ":" + _currentGrid.GridObjects[i, j].Value);
				}
			}
		}
	}
}