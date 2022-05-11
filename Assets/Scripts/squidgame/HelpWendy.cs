using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HelpWendy : MonoBehaviour
{
	private List<int> _indexes = new List<int>();

	private int _initialSpawnNumber = 20;

	[SerializeField] private List<Sprite> _wendySprites;
	[SerializeField] private PlayerWendy _wendyCharacter;
	[SerializeField] private Transform _spawnCharacter;
	private List<PlayerWendy> _wendies = new List<PlayerWendy>();

	[SerializeField] private RectTransform _seulgiTransform;
	[SerializeField] private Image _expression;

	private float _speed = 0.3f;

	[SerializeField] private Image _timerFill;
	private float _totalTime = 30f; //60 seconds
	private float _timer = 0f;


	private bool _isGameOver = false;
	[SerializeField] private Transform _gameOverPanel;

	[SerializeField] private Text _cheeredText;
	[SerializeField] private Text _savedText;

	private int _cheered;
	private int _saved;

	private void Start()
	{
		SpawnWendies();
	}

	public void ButtonEvt_Restart()
	{
		_spawnCharacter.DestroyChildren();
		_timer = 0f;
		_timerFill.fillAmount = 1;
		_indexes.Clear();
		_wendies.Clear();
		_cheered = 0;
		_saved = 0;
		
		SpawnWendies();
		_gameOverPanel.gameObject.SetActive(false);
		_isGameOver = false;
	}
	
	private void SpawnWendies()
	{
		for (var i = 0; i < _initialSpawnNumber; i++)
		{
			var wendy = Instantiate(_wendyCharacter, _spawnCharacter);
			var index = Random.Range(0, 2);
			_indexes.Add(index);
			wendy.Sprite.sprite = _wendySprites[index];
			_wendies.Add(wendy);
		}
		_wendies[0].Evt_Move();
	}

	public void ButtonEvt_Help(int index)
	{
		if (index == _indexes[0])
		{
			if (index == 1)
			{
				_saved++;
				StartCoroutine(MoveSeulgi(_wendies[0].rectTransform.position, ChangeWendy));
			}
			else
			{
				_cheered++;
				StartCoroutine(Express());
				ChangeWendy();				
			}
		}
		else
		{
			Evt_GameOver();
		}
	}

	private void Update()
	{
		if (_isGameOver) return;

		if (_timer >= _totalTime)
		{
			_timer = 0;
			_isGameOver = true;
			Evt_GameOver();
		}

		_timerFill.fillAmount = 1 - (_timer / _totalTime);
		_timer += Time.deltaTime;
		
	}


	private void Evt_GameOver()
	{
		_cheeredText.text = _cheered + "";
		_savedText.text = _saved+ "";
		_gameOverPanel.gameObject.SetActive(true);
	}
	private void ChangeWendy()
	{
		var previousWendy = _wendies[0];
		_wendies.RemoveAt(0);
		_wendies.Add(previousWendy);
		_indexes.RemoveAt(0);
		var newIndex = Random.Range(0, 2);
		_indexes.Add(newIndex);
		previousWendy.transform.SetAsLastSibling();
		previousWendy.Evt_Complete(() => { previousWendy.Sprite.sprite = _wendySprites[newIndex];});
		_wendies[0].Evt_Move();
	}

	private IEnumerator Express()
	{
		_expression.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.2f);
		_expression.gameObject.SetActive(false);
	}
	
	private IEnumerator MoveSeulgi(Vector2 toPosition, Action onMid)
	{
		var currentPosition = _seulgiTransform.position;
		var timer = 0f;
		while (timer < _speed)
		{
			_seulgiTransform.position = Vector2.Lerp(currentPosition, toPosition , timer/_speed);
			timer += Time.deltaTime;
			yield return null;
		}
		//_expression.gameObject.SetActive(true);
		onMid();
		timer = 0f; 
		
		while (timer < _speed)
		{
			_seulgiTransform.position = Vector2.Lerp(toPosition, currentPosition, timer/_speed);
			timer += Time.deltaTime;
			yield return null;
		}

		_seulgiTransform.position = currentPosition;
		//_expression.gameObject.SetActive(false);
		
		yield return null;
	}
}
