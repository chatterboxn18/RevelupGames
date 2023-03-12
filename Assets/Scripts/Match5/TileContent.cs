using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileContent : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void SetContent(ContentData data)
    {
        _image.sprite = data.ContentSprite;
    }
}
