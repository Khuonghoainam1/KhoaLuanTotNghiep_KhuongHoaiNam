using UnityEngine;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    public GameObject white;
    public GameObject red;
    public TMP_Text value;
    public void OnEnable()
    {
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            gameObject.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
    private void Update()
    {
        float x = white.transform.localScale.x;
        x = Mathf.Lerp(white.transform.localScale.x, red.transform.localScale.x, Time.deltaTime * 4);
        white.transform.localScale = new Vector3(x, white.transform.localScale.y, white.transform.localScale.z);
    }
}
