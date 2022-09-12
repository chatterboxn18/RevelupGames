using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest
{
    public class DQDebugCreator : MonoBehaviour
    {
        [SerializeField] private Image _cardPhoto;
        [SerializeField] private RectTransform _cardPhotoSize;
        [SerializeField] private NativeGalleryController _nativeGallery;

        private IEnumerator Start()
        {
            yield return null;
        }

        public void ButtonEvt_OpenGallery()
        {
            _nativeGallery.FromGalleryToImage(_cardPhoto, false, _cardPhotoSize.rect.width);
        }

        private void SetPictureToPhotoSize()
        {
            
        }
    }
}