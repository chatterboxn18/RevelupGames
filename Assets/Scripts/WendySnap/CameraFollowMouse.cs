using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollowMouse : MonoBehaviour
{
	private float _speed = 200;
	[SerializeField] private bool _enabled;

	[SerializeField] private Vector2 _limits;

	[SerializeField] private RawImage _texture;
	[SerializeField] private Camera _camera;
	[SerializeField] private RawImage _photo;
	[SerializeField] private Image _fader;

	public void EnableMovement(bool on)
	{
		_enabled = on;
	}

	private void Start()
	{
		_texture.texture = new RenderTexture(Mathf.RoundToInt(_texture.rectTransform.rect.width), Mathf.RoundToInt(_texture.rectTransform.rect.height), 32);
		_camera.targetTexture = (RenderTexture) _texture.texture;
	}
	
	private void Update(){
		if (!_enabled) return;
		Debug.Log(Input.mousePosition);
		if (Input.mousePosition.x > 1280)
		{
			transform.position += new Vector3(Time.deltaTime * _speed, 0.0f, 0.0f);
		}
		else if (Input.mousePosition.x < 640)
		{
			transform.position -= new Vector3(Time.deltaTime * _speed, 0.0f, 0.0f);
		}
		
		if (Input.mousePosition.y > 720)
		{
			transform.position += new Vector3(0.0f, Time.deltaTime * _speed, 0.0f);
		}
		else if (Input.mousePosition.y < 360)
		{
			transform.position -= new Vector3(0.0f,Time.deltaTime * _speed,  0.0f);
		}
		
		if (transform.position.x <= -_limits.x)
			transform.position = new Vector2(-_limits.x, transform.position.y);
		if (transform.position.x >= _limits.x)
			transform.position = new Vector2(_limits.x, transform.position.y);
		
		if (transform.position.y <= -_limits.y)
		{
			transform.position = new Vector2(transform.position.x, -_limits.y);
		}

		if (transform.position.y >= _limits.y)
		{
			transform.position = new Vector2(transform.position.x, _limits.y);
		}

		if (Input.GetMouseButton(0))
		{
			_enabled = false;
			var rt = new RenderTexture(_texture.texture.width, _texture.texture.height, 32);
			Graphics.Blit(_texture.texture, rt);
			_photo.texture = rt;
			StartCoroutine(Flash());
		}
	}

	private IEnumerator Flash()
	{
		var timer = 0f;
		var total = 0.5f;
		while (timer <= total)
		{
			_fader.color = _fader.color.SetAlpha(Mathf.Lerp(0,1, timer / total));

			timer += Time.deltaTime;
			yield return null;
		}

		timer = 0f;		
		_photo.transform.parent.parent.gameObject.SetActive(true);

		while (timer <= total)
		{
			_fader.color = _fader.color.SetAlpha(Mathf.Lerp(1, 0, timer / total));
			timer += Time.deltaTime;
			yield return null;
		}
	}
}
