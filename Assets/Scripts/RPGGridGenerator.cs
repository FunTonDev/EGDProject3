using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGridGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject gridUnit;
    [SerializeField] private Transform gridContainer;

    [Header("Variables")]
    public uint totalX;
    public uint totalY;
    public uint totalZ;
    public Quaternion targetRot;
    private float tileSize;

    private void Awake()
    {
        tileSize = 1;
        for (int i = 0; i < totalY; i++)
        {
            for (int j = 0; j < totalZ; j++)
            {
                for (int k = 0; k < totalX; k++)
                {
                    Vector3 pos = new Vector3(gridContainer.position.x + tileSize * k, gridContainer.position.y + tileSize * i, gridContainer.position.z - tileSize * j);
                    GameObject newUnit = Instantiate(gridUnit, pos, gridContainer.transform.rotation, gridContainer);
                }
            }
        }
        transform.rotation = targetRot;
    }
}
