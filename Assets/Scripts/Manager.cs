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

    IEnumerator Scheduler()
    {
        AudioAnalyzer.disabled = false;
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.playSuccess();
        yield return new WaitForSeconds(2);
        SoundManager.Instance.playSuccess();
        AudioAnalyzer.disabled = true;
        currentStage = 1;
        yield return new WaitForSeconds(1);
        SoundManager.Instance.playSuccess();
        currentStage = 2;
        AudioAnalyzer.disabled = false;
        StartCoroutine(analyzer.Record(2, "test"));
    }

    private void Restart()
    {
        StopCoroutine(gameRoutine);
        currentStage = 0;

        gameRoutine = Scheduler();
        StartCoroutine(gameRoutine);
    }

    void Start()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();

            if (Display.displays.Length > 2)
            {
                Display.displays[2].Activate();
            }
        } else
        {
            Debug.Log("App requires 3 displays, but only " + Display.displays.Length + " are connected");
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
                background.transform.Translate(Vector3.back * Time.deltaTime, Space.World);

                break;
            case 2:
                // Move the object upward in world space 1 unit/second.
                background.transform.Translate(Vector3.zero, Space.World);
                break;
            default:
                break;
        }
    }
}