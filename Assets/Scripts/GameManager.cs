using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public GameObject TowerLightPrefab;
    public GameObject SelectedPrefab;
    public GameObject Placements; 
    public GameObject CrossObject;
    public GameObject PlayerPrefab;
    public GameObject HealthBarPrefab;


    public Vector2Int SpawnGridSize;
    public Vector2Int PlaneGridSize;
    public Vector2Int TowerGridSize;

    public List<LightTower> LightTowers;
    public List<AttackTower> AttackTowers;
    public List<Fence> Fences;








    public List<Transform> SpawnGrids;
    public List<Transform> PlaneGrids;
    public List<Transform> TowerGrids;

     
    public List<Transform> PlaneIsSelectedGrids;
    public List<Transform> TowerIsSelectedGrids;


    /// <summary>
    /// x: Increases to right, y: Increases to bottom.
    /// </summary>
    public Transform[][] GetSpawnGrids;
    /// <summary>
    /// x: Increases to right, y: Increases to bottom.
    /// </summary>
    public Transform[][] GetPlaneGrids;
    /// <summary>
    /// x: Increases to right, y: Increases to bottom.
    /// </summary>
    public Transform[][] GetTowerGrids;
    public Dictionary<Transform, Vector2Int> GetSpawnGridPosition = new Dictionary<Transform, Vector2Int>();
    public Dictionary<Transform, Vector2Int> GetPlaneGridPosition = new Dictionary<Transform, Vector2Int>();
    public Dictionary<Transform, Vector2Int> GetTowerGridPosition = new Dictionary<Transform, Vector2Int>();


    Transform MouseChosenGrid;
    Player CurrentPlayer;



    public List<GridEntity> SpawnedTowerEntities;
    public List<GridEntity> SpawnedPlaneEntities;

    public enum GridPositionEnum : ushort
    {
        NULL = 0,
        SpawnGrids = 1,
        PlaneGrids = 2,
        TowerGrids = 3,
    }


    
    void InitializeGrids()
    {

        GetTowerGrids = new Transform[(int) TowerGridSize.x][];
        for (ushort j = 0; j <  GetTowerGrids.Length; j++)
            GetTowerGrids[j] = new Transform[(int) TowerGridSize.y];

        for (ushort j = 0; j < TowerGridSize.x; j++)
            for (ushort i = 0; i < TowerGridSize.y; i++)
            {
                //Debug.Log(GetTowerGrids[j][i] + " " + j + " " + i); 
                GetTowerGrids[j][i] = TowerGrids[(int)TowerGridSize.x * i + j];
                GetSpawnGridPosition[GetTowerGrids[j][i]] = new Vector2Int(j, i); 
            } 
         

        GetPlaneGrids = new Transform[(int) PlaneGridSize.x][];//10
        for (ushort j = 0; j <  GetPlaneGrids.Length; j++)
             GetPlaneGrids[j] = new Transform[(int) PlaneGridSize.y];//7 

        for (ushort j = 0; j <  PlaneGridSize.x; j++) 
            for (ushort i = 0; i <  PlaneGridSize.y; i++)
            {
                //Debug.Log(GetPlaneGrids[j][i] + " " + j + " " + i + "    "   + (PlaneGridSize.x) * (j) );
                GetPlaneGrids[j][i] =  PlaneGrids[(int) (PlaneGridSize.x) * (i) + j];
                GetPlaneGridPosition[GetPlaneGrids[j][i]] = new Vector2Int(j, i); 
            } 



        GetSpawnGrids = new Transform[(int) SpawnGridSize.x][];
        for (ushort j = 0; j <  GetSpawnGrids.Length; j++)
             GetSpawnGrids[j] = new Transform[(int) SpawnGridSize.y];

        for (ushort j = 0; j <  SpawnGridSize.x; j++) 
            for (ushort i = 0; i <  SpawnGridSize.y; i++)
            {
                //Debug.Log(GetSpawnGrids[j][i] + " " + j + " " + i);
                GetSpawnGrids[j][i] =  SpawnGrids[(int) SpawnGridSize.x * i + j];
                GetSpawnGridPosition[GetSpawnGrids[j][i]] = new Vector2Int(j, i); 
            } 

    }
    void GridDetection()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Interactable Grid")
            {
                if (MouseChosenGrid == null)
                {
                    MouseChosenGrid = hit.collider.transform;
                    MouseChosenGrid.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (MouseChosenGrid != hit.collider.transform)
                {
                    MouseChosenGrid.transform.GetChild(0).gameObject.SetActive(false);
                    MouseChosenGrid = hit.collider.transform;
                    MouseChosenGrid.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else if (MouseChosenGrid != null)
        {
            {
                MouseChosenGrid.transform.GetChild(0).gameObject.SetActive(false);
                MouseChosenGrid = null;
            }
        }

    }

    private void Awake()
    {
        main = this;
    }

    void SpawnPlayer()
    {
        CurrentPlayer = Instantiate(PlayerPrefab, new Vector3(0, 0, -1), Quaternion.identity).GetComponent<Player>();
        CurrentPlayer.Speed = 2;


    }
    /// <summary>
    /// Spawn for Tower Grid.
    /// </summary>
    /// <param name="Building"></param>
    /// <param name="Position"></param>
    GridEntity SpawnBuilding(GridEntity Building, Vector2Int Position)
    {

        GridEntity ClonedEntityGrid = Building.CloneThis();

        ClonedEntityGrid.GridSpriteRenderer = GetTowerGrids[Position.x][Position.y].GetComponent<SpriteRenderer>();
      


        if (ClonedEntityGrid.GetType() == typeof(AttackTower))
        {

        }
        else if (ClonedEntityGrid.GetType() == typeof(LightTower))
        {
            ((LightTower)ClonedEntityGrid).Light = Instantiate(TowerLightPrefab, ClonedEntityGrid.GridSpriteRenderer.transform).GetComponent<Light2D>();
        }
        else if(ClonedEntityGrid.GetType() == typeof(Fence))
        {

        }
         
        ClonedEntityGrid.HealthBar = Instantiate(HealthBarPrefab, ClonedEntityGrid.GridSpriteRenderer.transform).transform;

        SpawnedTowerEntities.Add(ClonedEntityGrid);
        CallStartForGridEntities();


        return ClonedEntityGrid;

    }

    /// <summary>
    /// Spawn for Plane Grid.
    /// </summary>
    /// <param name="Entity"></param>
    /// <param name="Position"></param>
    void SpawnEntities(GridEntity Entity, Vector2Int Position)
    {

        GridEntity ClonedEntityGrid = Entity.CloneThis();

        SpriteRenderer GridSprite = GetPlaneGrids[Position.x][Position.y].GetComponent<SpriteRenderer>();
        GridSprite.sprite = ClonedEntityGrid.Sprite;

        SpawnedPlaneEntities.Add(ClonedEntityGrid); 

        if (ClonedEntityGrid.GetType() == typeof(AttackTower))
        {

        }
        else if (ClonedEntityGrid.GetType() == typeof(LightTower))
        {

        }
        else if (ClonedEntityGrid.GetType() == typeof(Fence))
        {

        }
         

    }

    void MoveToCursorPoint()
    {
        if(Input.GetButtonDown("Fire1"))
        {
             
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] arr = Physics2D.LinecastAll(CurrentPlayer.transform.position, mouseWorldPos);
            Debug.DrawLine(CurrentPlayer.transform.position, mouseWorldPos, Color.red, 1);
            Vector2 crossObjectPos = Vector2.zero; 
                for (byte j = 0; j < arr.Length; j++)
                {
                if ((!arr[arr.Length - j - 1].collider.isTrigger))
                {
                    if (arr[arr.Length - j - 1].collider.tag == "Walls")
                    {
                        crossObjectPos = arr[arr.Length - j - 1].point;
                        float extraSpace = 0.3f;
                        if (arr[arr.Length - j - 1].collider.name == "Top Collider")
                        {
                            print("TOP DETECTED");
                            crossObjectPos += new Vector2(0, -extraSpace); 
                        }
                        else if (arr[arr.Length - j - 1].collider.name == "Bottom Collider")
                        {
                            print("BOTTOM DETECTED");
                            crossObjectPos += new Vector2(0, extraSpace);
                        }
                        else if (arr[arr.Length - j - 1].collider.name == "Left Collider")
                        {
                            print("LEFT DETECTED");
                            crossObjectPos += new Vector2(extraSpace, 0);
                        }
                        else if (arr[arr.Length - j - 1].collider.name == "Right Collider")
                        {
                            print("RIGHT DETECTED");
                            crossObjectPos += new Vector2(-extraSpace, 0);
                        }
                    }
                    else if (arr[arr.Length - j - 1].collider.tag == "Interactable Grid")
                    {
                        crossObjectPos = arr[arr.Length - j - 1].point;
                        Vector2 direction = (arr[arr.Length - j - 1].point - (Vector2)arr[arr.Length - j - 1].transform.position).normalized;
                        float extraSpace = 0.2f;
                        if (direction.x > 0 && direction.y > 0)//x+ y+
                        {
                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                crossObjectPos += new Vector2(extraSpace, 0);
                            else
                                crossObjectPos += new Vector2(0, extraSpace);
                        }
                        else if (direction.x > 0 && direction.y < 0)//x+ , y-
                        {
                            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                crossObjectPos += new Vector2(extraSpace, 0);
                            else
                                crossObjectPos += new Vector2(0, -extraSpace);
                        }
                        else if (direction.x < 0 && direction.y < 0)//x- y-
                        {

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                crossObjectPos += new Vector2(-extraSpace, 0);
                            else
                                crossObjectPos += new Vector2(0, -extraSpace);
                        }
                        else if (direction.x < 0 && direction.y > 0)//x- y+
                        {

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                crossObjectPos += new Vector2(-extraSpace, 0);
                            else
                                crossObjectPos += new Vector2(0, extraSpace);
                        } 
                    }
                }
            }
            if(crossObjectPos == Vector2.zero)
                crossObjectPos = mouseWorldPos;

            CrossObject.transform.position = new Vector3(crossObjectPos.x, crossObjectPos.y, CrossObject.transform.position.z);
            CrossObject.GetComponent<Animator>().Play("Cross Animation Start");
            CrossObject.gameObject.SetActive(true);

            CurrentPlayer.SetTargetPosition(crossObjectPos);
        }
    }
    public void RemoveCursorPoint()
    {

        CrossObject.gameObject.SetActive(false);
    }
    void CallUpdatesForGridEntities()
    {
        for (ushort j = 0; j < GameManager.main.SpawnedTowerEntities.Count; j++)
            GameManager.main.SpawnedTowerEntities[j].UpdateBase();

        for (ushort j = 0; j < GameManager.main.SpawnedPlaneEntities.Count; j++)
            GameManager.main.SpawnedPlaneEntities[j].UpdateBase();
    }
    void CallStartForGridEntities()
    {

        for (ushort j = 0; j < GameManager.main.SpawnedTowerEntities.Count; j++)
            GameManager.main.SpawnedTowerEntities[j].StartBase();

        for (ushort j = 0; j < GameManager.main.SpawnedPlaneEntities.Count; j++)
            GameManager.main.SpawnedPlaneEntities[j].StartBase();
    }
    
    void CallFixedUpdateForGridEntities()
    { 
        for (ushort j = 0; j < GameManager.main.SpawnedTowerEntities.Count; j++)
            GameManager.main.SpawnedTowerEntities[j].FixedUpdateBase();

        for (ushort j = 0; j < GameManager.main.SpawnedPlaneEntities.Count; j++)
            GameManager.main.SpawnedPlaneEntities[j].FixedUpdateBase();
    }
    private void Start()
    {
        InitializeGrids();
        GridEntity s = SpawnBuilding(AttackTowers[0], new Vector2Int(1, 2));

        SpawnPlayer();

    }
    private void FixedUpdate()
    {
        CallFixedUpdateForGridEntities();

    }
    void Update()
    {
        MoveToCursorPoint();
        CallUpdatesForGridEntities();
        GridDetection();
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
            {
                main.SpawnGrids.Add(item);
                item.tag = "Grid";
                item.GetComponent<BoxCollider2D>().isTrigger = true;
            }


            Transform planeGrids = main.Placements.transform.Find("Plane Grids");
            main.PlaneGrids = new List<Transform>();
            main.TowerIsSelectedGrids = new List<Transform>();
            foreach (Transform item in planeGrids)
            {
                main.PlaneGrids.Add(item);
                item.tag = "Interactable Grid";
                item.GetComponent<BoxCollider2D>().isTrigger = true;
                Transform IsSelected = item.Find("Is Selected"); 
                if (IsSelected == null)
                    IsSelected = Instantiate(main.SelectedPrefab, item).transform;

                IsSelected.name = "Is Selected";
                IsSelected.gameObject.SetActive(false);
                IsSelected.transform.localPosition = new Vector3(0, 0, 0.2f);
                IsSelected.transform.localScale = new Vector3(0.195063f, 0.1933868f, 1);
                main.PlaneIsSelectedGrids.Add(IsSelected.transform);
                 
            }



            Transform towerGrids = main.Placements.transform.Find("Tower Grids");
            main.TowerGrids = new List<Transform>();

            main.TowerIsSelectedGrids = new List<Transform>();
            foreach (Transform item in towerGrids)
            {
                main.TowerGrids.Add(item);
                item.tag = "Interactable Grid";
                item.GetComponent<BoxCollider2D>().isTrigger = true;
                Transform IsSelected = item.Find("Is Selected"); 
                if (IsSelected == null)
                    IsSelected = Instantiate(main.SelectedPrefab, item).transform; 

                IsSelected.name = "Is Selected";
                IsSelected.gameObject.SetActive(false);
                IsSelected.transform.localPosition = new Vector3(0, 0, 0.2f);
                IsSelected.transform.localScale = new Vector3(0.195063f, 0.1933868f, 1);
                main.TowerIsSelectedGrids.Add(IsSelected.transform);
                 

                

            }





            Debug.Log("Game Manager Objects Refreshed!");

        }
        EditorGUI.EndChangeCheck();
    }
}
#endif