using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceHandler : MonoBehaviour
{
	private string _serverPath = "";

	protected IEnumerator GetTexture(string path, Action<bool, DownloadHandler> onComplete, bool downloaded = false)
	{
				
		var uriBuilder = new UriBuilder(Path.Combine(Application.persistentDataPath,path));
		if (!downloaded)
			uriBuilder = new UriBuilder(Path.Combine(_serverPath, path));
		var uri = uriBuilder.Uri;
		var request = UnityWebRequestTexture.GetTexture(uri);
		yield return request.SendWebRequest();
		if (!string.IsNullOrEmpty(request.error))
		{
			var streamBuilder = new UriBuilder(Path.Combine(Application.streamingAssetsPath,path));
			var stream = streamBuilder.Uri;
			var streaming = UnityWebRequestTexture.GetTexture(stream);
			yield return streaming.SendWebRequest();
			onComplete(string.IsNullOrEmpty(streaming.error), streaming.downloadHandler);
			yield break;
		}
		onComplete(string.IsNullOrEmpty(request.error), request.downloadHandler);
	}
	
	protected IEnumerator CutSprites(string localPath, float spriteHeight, float spriteWidth, Action<List<Sprite>> onComplete)
	{
		DownloadHandler request = null;
		yield return GetTexture(localPath, (success, handler) =>
		{
			request = handler;
		});

		if (request == null) yield break;

		var texture = ((DownloadHandlerTexture) request).texture;

		var _spriteCount = Mathf.RoundToInt(texture.height / spriteHeight);
		var _spriteWidth = Mathf.RoundToInt(texture.width / spriteWidth);
		var spriteList = new List<Sprite>();
		for (var i = 0; i < _spriteCount; i++)
		{
			for (var j = 0; j < _spriteWidth; j++)
			{
				var sprite = Sprite.Create(texture,
					new Rect(j * spriteWidth, i * spriteHeight, spriteWidth, spriteHeight),
					new Vector2(0.5f, 0.5f));
				spriteList.Add(sprite);
			}
		}

		onComplete(spriteList);

	}
}
