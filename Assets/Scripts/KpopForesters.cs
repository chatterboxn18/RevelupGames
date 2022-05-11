using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KpopForesters : MonoBehaviour
{
	[SerializeField] private Sprite[] _neutralSprites;
	[SerializeField] private Sprite[] _stabSprites;
	[SerializeField] private Rigidbody2D[] _blockPrefabs;
	private List<Rigidbody2D> _blockTransforms = new List<Rigidbody2D>();
	private List<int> _blocks = new List<int>();

	[SerializeField] private Image _characterImage;
	[SerializeField] private Vector2 _leftLocation;
	[SerializeField] private Vector2 _rightLocation;

	private float _blockHeight = 310f;
	
	[SerializeField] private Transform _groupBlocks;

	[SerializeField] private Text _score; 
	
	private int _spriteCounter;
	private bool _isLeft = true;
	private int _timer;
	[SerializeField] private int _frameRate;

	private int _spawnRate = 15;

	private int _blocksChoppped = 0;

	private bool _isNeutral;
	private int _neutralCounter = 0;
	private bool _isGameOver;

	[Header("Timer")] [SerializeField] private Image _timerFill;
	private float _gameTimer = 10f;
	private float _loseTime = 10f;
	private float _timeIncrease = 0.25f;

	[Header("GameOver")]
	[SerializeField] private GameObject _gameOverPanel;

	[SerializeField] private Text _gameOverText;


	private void Awake()
	{
	}

	private void Start()
	{
		AddBlocks();
	}

	private void Reset()
	{
		_gameTimer = _loseTime;
		_isNeutral = false;
		_characterImage.transform.localPosition = _leftLocation;
		_blocksChoppped = 0;
		_score.text = "0";
		_isGameOver = false;
		AddBlocks();
		_gameOverPanel.SetActive(false);
	}

	private void GameOver()
	{
		_isGameOver = true;
		_gameOverText.text = _blocksChoppped.ToString();
		_gameOverPanel.SetActive(true);
		
		_blocks.Clear();
		for (var i = 0; i < _groupBlocks.childCount; i++)
		{
			Destroy(_groupBlocks.GetChild(i).gameObject);
		}
		_blockTransforms.Clear();
	}

	private void AddBlocks()
	{
		//first block 
		_blocks.Add(2);
		var block = Instantiate(_blockPrefabs[2], _groupBlocks);
		block.transform.localPosition = new Vector3(0, 0, 0);
		_blockTransforms.Add(block.GetComponent<Rigidbody2D>());
		
		
		for (var i = 1; i < _spawnRate; i++)
		{
			if (i < 3)
			{
				_isNeutral = true;
			}
			CreateBlock(i);
		}
	}

	private void CreateBlock(int number)
	{
		var index = _isNeutral ? 2 : Random.Range(0, 3);
		if (index == 2)
			_neutralCounter++;
		else
			_neutralCounter = 0;
		if (_neutralCounter > 3)
		{
			_neutralCounter = 0;
			index = Random.Range(0, 2);
		}
		_blocks.Add(index);
		var block = Instantiate(_blockPrefabs[index], _groupBlocks);
		block.transform.localPosition = new Vector3(0, _blockHeight * number, 0);
		_blockTransforms.Add(block);
		_isNeutral = !_isNeutral;
	}

	public void LeftChop()
	{
		_isLeft = true;
		_characterImage.transform.localPosition = _leftLocation;
		_characterImage.sprite = _neutralSprites[0];
		if (_blocks[1] == 0)
		{
			GameOver();
		}
		else
		{
			_gameTimer += _timeIncrease;
			ChopBlock(false);
			_characterImage.sprite = _stabSprites[0];
			_blocksChoppped++;
			_score.text = _blocksChoppped.ToString();
		}
	}

	public void RightChop()
	{
		_isLeft = false;
		_characterImage.sprite = _neutralSprites[1];
		_characterImage.transform.localPosition = _rightLocation;
		if (_blocks[1] == 1)
		{
			GameOver();
		}
		else
		{
			_gameTimer += _timeIncrease;
			ChopBlock(true);
			_characterImage.sprite = _stabSprites[1];
			_blocksChoppped++;
			_score.text = _blocksChoppped.ToString();
		}
	}
	
	private void Update()
	{
		if (_isGameOver)
			return;


		if (_gameTimer <= 0)
		{
			GameOver();
			return;
		}
		_timerFill.fillAmount = _gameTimer / _loseTime;
		_gameTimer -= Time.deltaTime;
		
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			LeftChop();
			return;
		}

		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			RightChop();
			return;
		}

		if (_timer >= _frameRate)
		{
			_characterImage.sprite = _neutralSprites[_isLeft ? _spriteCounter%2 : _spriteCounter%2+2];
			_spriteCounter++;
			_timer = 0;
		}

		_timer++;

	}

	private void ChopBlock(bool isRight)
	{
		_blockTransforms[0].isKinematic = false;
		_blockTransforms[0].AddForce(isRight ? new Vector2(500,300) : new Vector2(-500, 300), ForceMode2D.Impulse);
		_blocks.RemoveAt(0);
		_blockTransforms.RemoveAt(0);
		CreateBlock(_spawnRate);
		foreach (var block in _blockTransforms)
		{
			var position = block.transform.localPosition;
			block.transform.localPosition = new Vector3(position.x, position.y - _blockHeight);
		}
	}

	public void ButtonEvt_Reset()
	{
		Reset();
	}
}
