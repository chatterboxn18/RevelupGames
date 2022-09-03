using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NativeGalleryController : MonoBehaviour
{
	public void FromGalleryToImage(Image imageTexture, bool isNativeSize = false, float containerSize = -1)
	{
		NativeGallery.GetImageFromGallery((media) =>
		{
			var image = NativeGallery.LoadImageAtPath(media);
			imageTexture.sprite = Sprite.Create(image,new Rect(Vector2.zero, new Vector2(image.width, image.height)), Vector2.zero );
			if (containerSize != -1)
			{
				var ratio = image.height * containerSize / image.width;
				imageTexture.rectTransform.sizeDelta = new Vector2(containerSize, ratio);
			}
			//imageTexture.preserveAspect = true;
			if (isNativeSize) imageTexture.SetNativeSize();
		});
	}
	
	
}
