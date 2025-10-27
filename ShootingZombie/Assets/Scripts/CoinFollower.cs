using AA_Game;
using UnityEngine;

public class CoinFollower : Item
{
    private Transform player;
    private float acceleration = 3f;
    private float maxSpeed = 30f;

    private Vector3 previousPlayerPosition;
    private float currentSpeed;

    private void OnEnable()
    {
        player = GameManager.Instance.trainManager.transform;
        previousPlayerPosition = player.position;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 currentPlayerPosition = player.position + new Vector3(0, 2, 0);
            Vector3 playerVelocity = (currentPlayerPosition - previousPlayerPosition) / Time.fixedDeltaTime;
            if (playerVelocity.magnitude < 0.1f)
            {
                playerVelocity = new Vector3(-10,0,0);
            }

            currentSpeed = Mathf.Clamp(playerVelocity.magnitude * acceleration, 0f, maxSpeed);
            Vector3 direction = (currentPlayerPosition - transform.position).normalized;
            transform.Translate(direction * currentSpeed * Time.fixedDeltaTime);

            previousPlayerPosition = currentPlayerPosition;
            if(Vector3.Distance(transform.position, player.transform.position) < 3f)
            {
                Kill();
            }
        }
    }
}
