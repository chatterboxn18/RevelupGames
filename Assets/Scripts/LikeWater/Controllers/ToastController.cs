using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _toastText;
    [SerializeField] private CanvasGroup _canvasGroup;

    public CanvasGroup CanvasGroup => _canvasGroup;

    private void Awake()
    {
        _canvasGroup.alpha = 0;
    }

    public void SetText(string message)
    {
        _toastText.text = message;
    }
}
