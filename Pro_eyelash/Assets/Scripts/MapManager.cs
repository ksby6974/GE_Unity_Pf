using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] int iMapType;
    [SerializeField] GameObject iTile;
    [SerializeField] GameObject [,] iTileArray;
    int iLimit = 8;
    int iCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        iMapType = 0;
        iTileArray = new GameObject[iLimit,iLimit];

        //int iTemp = Screen.width / iLimit;
        this.transform.position = new Vector3(this.transform.position.x - 10, this.transform.position.y - 10,0);
        //Debug.Log($"{Screen.width}, {iTemp}");


        CreateMap(iMapType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMap(int type)
    {
        for (int i = 0; i < iLimit; i++)
        {
            for (int j = 0; j < iLimit; j++)
            {
                CreateTile(0, i, j);
            }
        }
    }

    public void CreateTile(int i, int x, int y)
    {
        float fTemp = 2.5f;
            
        Vector3 vTemp = this.transform.position + new Vector3(x * fTemp, y * fTemp, 0);

        GameObject instance = Instantiate(iTile, vTemp, Quaternion.identity);
        instance.transform.SetParent(this.transform);
        instance.GetComponent<Tile>().SetInit(iCount++, vTemp.x, vTemp.y);
        iTileArray[x,y] = instance;
    }
}
