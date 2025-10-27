using UnityEngine;

public class ParallaxControl : MonoBehaviour
{
    private float startPos;
    private GameObject camera;
    [SerializeField] private float parallaxEffect;

    private void OnEnable()
    {
        camera = GameManager.Instance.cam.gameObject;
        startPos = transform.position.x;
    }

    private void Update()
    {
        float distance = (camera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
