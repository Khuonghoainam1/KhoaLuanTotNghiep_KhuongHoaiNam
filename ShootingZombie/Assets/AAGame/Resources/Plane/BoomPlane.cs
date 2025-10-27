using AA_Game;
using Yurowm.GameCore;

public class BoomPlane : BulletBase
{
    private float distanceDestroy = 6f;

    public override void OnEnable()
    {
        base.OnEnable();
        bulletHealth = bulletHealthBase;

        if (GlobalData.gameMode == GameMode.BossWorld)
        {
            distanceDestroy = 13f;
        }
        else
        {
            distanceDestroy = 6f;
        }
    }

    public override void Update()
    {

        if (transform.position.y < -distanceDestroy)
        {
            Item hitBazoka = ContentPoolable.Emit(ItemID.enemy_hit_bazoka) as Item;
            hitBazoka.transform.position = this.transform.position;
            //choi am thanh bom plane
            AudioManager.instance.Play("boomPlane");
            Kill();
        }
    }
}
