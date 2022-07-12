using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DaySelectItem : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _dayText;
	[SerializeField] private GameObject _statText;
	private int _index;
	public Action<int> Evt_SelectWord = delegate(int i) {  };

	public void SetDay(string name, bool stat, int index)
	{
		_dayText.text = name;
		_statText.SetActive(stat);
		_index = index;
	}

	public void UpdateDay(bool stat)
	{
		_statText.SetActive(stat);
	}

	public void ButtonEvt_SelectWord()
	{
		Evt_SelectWord(_index);
	}
}
