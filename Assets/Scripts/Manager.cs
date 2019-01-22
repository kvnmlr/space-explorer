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

    private int currentStage;
    private IEnumerator gameRoutine;
    private string UUID;
    private string recordingsRoot;

    IEnumerator Scheduler()
    {
        AudioAnalyzer.disabled = false;
        yield return new WaitForSeconds(0);
        SoundManager.Instance.playSuccess();
        AudioAnalyzer.disabled = true;
        currentStage = 1;
        yield return new WaitForSeconds(60);
        SoundManager.Instance.playSuccess();
        currentStage = 2;
        AudioAnalyzer.disabled = false;
        StartCoroutine(analyzer.Record(2, recordingsRoot + "/" + UUID + "/test.wav"));
    }

    private void Restart()
    {
        StopCoroutine(gameRoutine);
        currentStage = 0;
        UUID = GetUniqueID();

        gameRoutine = Scheduler();
        StartCoroutine(gameRoutine);
    }

    void Start()
    {
        recordingsRoot = Application.persistentDataPath + "/Recordings/";
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();

            if (Display.displays.Length > 2)
            {
                Display.displays[2].Activate();
            }
        } else
        {
            Debug.Log("App supports 3 displays, but only " + Display.displays.Length + " are connected");
        }

        gameRoutine = Scheduler();

        Restart();
    }

    void Update()
    {
        if (Input.GetKeyDown(restartKeyCode))
        {
            SoundManager.Instance.playError();
            Restart();
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