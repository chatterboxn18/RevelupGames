using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxItem : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    [SerializeField] private Image _image; 
    public List<Sprite> Sprites = new List<Sprite>();

    public void SetImage(int index)
    {
        _image.sprite = Sprites[index];
    }
}
