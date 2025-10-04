using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pieces))]
public class DragNDrop : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private Pieces piece;

    void Start()
    {
        cam = Camera.main;
        piece = GetComponent<Pieces>();
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        piece.SnapToNearestTile();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }
}
