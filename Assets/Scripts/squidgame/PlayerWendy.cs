using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWendy : MonoBehaviour
{
	public Image Sprite;
	[SerializeField] private Sprite _victorySprite;
	[SerializeField] private Vector2 _midPosition;
	[SerializeField] private Vector2 _endPosition;

	public float Speed = 1f;

	public RectTransform rectTransform;
	private Coroutine _currentCoroutine;
	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void Evt_Move()
	{
		_currentCoroutine = StartCoroutine(MoveForward());
	}
	
	public void Evt_Complete(Action onComplete)
	{
		StopCoroutine(_currentCoroutine);
		StartCoroutine(LerpToEnd(onComplete));
	}

	private IEnumerator MoveForward()
	{
		var currentPosition = rectTransform.anchoredPosition;
		var timer = 0f;
		while (timer < Speed)
		{
			rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, _midPosition, timer/Speed);
			timer += Time.deltaTime;
			yield return null;
		}
	}
	
	private IEnumerator LerpToEnd(Action onComplete)
	{
		Sprite.sprite = _victorySprite;
		var currentPosition = rectTransform.anchoredPosition;
		var endPosition = currentPosition + _endPosition;
		var timer = 0f;
		while (timer < Speed)
		{
			rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, endPosition, timer/Speed);
			timer += Time.deltaTime;
			yield return null;
		}

		rectTransform.anchoredPosition = Vector2.zero;
		onComplete();
	}
}
