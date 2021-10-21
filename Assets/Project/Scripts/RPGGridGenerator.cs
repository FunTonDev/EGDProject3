using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGridGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject gridUnit;
    [SerializeField] private Transform gridContainer;

    [Header("Variables")]
    public uint totalHor;
    public uint totalVer;
    public float startXPos;
    public float startZPos;
    private float tileSize;
    private float yPos;

    private void Awake()
    {
        tileSize = gridUnit.transform.localScale.x;
        yPos = gameObject.transform.position.y;

        for (int i = 0; i < totalVer; i++)
        {
            for (int j = 0; j < totalHor; j++)
            {
                Vector3 pos = new Vector3(startXPos + tileSize * j, yPos, startZPos - tileSize * i);
                Instantiate(gridUnit, pos, gridContainer.transform.rotation, gridContainer);
            }
        }
    }
}
