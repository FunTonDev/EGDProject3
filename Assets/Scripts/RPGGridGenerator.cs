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
        if (terrainID < -1 || terrainID > 1)
        {
            Debug.LogError(string.Format("Terrain[{0}] not given proper ID([0,1])", gameObject.name));
            return;
        }
        else if (size.x < 1 || size.y < 1 || size.z < 1)
        {
            Debug.LogError(string.Format("Terrain[{0}] not given proper size([0,inf], [0,inf], [0,inf])", gameObject.name));
            return;
        }
        Transform model = transform.GetChild(0);
        Transform gridContainer = transform.GetChild(1);
        int tileSize = 1, xDiff = 0, zDiff = 0;
        model.GetComponent<MeshFilter>().mesh = meshes[terrainID];
        model.localPosition = (terrainID == 1 ? 1 : 0) * new Vector3(-0.167f * size.x, 0.0f, 0.167f * size.z);
        model.localScale = (terrainID == 1 ?  5 : 1) * new Vector3(0.1f * size.x, 0.1f * size.y, 0.1f * size.z);
        

        //Size alignment
        //model.localScale = new Vector3(model.localScale.x * size.x * 0.1f,
                                    //model.localScale.y, model.localScale.z * size.z * 0.1f);

        /*  MODEL ALWAYS AT 0,0,0(except triangle which is also pushed up/left)
         *  GRID CONTAINER STARTS AT 0,0,0 AND GETS PUSHED UP/LEFT
         *  
         */
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
        gridContainer.localPosition = new Vector3(-0.5f * (size.x - 1), 0.5f, 0.5f * (size.z - 1));
        transform.rotation = targetRot;
        //Gridcontainerpos = 4.5*5(n-1)
    }
}