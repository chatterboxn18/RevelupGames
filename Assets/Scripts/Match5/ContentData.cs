using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ContentData 
{
	public ContentData Data
	{
		get
		{
			if (_contentData == null)
				_contentData = new ContentData();
			return _contentData;
		}
	}

	public string ContentName => _contentName;
	public Sprite ContentSprite => _contentSprite;
	private ContentData _contentData;
	[SerializeField] private Sprite _contentSprite;
	[SerializeField] private string _contentName;

}
