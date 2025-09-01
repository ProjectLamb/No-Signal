using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using FMODUnity;
using FMOD.Studio;

public class VideoManager : MonoBehaviour
{
    private EventInstance creditOST;
    private float pitch;
    public VideoPlayer vid;

    void Start()
    {
        vid.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Credit.mp4");
        creditOST = AudioManager.Instance.CreateInstance(FMODEvents.instance.credit);
        creditOST.start();
        StartCoroutine("WaitForVid");
    }

    void VideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("Title");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // creditOST.getPitch(out pitch);
            // pitch = 3f;
            // creditOST.setPitch(pitch);
            vid.playbackSpeed = 3f;
        }
        else
        {
            // creditOST.getPitch(out pitch);
            // pitch = 1.0f;
            // creditOST.setPitch(pitch);
            vid.playbackSpeed = 1f;
        }
    }

    IEnumerator WaitForVid()
    {
        yield return new WaitForSeconds(10f);
        vid.Play();
        vid.loopPointReached += VideoEnd;
    }
}
