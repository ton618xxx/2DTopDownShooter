using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    [Tooltip("Смещение камеры относительно цели. Y = -1 сдвигает кадр вниз.")]
    public Vector2 positionOffset = new Vector2(0f, -1f);

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {

    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x + positionOffset.x, target.position.y + positionOffset.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget; 
    }
}
