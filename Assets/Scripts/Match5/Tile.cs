using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
	private bool _canPop;
	public Vector2 Position => _position;
	private Vector2 _position;
	[SerializeField] private TileContent _tileContent;
	[SerializeField] private SimpleButton _simpleButton;
	private Vector2 _startPosition = Vector2.zero;
	private const float _minimumDelta = 0.02f;
	private bool _isSelected;
	private enum Movement
	{
		Up,
		Down, 
		Left, 
		Right, 
		None
	}
	
	public void SetTile(int x, int y, ContentData contentData)
	{
		_position = new Vector2(x, y);
		_tileContent.SetContent(contentData);
		_simpleButton.Evt_PointerDownEvent += OnPressed;
		_simpleButton.Evt_BasicEvent_Up += OnUp;
	}

	private void Update()
	{
		if (_isSelected)
			CheckTileMovement(Input.mousePosition);
	}

	private void CheckTileMovement(Vector2 inputPosition)
	{
		var greatestDelta = 0f; 
		var movementType = Movement.None;
		if (inputPosition.x - _startPosition.x >= _minimumDelta)
		{
			greatestDelta = inputPosition.x - _startPosition.x;
			movementType = Movement.Right;
		}

		if (inputPosition.y - _startPosition.y >= _minimumDelta)
		{
			if (inputPosition.y - _startPosition.y > greatestDelta)
			{
				greatestDelta = inputPosition.y - _startPosition.y;
				movementType = Movement.Up;
			}
		}
			
		if (inputPosition.y - _startPosition.y >= (-1) * _minimumDelta)
		{
			if (Mathf.Abs(inputPosition.y - _startPosition.y) > greatestDelta)
			{
				greatestDelta = Mathf.Abs( inputPosition.y - _startPosition.y);
				movementType = Movement.Down;
			}
		}
			
		if (inputPosition.x - _startPosition.x >= (-1) * _minimumDelta)
		{
			if (Mathf.Abs(inputPosition.x - _startPosition.x) > greatestDelta)
			{
				greatestDelta = Mathf.Abs( inputPosition.x - _startPosition.x);
				movementType = Movement.Left;
			}
		}

		if (movementType != Movement.None)
		{
			Debug.Log(movementType);
			_isSelected = false;
		}
	}
	
	private void OnPressed(PointerEventData inputPosition)
	{
		if (!_isSelected)
		{
			_isSelected = true;
			_startPosition = inputPosition.position;
			Debug.Log("Start position: " + _startPosition);
		}
	}

	private void OnUp()
	{
		_startPosition = Vector2.zero;
		_isSelected = false;
	}

	public void SetContent(TileContent tileContent)
	{
		_tileContent = tileContent;
	}
}
