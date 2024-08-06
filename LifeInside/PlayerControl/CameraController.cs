using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PublicVariables
    public Vector3 offset;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    #endregion

    #region PrivateVariables
    public Transform player;
#endregion

#region PublicMethods

#endregion

#region PrivateMethods
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + new Vector3(offset.x, offset.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

            float clampedX = Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x);
            float clampedY = Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
#endregion
}
