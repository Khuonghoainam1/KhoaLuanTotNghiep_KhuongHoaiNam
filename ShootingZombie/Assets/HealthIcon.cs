using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("IconBot/NewIcon/" + User.Instance.UserPlayerUsing.skin);
    }
}
