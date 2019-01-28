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
        yield return new WaitForSeconds(1);
        AudioAnalyzer.disabled = false;

        yield return new WaitForSeconds(10);
        currentStage = 1;
        SoundManager.Instance.playSuccess();
        StartCoroutine(analyzer.Record(5, recordingsRoot + "/" + UUID + "/test.wav"));

        yield return new WaitForSeconds(10);
        SoundManager.Instance.playSuccess();
        StartCoroutine(analyzer.Record(5, recordingsRoot + "/" + UUID + "/test2.wav"));

        yield return new WaitForSeconds(50);
        currentStage = 2;
        AudioAnalyzer.disabled = false;
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