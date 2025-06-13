using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public RenderTexture renderTexture;

    void Start()
    {
        // Asignar la render texture al video player
        videoPlayer.targetTexture = renderTexture;

        // Asignar la render texture al raw image
        rawImage.texture = renderTexture;

        // Reproducir el video
        videoPlayer.Play();

        // Velocidad normal = 1.0f
        // Más lento: valores entre 0.0f y 1.0f (ej. 0.5f = mitad de velocidad)
        // Más rápido: valores mayores a 1.0f (ej. 2.0f = doble velocidad)    
        videoPlayer.playbackSpeed = 0.5f;
    }
}