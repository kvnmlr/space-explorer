using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    public AudioClip open;
    public AudioClip error;
    public AudioClip success;
    public AudioClip tap;
    public AudioClip truster;
    public AudioClip comet;
    public AudioClip ambient;
    public AudioClip[] voices;


    public AudioSource audioSource;
    public AudioSource voiceAudioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator playFile(string filename)
    {
        Debug.Log(filename);
        //AudioClip clip = (AudioClip)Resources.Load(filename);
        //audioSource.clip = clip;
        //audioSource.PlayOneShot(clip);

        string path = "file:///" + filename;

        WWW www = new WWW(path);
        yield return www;
        AudioClip clip = www.GetAudioClip(false, false, AudioType.WAV);
        //auSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    public void stopVoice()
    {
        voiceAudioSource.Stop();
    }

    public void stopSounds()
    {
        audioSource.Stop();
    }

    public void playVoice(int i)
    {
        voiceAudioSource.clip = voices[i];
        voiceAudioSource.Play();
    }

    public void playOpen()
    {
        audioSource.PlayOneShot(open, 0.7F);
    }

    public void playSuccess()
    {
        audioSource.PlayOneShot(success, 0.7F);
    }

    public void playError()
    {
        audioSource.PlayOneShot(error, 0.7F);
    }

    public void playTap()
    {
        audioSource.PlayOneShot(tap, 0.7F);
    }

    public void playThruster()
    {
        audioSource.PlayOneShot(truster, 0.2F);
    }

    public void playComet()
    {
        audioSource.PlayOneShot(comet, 0.7F);
    }

    public void playAmbient()
    {
        audioSource.PlayOneShot(ambient, 0.05F);
    }
}