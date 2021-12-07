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
    public bool decayX;
    public bool decayZ;
    public Quaternion targetRot;
    private float tileSize;

    private void Awake()
    {
        int xDiff = 0, zDiff = 0;
        Debug.Log(gameObject.name);
        tileSize = 1;
        for (int i = 0; i < totalY; i++)
        {
            for (int j = 0; j < totalZ - zDiff; j++, zDiff -= decayZ ? 1 : 0)
            {
                for (int k = 0; k < totalX - xDiff; k++, xDiff -= decayX ? 1 : 0)
                {
                    Vector3 pos = new Vector3(gridContainer.position.x + tileSize * k, gridContainer.position.y + tileSize * i, gridContainer.position.z - tileSize * j);
                    GameObject newUnit = Instantiate(gridUnit, pos, gridContainer.transform.rotation, gridContainer);
                }
            }
        }
        transform.rotation = targetRot;
    }
}
