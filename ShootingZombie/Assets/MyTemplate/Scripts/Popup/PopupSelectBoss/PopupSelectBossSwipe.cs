using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Thanh.Core;
public class PopupSelectBossSwipe : MonoBehaviour
{
    [SerializeField]
    private GameObject scrollbar;
    public List<GameObject> btnCellBoss=  new List<GameObject>();
    private float scroll_pos = 0;
    private float[] pos;
    private float time;
    private bool runIt = false;
    int btnNumber;
    private Button takeTheBtn;
    [SerializeField] private Transform content;
    public GameObject buttonPrefab;


    public void Start()
    {
        spawnCell();
    }

    private void Update()
    {
        UpdateMenuBoss();
    }

    private void UpdateMenuBoss()
    {
        pos = new float[transform.childCount];
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
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (Mathf.Abs(scroll_pos - pos[i]) < distance / 2)
            {
                Debug.LogWarning("Current Selected Level " + i);
                Transform selectedChild = transform.GetChild(i);

                // Thiết lập kích thước đã chọn
                selectedChild.localScale = Vector2.Lerp(selectedChild.localScale, new Vector2(0.7f, 0.7f), 0.1f);

                // Bật tương tác cho nút đã chọn
                btnCellBoss[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                Transform otherChild = transform.GetChild(i);

                // Thiết lập kích thước cho các nút khác
                otherChild.localScale = Vector2.Lerp(otherChild.localScale, new Vector2(0.3f, 0.3f), 0.1f);

                // Tắt tương tác cho các nút không được chọn
                btnCellBoss[i].GetComponent<Button>().interactable = false;
            }
        }
      
    }

    public void spawnCell()
    {
        foreach (LevelBossSetUp levelBossSetUp in LevelConfig.instance.levelBossSetUps)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, content);
            buttonInstance.GetComponent<SlectBossCellView>().SetData(levelBossSetUp);
            buttonInstance.GetComponent<SlectBossCellView>().level = levelBossSetUp.level;
            btnCellBoss.Add(buttonInstance);
        }
    }

    private void GecisiDuzenle(float distance, float[] pos)
    {
        
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);
            }
        }
    }

}
