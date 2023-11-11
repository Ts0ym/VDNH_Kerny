using AwakeSolutions;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public AwakeMediaPlayer ContentPlayer;
    public AlphaTransition LayerAlpha;

    public void Open(string folderName, string fileName, bool autoPlay = false, bool loop = false)
    {
        ContentPlayer.Open(folderName,fileName, autoPlay, loop);
    }

    public void StartFadeIn(float duration)
    {
        LayerAlpha.StartFadeIn(duration);
    }

    public void StartFadeOut(float duration)
    {
        LayerAlpha.StartFadeOut(duration);
    }

    public void SetOpaque()
    {
        LayerAlpha.SetOpaque();
    }

    public void SetTransparent()
    {
        LayerAlpha.SetTransparent();
    }

    public void SetSpeed(float speed)
    {
        ContentPlayer.speed = speed;
    }

    public void Play()
    {
        ContentPlayer.Play();
        ContentPlayer.Seek(0);
    }

    public void Pause()
    {
        ContentPlayer.Pause();
    }
}
