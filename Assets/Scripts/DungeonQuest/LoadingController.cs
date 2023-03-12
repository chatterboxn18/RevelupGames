using System.Collections;
using System.Collections.Generic;
using DungeonQuest;
using UnityEngine;
using UnityEngine.Android;

namespace RevelupGames
{
	public class LoadingController : MonoBehaviour
	{
		private Animator _animator;
		private bool _isLoaded;

		[SerializeField]
		private bool _forceLoad;
		
		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (_isLoaded)
				return;
			if (_forceLoad || DQResourceManager.IsReady)
			{
				_animator.SetTrigger("DoFinish");
				_isLoaded = true;
			}
		}

		public void AnimEvt_CloseLoader()
		{
			gameObject.SetActive(false);
		}
	}
}