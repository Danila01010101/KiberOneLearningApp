 using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
    public class GifOpener
    {
        private VideoPlayer videoPlayer;
        private Transform videoWindow;

        public GifOpener(VideoPlayer videoPlayer, Transform videoWindow)
        {
            this.videoPlayer = videoPlayer;
            this.videoWindow = videoWindow;
        }
        
        public void Subscribe(Button openGifButton, Button closeGifButton)
        {
            openGifButton.onClick.AddListener(OpenGifWindow);
            closeGifButton.onClick.AddListener(CloseGifWindow);
        }
        
        public void Unsubscribe(Button openGifButton, Button closeGifButton)
        {
            openGifButton.onClick.RemoveListener(OpenGifWindow);
            closeGifButton.onClick.RemoveListener(CloseGifWindow);
        }

        private void OpenGifWindow()
        {
            videoWindow.gameObject.SetActive(true);
            videoWindow.localScale = Vector3.zero;
            videoWindow.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(videoPlayer.Play);
        }

        private void CloseGifWindow()
        {
            videoWindow.localScale = Vector3.one;
            videoWindow.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(delegate
            {
                videoWindow.gameObject.SetActive(false);
            });
            videoPlayer.Stop();
        }

        public void SetNewVideo(string path)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = path;
            videoPlayer.Prepare();
            videoPlayer.Prepare();
        }
    }
}
