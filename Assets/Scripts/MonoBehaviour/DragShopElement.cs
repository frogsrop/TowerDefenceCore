using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class DragShopElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Entity _entityStorage;
    private Entity _entityTower;
    private EntityManager _entityManager;
    private float3 _posSpawn;
    private BlobAssetStore _blobAssetStore;
    
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
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        arrayGridElements = new Vector2[lengtnGrid,widthGrid];
        arrayControllGrid = new bool[lengtnGrid,widthGrid];
        
        for (int i = 0; i < arrayGridElements.GetLength(0); i++)
        {
            for (int j = 0; j < arrayGridElements.GetLength(1); j++)
            {
                arrayGridElements[i, j] = new Vector2( posGrid.x+i*spacingGrid, posGrid.y+j*spacingGrid);
                arrayControllGrid[i, j] = true;
            }
        }
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
                if (arrayGridElements[i, j] == roundHit && arrayControllGrid[i, j])
                {
                    posActive = arrayGridElements[i, j];
                    GreenSquare.transform.position = new Vector3(posActive.x, posActive.y, 0);
                    indexBoolI = i;
                    indexBoolJ = j;
                }
                if (arrayGridElements[i, j] == roundHit && !arrayControllGrid[i, j])
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
        if (RedSquare.transform.position.x != 100) return;
        
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        RaycastHit2D hit = Physics2D.Raycast( worldPoint, Vector2.zero );
        if ( hit.collider != null )
        {
            _posSpawn = new float3(posActive.x, posActive.y, 0);
            _entityStorage = _entityManager.CreateEntityQuery(
                typeof(StoragePrefabsComponent)).GetSingletonEntity();
            
            _entityTower = _entityManager.GetComponentData<StoragePrefabsComponent>(_entityStorage).TowerPrefab;
            
            var towerUniformScaleTransform = new UniformScaleTransform
                { Position = _posSpawn, Scale = 0.5f };
            var setSpawnTowerPosition = new LocalToWorldTransform
                { Value = towerUniformScaleTransform };
            _entityManager.SetComponentData(_entityTower, setSpawnTowerPosition);
            _entityManager.Instantiate(_entityTower);
            arrayControllGrid[indexBoolI, indexBoolJ] = false;
        }
    }

}
