using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int x;
    public int y;

    // �Ƿ���ߡ��Ƿ�������ȣ�������������չ
    public bool isWalkable = true;

    public void Init(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
    }

    private void OnMouseDown()
    {
    }
}
