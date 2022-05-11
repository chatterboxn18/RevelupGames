using System.IO;
using System.Text;
using UnityEngine;

public class JSONDataController<T> where T : new()
{
	private T _jsonContent;
	public T JsonContent => _jsonContent;

	public string CreateJsonString(T objectItem)
	{
		return JsonUtility.ToJson(objectItem);
	}

	public void UpdateJsonContent(T content, string localPath)
	{
		_jsonContent = content;
		WriteJson(localPath);
	}

	public void RetrieveJson(string localPath, string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			WriteJson(localPath);
			return;
		}

		_jsonContent = JsonUtility.FromJson<T>(text);

	}

	public void WriteJson(string localPath)
	{
		var path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + localPath;
		if (_jsonContent == null)
		{
			_jsonContent = new T();
		}
		var saveFile = CreateJsonString(_jsonContent);
		if (path.Length != 0 && saveFile != null)
		{
			File.WriteAllBytes(path, Encoding.ASCII.GetBytes(saveFile));
		}
	}
	
}
