using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MusicClass : MonoBehaviour
{
    private AudioSource[] _audioSources;
    public float fadeSpeed = 0.5f;
    public float fadeInOutMultiplier = 0.0f;
    public float volumeMax = 1.0f;
    public bool isPlaying;

    public string playingTrackName = "Nothing";
    public int playingTrackIndex;
    public float playingTrackVolume = 0.000f;

    public string lastTrackName = "Nothing";
    public int lastTrackIndex;
    public float lastTrackVolume = 0.000f;



    public IEnumerator FadeOutOldMusic_FadeInNewMusic()
    {
        _audioSources[playingTrackIndex].volume = 0.000f;
        _audioSources[playingTrackIndex].Play();
        while (_audioSources[playingTrackIndex].volume < volumeMax)
        {
            _audioSources[lastTrackIndex].volume -= fadeSpeed * 2;
            _audioSources[playingTrackIndex].volume += fadeSpeed * 2;
            //Debug.Log("Fade: " + lastTrackName + " " + _audioSources[lastTrackIndex].volume.ToString() + " Rise: " + playingTrackName + " " + _audioSources[playingTrackIndex].volume.ToString());
            yield return new WaitForSeconds(0.001f);
            lastTrackVolume = _audioSources[lastTrackIndex].volume;
            playingTrackVolume = _audioSources[playingTrackIndex].volume;
        }
        _audioSources[lastTrackIndex].volume = 0.000f; // Just In Case....
        _audioSources[lastTrackIndex].Stop();

        lastTrackIndex = playingTrackIndex;
        lastTrackName = playingTrackName;
        isPlaying = true;
    }

    public IEnumerator FadeInNewMusic()
    {
        _audioSources[playingTrackIndex].volume = 0.000f;
        _audioSources[playingTrackIndex].Play();
        while (_audioSources[playingTrackIndex].volume < volumeMax)
        {
            _audioSources[playingTrackIndex].volume += fadeSpeed * 2;
            //Debug.Log("Fading In: " + _audioSources[track_index].volume.ToString());
            yield return new WaitForSeconds(0.001f);
            playingTrackVolume = _audioSources[playingTrackIndex].volume;
        }
        lastTrackIndex = playingTrackIndex;
        lastTrackName = playingTrackName;
        isPlaying = true;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSources = GetComponentsInChildren<AudioSource>();
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<MusicClass>().PlayMusic("BGM");
        }
        else
            FindObjectOfType<MusicClass>().PlayMusic("Track1");

    }

    public void PlayMusic(string transformName)
    {
        for (int a = 0; a < _audioSources.Length; a++)
        {
            if (_audioSources[a].name == transformName)
            {
                Debug.Log("Found Track Name (" + transformName + ") at Index(" + a.ToString() + ")");
                playingTrackIndex = a;
                playingTrackName = transformName;
                break;
            }
        }
        if (playingTrackIndex == lastTrackIndex)
        {
            Debug.Log("Same Track Selected");
            return;
        }
        else
        {
            if (isPlaying)
            {
                Debug.Log("Fading in new music - Fading out old music");
                StartCoroutine(FadeOutOldMusic_FadeInNewMusic());
            }
            else
            {
                Debug.Log("Fading in new music");
                StartCoroutine(FadeInNewMusic());
            }
        }
    }

    public void setVolume(float val)
    {
        volumeMax = val;
    }

    public void toggleMusic()
    {
        if(isPlaying)
            FindObjectOfType<MusicClass>().PlayMusic("Nothing");
        else
            FindObjectOfType<MusicClass>().PlayMusic("Track1");
    }
}
