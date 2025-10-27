using UnityEngine;

public class IceBulletController : BulletBase
{
    public override void AddForce()
    {
        bulletRB.AddForce(-transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        transform.localScale = transform.localScale * (1+Time.deltaTime/2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bot"))
        {
            collision.gameObject.GetComponent<CharacterBase>().ChangeState(new CharStunState(collision.gameObject.GetComponent<CharacterBase>()));
        }
    }
}
