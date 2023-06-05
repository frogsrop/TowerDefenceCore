using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;

public class DragElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    private Entity _entity;
    private EntityManager _entityManager;
    private float3 _posSpawn;
    
    [SerializeField] private GameObject TowerImgPrefab;
    [SerializeField] private GameObject GreenSquare;
    [SerializeField] private GameObject RedSquare;
    [SerializeField] private int lengtnGrid;
    [SerializeField] private int widthGrid;
    [SerializeField] private int spacingGrid;
    [SerializeField] private Vector2 posGrid;
    private Vector2[,] arrayGridElements;
    private bool[,] arrayControllGrid;

    private Vector2 posActive;
    private int indexBoolI;
    private int indexBoolJ;
    
    
    void Start()
    {
        arrayGridElements = new Vector2[lengtnGrid,widthGrid];
        arrayControllGrid = new bool[lengtnGrid,widthGrid];
        // 0.0 = 0.0
        // 0.1 = 0.3
        // 1.0 = 3.0
        // 1.1 = 3.3 
        // 2.0 = 6.0
        // 2.1 = 6.3
        // 3.0 = 9.0
        // 3.1 = 9.3
        
        // i.j = x.y
        // var ShagSetki = 3
        //Vector2 forArray = new Vector2( i*ShagSetki, j*ShagSetki);
        //Debug.Log("Start");
        //GreenSquare.SetActive(false);
        //Debug.Log("SetActive = false");
        for (int i = 0; i < arrayGridElements.GetLength(0); i++)
        {
            for (int j = 0; j < arrayGridElements.GetLength(1); j++)
            {
                arrayGridElements[i, j] = new Vector2( i*spacingGrid, j*spacingGrid);
                arrayControllGrid[i, j] = true;
            }
        }
        
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Update()
    {
        TowerImgPrefab.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, 1));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Instantiate(TowerImgPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        var roundHit = new Vector2(Mathf.Round(dragPoint.x), Mathf.Round(dragPoint.y));

        for (int i = 0; i < arrayGridElements.GetLength(0); i++)
        {
            for (int j = 0; j < arrayGridElements.GetLength(1); j++)
            {
                if (arrayGridElements[i, j] == roundHit && arrayControllGrid[i, j] == true)
                {
                    posActive = arrayGridElements[i, j];
                    GreenSquare.transform.position = new Vector3(posActive.x, posActive.y, 0);
                    indexBoolI = i;
                    indexBoolJ = j;
                }
                if (arrayGridElements[i, j] == roundHit && arrayControllGrid[i, j] == false)
                {
                    posActive = arrayGridElements[i, j];
                    RedSquare.transform.position = new Vector3(posActive.x, posActive.y, 0);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GreenSquare.transform.position = new Vector3(100, 100, 0);
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        RaycastHit2D hit = Physics2D.Raycast( worldPoint, Vector2.zero );
        if (RedSquare.transform.position.x != 100) return;
        if ( hit.collider != null )
        {
            Debug.Log("hit.collider");
            _posSpawn = new float3(worldPoint.x, worldPoint.y, 0);
            //if (_posSpawn.y > -1 || _posSpawn.y < -5 || _posSpawn.x < -9 || _posSpawn.x > 1 ) return;
            _entity = _entityManager.CreateEntityQuery(typeof(PrefabComponent)).GetSingletonEntity();
            var spawnPosComponent = new SpawnComponent
            {
                TowerPos = _posSpawn,
                OnOff = true
            };
            _entityManager.SetComponentData(_entity, spawnPosComponent);
            
            arrayControllGrid[indexBoolI, indexBoolJ] = false;
            var test = arrayControllGrid[indexBoolI, indexBoolJ];
            Debug.Log(test + " "+ indexBoolI +" "+ indexBoolJ);
        }
    }

}
