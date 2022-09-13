using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using FileMode = UnityEditor.VersionControl.FileMode;

namespace DungeonQuest
{
	public class DQDebugCreator : MonoBehaviour
	{
		[SerializeField] private Image _cardPhoto;
		[SerializeField] private RectTransform _cardPhotoSize;
		[SerializeField] private NativeGalleryController _nativeGallery;
		[SerializeField] private Image _cardBackground; 

		//inputs 
		[SerializeField] private TMP_InputField _actionInput;
		[SerializeField] private TMP_InputField _actionGrowthInput;
		[SerializeField] private TMP_InputField _startPriceInput;
		[SerializeField] private TMP_InputField _startInflactionInput;
		
		// sprite defaults 700 x 400

		// Save variables. Will not save without button click. 
		private List<DQCharacterData.Card> _cardList = new List<DQCharacterData.Card>();
		private DQCharacterData.Card _currentCard;
		private int _cardSpriteIndex = 0; //to help define each sprite 
		private Image _currentSprite; 

		private IEnumerator Start()
		{
			yield return null;
		}

		private void CreateSpritesForCard(Texture2D tex, string name)
		{
			//tex = new Texture2D(700, 400,GraphicsFormat.R8G8B8_UInt, TextureCreationFlags.None);
			byte[] bytes = tex.EncodeToPNG();
			FileStream stream = new FileStream("Assets/2D/DungeonQuest/Sprites/" + name + ".png", System.IO.FileMode.Create, FileAccess.Write);
			BinaryWriter writer = new BinaryWriter(stream);
			for (int i = 0; i < bytes.Length; i++) {
				writer.Write(bytes[i]);
			}
			writer.Close();
			stream.Close();
		}
		
		public void ButtonEvt_OpenGallery()
		{
			_nativeGallery.FromGalleryToImage(_cardPhoto, false, _cardPhotoSize.rect.width);
		}

		public void ButtonEvt_SelectCharacter(int index)
		{
			var character = (DQCharacterData.RedVelvet)index;
			_cardBackground.color = DQResourceManager.Colors[character];
			if (_currentCard == null)
			{
				_currentCard = ScriptableObject.CreateInstance<DQCharacterData.Card>();
				_currentCard.Character = DQCharacterData.Characters[character];
				return;
			}

			_currentCard.Character = DQCharacterData.Characters[character];
		}

		public void ButtonEvt_SaveToCard()
		{
			if (_currentCard == null)
				_currentCard = ScriptableObject.CreateInstance<DQCharacterData.Card>();
			
			_currentCard.Sprites.Add(Sprite.Create(_cardPhoto.sprite.texture, new Rect(new Vector2(0,0),new Vector2(700f,400f)), Vector2.zero));
			
			if (!string.IsNullOrEmpty(_actionInput.text))
			{
				_currentCard.Action = float.Parse(_actionInput.text);; 
			}

			if (!string.IsNullOrEmpty(_actionGrowthInput.text))
			{
				_currentCard.ActionGrowth = float.Parse(_actionInput.text);
			}

			if (!string.IsNullOrEmpty(_startPriceInput.text))
			{
				_currentCard.StartPrice = int.Parse(_startPriceInput.text);
			}

			if (!string.IsNullOrEmpty(_startInflactionInput.text))
			{
				_currentCard.PriceInflation = float.Parse(_startInflactionInput.text);
			}
		}
		
		private void SetPictureToPhotoSize()
		{
			
		}
	}
}