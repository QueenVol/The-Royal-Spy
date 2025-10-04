using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int x;
    public int y;
    public bool isWalkable = true;

    private SpriteRenderer sr;
    private Color defaultColor;
    public Color highlightColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;

        if (sr == null) sr = GetComponent<SpriteRenderer>();
        defaultColor = ((x + y) % 2 == 0) ? Color.white : Color.gray;
        sr.color = defaultColor;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    void OnMouseDown()
    {
        GridManager.Instance.OnTileClicked(this);
    }

    public void Highlight(bool enable)
    {
        if (sr != null)
            sr.color = enable ? highlightColor : defaultColor;
    }
}