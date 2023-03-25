using UnityEngine;

public class PlayerReferencing : MonoBehaviour
{
    public Transform playerTransform;
    public Transform referenceTransform;
    public GameObject testObject;

    private void Update()
    {
        var currentPos = Input.mousePosition;
        var worldPos =
            Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x, currentPos.y, -Camera.main.transform.position.z));
        worldPos.z = 0;
        playerTransform.position = worldPos;
        
        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        transform.position = mousePosition;*/

        /*var height = testObject.GetComponent<Renderer>().bounds.size.y;
        testObject.transform.position = referenceTransform.position - new Vector3(0, height / 2, 0);*/

        if (Input.GetMouseButtonDown(0)) Debug.Log(referenceTransform.position);
    }
}