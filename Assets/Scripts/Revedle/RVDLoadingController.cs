using System.Collections;
using System.Collections.Generic;
using DungeonQuest;
using UnityEngine;
using UnityEngine.Android;

namespace Revedle
{
	public class RVDLoadingController : MonoBehaviour
	{
		private Animator _animator;
		private bool _isLoaded;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}
		private void Update()
		{
			if (_isLoaded)
				return;
			if (RVDResourceManager.IsReady)
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