using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchBoxes : MonoBehaviour
{
	[SerializeField] private BoxItem _blockPrefab;
	private List<BoxItem> _blockTransforms = new List<BoxItem>();
	private List<int> _blocks = new List<int>();


	private float _blockHeight = 310f;
	
	[SerializeField] private Transform _groupBlocks;

	[SerializeField] private Text _score; 

	private int _spawnRate = 30;

	private int _blocksChoppped = 0;

	private bool _isGameOver;
	private bool _isStarted;

	[Header("Block Control")] [SerializeField]
	private Transform _behindParent;


	[Header("Timer")] [SerializeField] private Image _timerFill;
	private float _gameTimer = 10f;
	private float _loseTime = 10f;
	private float _timeIncrease = 0.25f;

	[Header("GameOver")]
	[SerializeField] private GameObject _gameOverPanel;
	[SerializeField] private Text _gameOverText;
	[SerializeField] private Text _encouragementText;
	[SerializeField] private Sprite _sadGi;
	[SerializeField] private Sprite _happyGi;
	[SerializeField] private Image _seulgi;
	private const string _successText = "I KNEW YOU COULD DO IT! \nYOU'RE INCREDIBLE!";
	private const string _lostText = "BUT YOU'RE STILL AMAZING \nSEULGI YOU CAN DO IT";

	[Header("StartPanel")] [SerializeField]
	private GameObject _menuPanel;


	public void ButtonEvt_Start()
	{
		_menuPanel.gameObject.SetActive(false);
		_isStarted = true;
	}

	public enum Blocks
	{
		Triangle = 0, 
		Square = 1, 
		Circle =2
	}

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
		_blocksChoppped = 0;
		_score.text = "0";
		_isGameOver = false;
		AddBlocks();
		_gameOverPanel.SetActive(false);
	}

	private void GameOver()
	{
		StopAllCoroutines();
		_isGameOver = true;
		_gameOverText.text = _blocksChoppped.ToString();
		_encouragementText.text = _blocksChoppped > 2 ? _successText : _lostText;
		_seulgi.sprite = _blocksChoppped > 2 ? _happyGi : _sadGi;
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
		for (var i = 0; i < _spawnRate; i++)
		{
			CreateBlock(i);
		}
	}

	private void CreateBlock(int item)
	{
		var index = Random.Range(0, 3);
		_blocks.Add(index);
		var block = Instantiate(_blockPrefab, _groupBlocks);
		block.SetImage(index);
		block.transform.localPosition = new Vector3(0, _blockHeight * item, 0);
		_blockTransforms.Add(block);
	}

	private IEnumerator ResetBlock(BoxItem block)
	{
		var index = Random.Range(0, 3);
		yield return new WaitForSeconds(1f);
		block.SetImage(index);
		block.transform.SetParent(_groupBlocks);
		block.Rigidbody2D.isKinematic = true;
		block.Rigidbody2D.velocity = Vector3.zero;
		_blocks.Add(index);
		block.transform.localPosition = new Vector3(0, _blockHeight * _blockTransforms.Count, 0);
		_blockTransforms.Add(block);
	}

	public void ButtonEvt_Triangle()
	{
		if (_blocks[0] != 0)
		{
			GameOver();
		}
		else
		{
			_gameTimer += _timeIncrease;
			ChopBlock(0);
			_blocksChoppped++;
			_score.text = _blocksChoppped.ToString();
		}
	}
	
	public void ButtonEvt_Square()
	{
		if (_blocks[0] != 1)
		{
			GameOver();
		}
		else
		{
			_gameTimer += _timeIncrease;
			ChopBlock(1);
			_blocksChoppped++;
			_score.text = _blocksChoppped.ToString();
		}
	}

	public void ButtonEvt_Circle()
	{
		if (_blocks[0] != 2)
		{
			GameOver();
		}
		else
		{
			_gameTimer += _timeIncrease;
			ChopBlock(2);
			_blocksChoppped++;
			_score.text = _blocksChoppped.ToString();
		}
	}
	
	private void Update()
	{
		if (_isGameOver || !_isStarted)
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
			ButtonEvt_Triangle();
			return;
		}
		
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			ButtonEvt_Square();
			return;
		}


		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			ButtonEvt_Circle();
			return;
		}


	}

	private void ChopBlock(int type)
	{
		_blockTransforms[0].transform.SetParent(_behindParent);
		_blockTransforms[0].Rigidbody2D.isKinematic = false;
		switch ((Blocks) type)
		{
			case Blocks.Triangle:
				_blockTransforms[0].Rigidbody2D.AddForce(new Vector2(-500, 100), ForceMode2D.Impulse);
				break;
			case Blocks.Square:
				_blockTransforms[0].Rigidbody2D.AddForce(new Vector2(0,100), ForceMode2D.Impulse);
				break;
			case Blocks.Circle:
				_blockTransforms[0].Rigidbody2D.AddForce(new Vector2(500,100), ForceMode2D.Impulse);
				break;
		}
		_blocks.RemoveAt(0);
		var rb = _blockTransforms[0];
		_blockTransforms.RemoveAt(0);
		StartCoroutine(ResetBlock(rb));
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
