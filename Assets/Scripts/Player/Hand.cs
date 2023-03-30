using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform referenceTransform;
    [SerializeField] private GameObject blackHolePreview;
    [SerializeField] private GameObject blackHole;
    [SerializeField] private GameObject tempoPreview;
    [SerializeField] private GameObject tempo;

    private GameObject blackHolePreviewRef;

    private bool isPlacingAbilty = false;
    private void Update()
    {
        var currentPos = Input.mousePosition;
        var worldPos =
            Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x, currentPos.y, -Camera.main.transform.position.z));
        worldPos.z = 0;
        playerTransform.position = worldPos;

        if (isPlacingAbilty)
        {
            blackHolePreviewRef.transform.position = referenceTransform.position;

            if (Input.GetMouseButtonDown(0))
            {
                SpawnBlackHole();
            }
        }
    }

    public void OnBlackHoleAbilityClick()
    {
        if (isPlacingAbilty) return;
        isPlacingAbilty = true;
        //Check currency
        blackHolePreviewRef = Instantiate(blackHolePreview,
            referenceTransform.position,
            blackHolePreview.transform.rotation);
    }

    private void SpawnBlackHole()
    {
        isPlacingAbilty = false;
        Destroy(blackHolePreviewRef.gameObject);
        Instantiate(blackHole, referenceTransform.position, blackHole.transform.rotation);
    }
}
