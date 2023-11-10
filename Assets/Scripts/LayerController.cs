using AwakeSolutions;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [SerializeField] public AwakeMediaPlayer _contentPlayer;
    [SerializeField] public AlphaTransition _layerAlpha;

    public void Open(string folderName, string fileName, bool autoPlay = false, bool loop = false)
    {
        _contentPlayer.Open(folderName,fileName, autoPlay, loop);
    }

    public void StartFadeIn(float duration)
    {
        _layerAlpha.StartFadeIn(duration);
    }

    public void StartFadeOut(float duration)
    {
        _layerAlpha.StartFadeOut(duration);
    }

    public void SetOpaque()
    {
        _layerAlpha.SetOpaque();
    }

    public void SetTransparent()
    {
        _layerAlpha.SetTransparent();
    }

    public void SetSpeed(float speed)
    {
        _contentPlayer.speed = speed;
    }

    public void Play()
    {
        _contentPlayer.Play();
        _contentPlayer.Seek(0);
    }
}
