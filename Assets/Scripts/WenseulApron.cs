using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WenseulApron : MonoBehaviour
{
	[SerializeField] private TimerBarTap _seulgiTimerTapGame;
	[SerializeField] private TimerBarTap _wendyTimerTapGame;

	[SerializeField]private GameObject _seulgiSuccess;
	[SerializeField]private GameObject _wendySuccess;

	[SerializeField] private SimpleButton _startButton;

	private void Awake()
	{
		_seulgiTimerTapGame.Evt_OnSuccess += () =>
		{
			_seulgiSuccess.SetActive(true);
			_wendyTimerTapGame.Evt_Start();
		};
		_wendyTimerTapGame.Evt_OnSuccess += () =>
		{
			_wendySuccess.SetActive(true);
		};
		_seulgiTimerTapGame.Evt_OnReset += () =>
		{
			_seulgiSuccess.SetActive(false);
		};
		_wendyTimerTapGame.Evt_OnReset += () =>
		{
			_wendySuccess.SetActive(false);
		};
	}

	public void ButtonEvt_Start()
	{
		_seulgiTimerTapGame.Evt_Start();
		_startButton.SetVisibility(false);
	}
	
	
}