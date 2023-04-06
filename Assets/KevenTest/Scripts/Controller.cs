using Enemies;
using Motherbase;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private static bool _isShowingPreview;
    public bool gameStarted;
    private CoreMotherBase _mb;
    private Plane _plane;
    private WaveManager _waveManager;
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

    public void Reset()
    {
        _waveManager.ClearQueue();
    }

    private void Start()
    {
        _waveManager = gameObject.GetComponent<WaveManager>();
        _plane = new Plane(Vector3.back, 0);
        _mb = FindObjectOfType<CoreMotherBase>();
        Cursor.visible = false;
    }

    public void GameStart()
    {
        if (gameStarted) return;
        gameStarted = true;
        _waveManager.GameStart();
    }
}