using UnityEngine;

public class PlayerReferencing : MonoBehaviour
{
    public Transform playerTransform;
    public Camera mainCamera;
    public Transform referenceTransform;
    public GameObject testObject;

    private void Update()
    {
        var currentPos = Input.mousePosition;
        var worldPos =
            mainCamera.ScreenToWorldPoint(new Vector3(currentPos.x, currentPos.y, -mainCamera.transform.position.z));
        worldPos.z = 0;
        playerTransform.position = worldPos;

        var height = testObject.GetComponent<Renderer>().bounds.size.y;
        testObject.transform.position = referenceTransform.position - new Vector3(0, height / 2, 0);

        if (Input.GetMouseButtonDown(0)) Debug.Log(referenceTransform.position);
    }
}