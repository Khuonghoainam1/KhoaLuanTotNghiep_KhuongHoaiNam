using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Target;
    public Transform camTransform;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 targetPosition = Target.transform.position + Offset;
            camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }
    }
}
