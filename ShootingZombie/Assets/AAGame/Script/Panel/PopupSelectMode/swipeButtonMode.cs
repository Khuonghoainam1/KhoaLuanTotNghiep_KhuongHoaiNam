using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swipeButtonMode : MonoBehaviour
{   //ScrollBerSwipe
    [SerializeField] private GameObject scrollBar;
    [SerializeField] private Transform content;
    [SerializeField] private List<GameObject> btnCellBoss = new List<GameObject>();
    private float scroll_pos = 0;
    private float[] pos;
    int number;
    float time;
    private bool runIt = false;
    public int abc;

    private void Update()
    {
        UpdateModeMenu();
    }   
    private void UpdateModeMenu()
    {
        pos = new float[btnCellBoss[1].transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        if (runIt)
        {
            GecisiDuzenle(distance, pos);
            time += Time.deltaTime;
            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (Mathf.Abs(scroll_pos - pos[i]) < distance / 2)
            {
                Transform selectChild = transform.GetChild(i);
                // Thiết lập kích thước đã chọn
                selectChild.localScale = Vector2.Lerp(selectChild.localScale, new Vector2(0.7f, 0.7f), 0.1f);

                btnCellBoss[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                Transform otherChild = transform.GetChild(i);
                otherChild.localScale = Vector2.Lerp(otherChild.localScale, new Vector2(0.3f, 0.3f), 0.1f);
            }

        }
    }

    private void GecisiDuzenle(float distance, float[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[number], 1f * Time.deltaTime);
            }
        }
    }

}
