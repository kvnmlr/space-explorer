using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject background;
    public AudioSource microphone;
    private int currentStage;

    IEnumerator Scheduler()
    {
        AudioAnalyzer.disabled = false;
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.playSuccess();
        yield return new WaitForSeconds(2);
        SoundManager.Instance.playSuccess();
        AudioAnalyzer.disabled = true;
        currentStage = 1;
        yield return new WaitForSeconds(2);
        SoundManager.Instance.playSuccess();
        currentStage = 2;
        AudioAnalyzer.disabled = false;
    }

    // Use this for initialization
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

        StartCoroutine(Scheduler());
    }

    // Update is called once per frame
    void Update()
    {

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
