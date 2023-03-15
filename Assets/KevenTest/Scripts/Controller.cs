using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject blackHole;
    [SerializeField] private GameObject tempoPlanet;
    [SerializeField] private float cooldown;
    private Motherbase.CoreMotherBase mb;

    private Plane plane;

    private bool isShowingPreview = false;

    private void Start()
    {
        plane = new Plane(Vector3.back, 0);
        mb = FindObjectOfType<Motherbase.CoreMotherBase>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GetWorldPosition() != Vector3.zero)
            {
                Instantiate(tempoPlanet, GetWorldPosition(), transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GetWorldPosition() != Vector3.zero)
            {
                Instantiate(blackHole, GetWorldPosition(), transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isShowingPreview)
            {
                mb.showShieldsPreview(false);
            }
            else
            {
                mb.showShieldsPreview(true);
            }
        }
    }

    private Vector3 GetWorldPosition()
    {
        float distance;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(mouseRay, out distance))
        {
            return mouseRay.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
