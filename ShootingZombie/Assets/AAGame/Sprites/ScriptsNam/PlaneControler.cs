using System.Collections;
using UnityEngine;
using Yurowm.GameCore;
using AA_Game;
public class PlaneControler : MonoBehaviour
{
    public float timeScale;
    public Transform posBullet;
    private float speed ;
    public Transform Target;

    public void OnEnable()
    {
        speed = 20;

        StartCoroutine(ShootPlane());
    }

    public void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    
    IEnumerator ShootPlane()
    {
        Item bullet = null;
        if (GlobalData.gameMode != GameMode.BossWorld)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else if(GlobalData.gameMode == GameMode.BossWorld)
        {
            yield return new WaitForSeconds(1f);
        }


        for (int i = 0; i < 20; i++)
        {
            bullet = ContentPoolable.Emit(ItemID.boom) as Item;
            bullet.transform.position = Target.transform.position;

            if(GlobalData.gameMode != GameMode.BossWorld)
            {
                yield return new WaitForSeconds(0.15f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(6f);
        bullet.Kill();
        gameObject.GetComponent<Item>().Kill();

    }
}
