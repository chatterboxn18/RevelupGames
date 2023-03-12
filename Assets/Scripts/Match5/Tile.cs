using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
	private bool _canPop;
	public Vector2 Position => _position;
	private Vector2 _position;
	[SerializeField] private TileContent _tileContent;
	public void SetTile(int x, int y, ContentData contentData)
	{
		_position = new Vector2(x, y);
		_tileContent.SetContent(contentData);
	}

	public void SetContent(TileContent tileContent)
	{
		_tileContent = tileContent;
	}
}
