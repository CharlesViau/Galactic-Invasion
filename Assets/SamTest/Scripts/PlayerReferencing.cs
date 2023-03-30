using UnityEngine;

public class PlayerReferencing : MonoBehaviour
{
    public Transform playerTransform;
    public Transform referenceTransform;

    
    private void Update()
    {
        var currentPos = Input.mousePosition;
        var worldPos =
            Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x, currentPos.y, -Camera.main.transform.position.z));
        worldPos.z = 0;
        playerTransform.position = worldPos;
    }
}