using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] int iMapType;
    [SerializeField] GameObject iTile;
    [SerializeField] GameObject [,] iTileArray;
    int iLimit = 8;
    int iCount = 1;

    // Start is called before the first frame update
    void Awake()
    {
        iMapType = 0;
        iTileArray = new GameObject[iLimit,iLimit];

        int iTemp = Screen.width / 16;
        Debug.Log($"{Screen.width}, {iTemp}");

        this.transform.position = new Vector3(this.transform.position.x - 10, this.transform.position.y - 10, 0);


        CreateMap(iMapType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMap(int type)
    {
        float fTemp = 2.6f;

        for (int i = 0; i < iLimit; i++)
        {
            for (int j = 0; j < iLimit; j++)
            {
                float fTempX = (float)j * fTemp;
                float fTempY = (float)i * fTemp;

                CreateTile(0, i, j, fTempX, fTempY);
            }
        }

        //this.transform.rotation = Quaternion.Euler(new Vector3(90, this.transform.rotation.y, this.transform.rotation.z));
    }

    public void CreateTile(int type, int i, int j, float x, float y)
    {
        Vector3 vTemp = this.transform.position + new Vector3(x, y, 0);
        GameObject instance = Instantiate(iTile, vTemp, this.transform.rotation);
        instance.transform.SetParent(this.transform);
        instance.GetComponent<Tile_>().SetInit(iCount++, vTemp.x, vTemp.y);
        iTileArray[i,j] = instance;
    }
}
