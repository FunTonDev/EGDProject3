using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class RPGGridGenerator : MonoBehaviour
{
    //[Header("USER INPUT")]
    [Header("Components")]
    [SerializeField] private GameObject gridUnit;
    [SerializeField] private List<Mesh> meshes;

    [Header("Variables")]
    public int terrainID;
    public Vector3 size;
    public Quaternion targetRot;
    public int xChange;
    public int zChange;

    private void Start()
    {
        GameObject model = transform.GetChild(0).gameObject;
        Transform gridContainer = transform.GetChild(1);
        int tileSize = 1, xDiff = 0, zDiff = 0;

        if (terrainID < -1 || terrainID > 1)
        {
            Debug.LogError(string.Format("Terrain[{0}] not given proper ID", gameObject.name));
            return;
        }
        else if (terrainID == 1)
        {
            model.transform.localPosition = new Vector3(-1.67f, 0.0f, 1.67f);
            model.transform.localScale = Vector3.one * 5;
        }

        model.GetComponent<MeshFilter>().mesh = meshes[terrainID];
        for (int i = 0; i < size.y; i++, zDiff += zChange)
        {
            for (int j = 0; j < size.z + zDiff; j++, xDiff += xChange)
            {
                for (int k = 0; k < size.x + xDiff; k++)
                {
                    Vector3 pos = new Vector3(gridContainer.position.x + tileSize * k, gridContainer.position.y + tileSize * i, gridContainer.position.z - tileSize * j);
                    GameObject newUnit = Instantiate(gridUnit, pos, gridContainer.transform.rotation, gridContainer);
                }
            }
        }
        transform.rotation = targetRot;
    }
}