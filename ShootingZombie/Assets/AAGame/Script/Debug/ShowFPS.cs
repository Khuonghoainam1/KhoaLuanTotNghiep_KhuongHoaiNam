using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class ShowFPS : MonoBehaviour
{
    public int FramesPerSec { get; protected set; }

    [SerializeField] private float frequency = 0.5f;


    private TMP_Text counter;

    private void Start()
    {
        Application.targetFrameRate = 60;
        //OnDemandRendering.renderFrameInterval = 1;
        counter = GetComponent<TMP_Text>();
        counter.text = "";
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);

            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            counter.text = "FPS: " + FramesPerSec.ToString();
        }
    }
}
