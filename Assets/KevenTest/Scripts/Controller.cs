using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject blackHole;
    [SerializeField] private GameObject tempoPlanet;
    [SerializeField] private float cooldown;

    private Plane plane;

    private void Start()
    {
        plane = new Plane(Vector3.back, 0);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (GetWorldPosition() != Vector3.zero)
            {
                Instantiate(tempoPlanet, GetWorldPosition(), transform.rotation);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (GetWorldPosition() != Vector3.zero)
            {
                Instantiate(blackHole, GetWorldPosition(), transform.rotation);
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
