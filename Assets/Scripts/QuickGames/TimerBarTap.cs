using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TimerBarTap : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	[SerializeField] private float _rangePercent = .15f;
	[SerializeField] private Image _tapAreaImage;
	[SerializeField] private Image _fullImage;
	private Vector2 _tapRange = Vector2.zero;
	[SerializeField] private Image _userTapArea;
	private float _timer;
	private bool _isRight;
	private bool _isStop;

	private float _topRange;
	private float _bottomRange;
	
	public Action Evt_OnSuccess = delegate {  };
	public Action Evt_OnReset = delegate {  };
	
	private void Start()
	{
		_isStop = true;
		RandomizeRange();	
	}

	public void Evt_Start()
	{
		_isStop = false;
	}

	private void RandomizeRange()
	{
		var rangeNumber = _rangePercent * _fullImage.rectTransform.rect.width;
		_tapAreaImage.rectTransform.sizeDelta = new Vector2(rangeNumber, _fullImage.rectTransform.rect.height);
		var maxRange = _fullImage.rectTransform.rect.width - rangeNumber;
		var randomRange = Random.Range(0, maxRange);
		_tapAreaImage.rectTransform.anchoredPosition = new Vector2(randomRange, 0);
		_bottomRange = randomRange;
		_topRange = randomRange + rangeNumber;
	}

	private void Update()
	{
		if (_isStop)
			return;
		if (_timer <= _speed)
		{
			if (_isRight)
			{
				_userTapArea.rectTransform.anchoredPosition =
					new Vector2(_timer / _speed * (_fullImage.rectTransform.rect.width-_userTapArea.rectTransform.rect.width), 0);
			}
			else
			{
				_userTapArea.rectTransform.anchoredPosition = 					
					new Vector2((1 -_timer / _speed) * (_fullImage.rectTransform.rect.width-_userTapArea.rectTransform.rect.width), 0);

			}

			_timer += Time.deltaTime;
		}
		else
		{
			_timer = 0;
			_isRight = !_isRight;
		}
	}

	private void Reset()
	{
		_userTapArea.rectTransform.anchoredPosition = Vector2.zero;
		_timer = 0;
		RandomizeRange();
		_isStop = false;
		Evt_OnReset();
	}

	public void ButtonEvt_Tap()
	{
		if (_isStop)
		{
			Reset(); 
			return;
		}
		_isStop = true;
		if (_userTapArea.rectTransform.anchoredPosition.x >= _bottomRange &&
		    _userTapArea.rectTransform.anchoredPosition.x <= _topRange)
		{
			Evt_OnSuccess();
			Debug.Log("Win");
		}
		else
		{
			Debug.Log("Lose");
		}
	}
}
