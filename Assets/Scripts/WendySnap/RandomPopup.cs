using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomPopup : MonoBehaviour
{
	[SerializeField] private List<Wendy> Wendys = new List<Wendy>();


	private List<Image> _wendyImages = new List<Image>();
	[SerializeField] private Image _prefab;

	private float _timer;
	[SerializeField] private float _spawnTime = 10f;
	
	[Serializable]
	private class Wendy
	{
		public Sprite Sprite;
		public Vector2 Position;
		public Vector2 Movement;
	}

	private void Start()
	{
		foreach (var wendy in Wendys)
		{
			var wannie = Instantiate(_prefab, transform);
			wannie.sprite = wendy.Sprite;
			wannie.rectTransform.anchoredPosition = wendy.Position;
			_wendyImages.Add(wannie);
		}
	}

	private IEnumerator SpawnWannie()
	{
		var index = Random.Range(0, _wendyImages.Count);
		var time = 0f;
		var total = 1f;
		var positionStart = Wendys[index].Position;
		var positionEnd = Wendys[index].Movement;
		while (time < total)
		{
			_wendyImages[0].rectTransform.anchoredPosition = Vector2.Lerp(positionStart, positionEnd, time/total);
			time += Time.deltaTime;
			yield return null;
		}
		_wendyImages[0].rectTransform.anchoredPosition = positionEnd;
		yield return new WaitForSeconds(2f);
		time = 0;
		while (time < total)
		{
			_wendyImages[0].rectTransform.anchoredPosition = Vector2.Lerp(positionEnd, positionStart,time/total);
			time += Time.deltaTime;
			yield return null;
		}
		_wendyImages[0].rectTransform.anchoredPosition = positionStart;
	}

	private void Update()
	{
		if (_timer > _spawnTime)
		{
			StartCoroutine(SpawnWannie());
			_timer = 0f;
			return;
		}

		_timer += Time.deltaTime;
	}
}