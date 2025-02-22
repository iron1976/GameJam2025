using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Placements;
    public List<Transform> SpawnGrids;
    public List<Transform> PlaneGrids;
    public List<Transform> TowerGrids;
    public Vector2 SpawnGridLength;
    public Vector2 PlaneGridLength;
    public Vector2 TowerGridLength;


    [SerializeField] public Transform[][] GetSpawnGrids;
    [SerializeField] public Transform[][] GetPlaneGrids;
    [SerializeField] public Transform[][] GetTowerGrids;

    public void GetSpawnGridByPosition(Vector2 pos)
    {

    }
    void Start()
    {

        GetTowerGrids = new Transform[(int) TowerGridLength.x][];
        for (ushort j = 0; j <  GetTowerGrids.Length; j++)
            GetTowerGrids[j] = new Transform[(int) TowerGridLength.y];

        for (ushort j = 0; j <  TowerGridLength.x; j++)
        {
            for (ushort i = 0; i <  TowerGridLength.y; i++)
            {
                GetTowerGrids[j][i] =  TowerGrids[(int) TowerGridLength.x * i + j];
                Debug.Log( GetTowerGrids[j][i] + " " +  i + " " + j);
            }
        }

        print("plane grid length:  " + (int)PlaneGridLength.x);

        GetPlaneGrids = new Transform[(int) PlaneGridLength.x][];//10
        for (ushort j = 0; j <  GetPlaneGrids.Length; j++)
             GetPlaneGrids[j] = new Transform[(int) PlaneGridLength.y];//7
        GetPlaneGrids[7][0] = PlaneGrids[0];
                                //10
        for (ushort j = 0; j <  PlaneGridLength.x; j++)
        {                               //7
            for (ushort i = 0; i <  PlaneGridLength.y; i++)
            {
                Debug.Log(GetPlaneGrids[j][i] + " " + j + " " + i + "    "   + (PlaneGridLength.x) * (j) );
                GetPlaneGrids[j][i] =  PlaneGrids[(int) (PlaneGridLength.x) * (i) + j];
            }
        }



        GetSpawnGrids = new Transform[(int) SpawnGridLength.x][];
        for (ushort j = 0; j <  GetSpawnGrids.Length; j++)
             GetSpawnGrids[j] = new Transform[(int) SpawnGridLength.y];

        for (ushort j = 0; j <  SpawnGridLength.x; j++)
        {
            for (ushort i = 0; i <  SpawnGridLength.y; i++)
            {
                GetSpawnGrids[j][i] =  SpawnGrids[(int) SpawnGridLength.x * i + j];
                Debug.Log( GetSpawnGrids[j][i] + " " + i + " " + j);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class ResourceLoaderGUIClass : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        GameManager main = (GameManager)(target);
        if (GUILayout.Button("Refresh Prefabs", GUILayout.Width(400), GUILayout.Height(100)))
        {
            Transform spawnGrids = main.Placements.transform.Find("Spawn Grids");
            main.SpawnGrids = new List<Transform>();
            foreach (Transform item in spawnGrids)
                main.SpawnGrids.Add(item);


            Transform planeGrids = main.Placements.transform.Find("Plane Grids");
            main.PlaneGrids = new List<Transform>();
            foreach (Transform item in planeGrids)
                main.PlaneGrids.Add(item);


            Transform towerGrids = main.Placements.transform.Find("Tower Grids");
            main.TowerGrids = new List<Transform>();
            foreach (Transform item in towerGrids)
                main.TowerGrids.Add(item);





            Debug.Log("Game Manager Objects Refreshed!");

        }
        EditorGUI.EndChangeCheck();
    }
}
#endif