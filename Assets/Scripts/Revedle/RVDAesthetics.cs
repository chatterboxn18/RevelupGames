using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Revedle
{
	public class RVDAesthetics : MonoBehaviour
	{
		public enum Days
		{
			Day1 = 0,
			Day2 = 1,
			Finale = 2,
			TwentyTwo = 3
		}

		[SerializeField] private List<DayAsthetic> _dayAsthetics = new List<DayAsthetic>();
		public List<DayAsthetic> DayAsthetics => _dayAsthetics;
		[Serializable]
		public struct DayAsthetic
		{
			public Color Background;
			public string Title;
			public Color TitleColor;
			public TMP_FontAsset FontAsset;
			public Sprite TitleImage;
			public Color KeyColor;
		}
	}
}