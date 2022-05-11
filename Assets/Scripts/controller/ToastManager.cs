using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ToastManager<T> : SerializedMonoBehaviour
{
    [OdinSerialize] protected Dictionary<T, ToastController> _toastDictionary = new Dictionary<T, ToastController>();

    [SerializeField] protected float _toastTime = 0.2f;
    [SerializeField] protected float _toastDisplayTime = 2f;

    public void ShowToast(T type)
    {
        var toast = _toastDictionary[type];
        toast.gameObject.SetActive(true);
        LeanTween.alphaCanvas(toast.CanvasGroup, 1, _toastTime).setOnComplete(
            () =>
            {
                LeanTween.alphaCanvas(toast.CanvasGroup, 0, _toastTime).setDelay(_toastDisplayTime).setOnComplete(() =>
                {
                    toast.gameObject.SetActive(false);
                });
            });
    }
}
