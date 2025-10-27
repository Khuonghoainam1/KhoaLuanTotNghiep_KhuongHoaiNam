using UnityEngine;

public class TramDungControl : MonoBehaviour
{
    public GameObject[] tramdungSo;

    private void OnEnable()
    {
        foreach(GameObject obj in tramdungSo)
        {
            obj.SetActive(false);
            tramdungSo[GameManager.Instance.mapNumber].SetActive(true);
        }
    }
}
