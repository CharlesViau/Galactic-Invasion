using Enemies;
using Motherbase;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private WaveManager _waveManager;
    private static bool IsShowingPreview;
    private CoreMotherBase _mb;

    private Plane _plane;

    public bool gameStarted = false;
    
    public static Controller Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _waveManager = gameObject.GetComponent<WaveManager>();
        _plane = new Plane(Vector3.back, 0);
        _mb = FindObjectOfType<CoreMotherBase>();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.W))
        {
            IsShowingPreview = !IsShowingPreview;
            _mb.ShowShieldsPreview(IsShowingPreview);
        }*/
    }

    public void GameStart()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            _waveManager.GameStart();
        }
    }

    private Vector3 GetWorldPosition()
    {
        if (Camera.main == null) return Vector3.zero;
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        return _plane.Raycast(mouseRay, out var distance) ? mouseRay.GetPoint(distance) : Vector3.zero;
    }

    private void MotherBaseAbility(int ability)
    {
        if (ability != 3) return;
        IsShowingPreview = !IsShowingPreview;
        _mb.ShowShieldsPreview(IsShowingPreview);
    }
}