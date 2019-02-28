using System;
using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    protected GameObject background;

    [SerializeField]
    protected AudioSource microphone;

    [SerializeField]
    protected AudioAnalyzer analyzer;

    [SerializeField]
    protected KeyCode restartKeyCode = KeyCode.Return;

    private bool isPlaying = false;


    public Animator cameraAnimator;
    public Animator liloAnimator;
    public Animator rocketAnimator;

    private int currentStage;
    private IEnumerator gameRoutine;
    private IEnumerator playbackRoutine;
    private string UUID;
    private string recordingsRoot;

    IEnumerator PlayBack()
    {
        isPlaying = false;

        SoundManager.Instance.audioSource.volume = 1f;
        SoundManager.Instance.audioSource.panStereo = 0f;
        AudioAnalyzer.disabled = false;

        SoundManager.Instance.playVoice(0);
        yield return new WaitForSeconds(11f);

        StartCoroutine(SoundManager.Instance.playFile(recordingsRoot + "/" + UUID + "/name.wav"));
        yield return new WaitForSeconds(3);

        SoundManager.Instance.playVoice(1);
        yield return new WaitForSeconds(11);

        SoundManager.Instance.playVoice(2);
        yield return new WaitForSeconds(7f);

        StartCoroutine(SoundManager.Instance.playFile(recordingsRoot + "/" + UUID + "/mitnehmen.wav"));
        yield return new WaitForSeconds(8);
    }


    IEnumerator Scheduler()
    {
        isPlaying = true;
        
        SoundManager.Instance.audioSource.volume = 1f;
        SoundManager.Instance.audioSource.panStereo = 0f;
        SoundManager.Instance.playAmbient();

        UUID = GetUniqueID();

        cameraAnimator.enabled = true;
        liloAnimator.enabled = true;
        rocketAnimator.enabled = true;

        cameraAnimator.Play(0);
        liloAnimator.Play(0);
        rocketAnimator.Play(0);

        yield return new WaitForSeconds(1);
        currentStage = 1;
        AudioAnalyzer.disabled = false;

        yield return new WaitForSeconds(6);

        SoundManager.Instance.playVoice(0);
        yield return new WaitForSeconds(11f);

        StartCoroutine(analyzer.Record(3, recordingsRoot + "/" + UUID + "/name.wav"));
        yield return new WaitForSeconds(3);

        SoundManager.Instance.audioSource.panStereo = 0.6f;
        SoundManager.Instance.playThruster();

        yield return new WaitForSeconds(6);
        SoundManager.Instance.playVoice(1);

        yield return new WaitForSeconds(3);
        SoundManager.Instance.audioSource.volume = 1f;
        SoundManager.Instance.audioSource.panStereo = -0.6f;
        SoundManager.Instance.playThruster();

       
        yield return new WaitForSeconds(9.5f);

        SoundManager.Instance.playVoice(2);
        yield return new WaitForSeconds(6.5f);

        StartCoroutine(analyzer.Record(8, recordingsRoot + "/" + UUID + "/mitnehmen.wav"));
        yield return new WaitForSeconds(8);
        yield return new WaitForSeconds(8);

        SoundManager.Instance.playVoice(3);
        yield return new WaitForSeconds(9);
        currentStage = 2;
        AudioAnalyzer.disabled = false;

        yield return new WaitForSeconds(7.5f);
        yield return new WaitForSeconds(15f);

        SoundManager.Instance.playVoice(4);
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(15f);
        SoundManager.Instance.playVoice(5);

    }

    private void Reset()
    {
        StopCoroutine(gameRoutine);
        StopCoroutine(playbackRoutine);

        currentStage = 0;

        cameraAnimator.Rebind();
        liloAnimator.Rebind();
        rocketAnimator.Rebind();

        cameraAnimator.enabled = false;
        liloAnimator.enabled = false;
        rocketAnimator.enabled = false;

        background.transform.position = new Vector3(0, 0, 0);
        gameRoutine = Scheduler();
        playbackRoutine = PlayBack();
    }

    void Start()
    {
        recordingsRoot = Application.persistentDataPath + "/Recordings";
        /*if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();

            if (Display.displays.Length > 2)
            {
                Display.displays[2].Activate();
            }
        } else
        {
            Debug.Log("App supports 3 displays, but only " + Display.displays.Length + " are connected");
        }*/

        cameraAnimator.enabled = false;
        liloAnimator.enabled = false;
        rocketAnimator.enabled = false;

        gameRoutine = Scheduler();
        playbackRoutine = PlayBack();
    }

    void Update()
    {
        if (Input.GetKeyUp(restartKeyCode))
        {
            if (!isPlaying)
            {
                SoundManager.Instance.playError();
                Reset();
                SoundManager.Instance.stopVoice();
                SoundManager.Instance.stopSounds();
                StartCoroutine(gameRoutine);

            } else
            {
                isPlaying = false;
                SoundManager.Instance.playError();
                Reset();
                SoundManager.Instance.stopVoice();
                Debug.Log("Current UID: " + UUID);
                StartCoroutine(playbackRoutine);
            }

        }     

        switch (currentStage)
        {
            case 1:
                // Move the object upward in world space 1 unit/second.
                background.transform.Translate(Vector3.back * Time.deltaTime * 10, Space.World);

                break;
            case 2:
                // Move the object upward in world space 1 unit/second.
                background.transform.Translate(Vector3.zero, Space.World);
                break;
            default:
                break;
        }
    }

    public static string GetUniqueID()
    {
        string key = "ID";

        var random = new System.Random();
        DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;

        string uniqueID = string.Format("{0:X}", Convert.ToInt64(timestamp));

        Debug.Log("Generated Unique ID: " + uniqueID);
        return uniqueID;
    }
}