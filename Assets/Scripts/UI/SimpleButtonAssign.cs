using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleButtonAssign : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void SetImage(Sprite sprite, string text = "")
    {
        _image.sprite = sprite;
        _text.text = text;
    }
}
