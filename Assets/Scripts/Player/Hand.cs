using Motherbase;
using UnityEngine;

internal enum CurrentAbility
{
    None,
    BlackHole,
    TempoPlanet
}

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform referenceTransform;
    [SerializeField] private GameObject blackHolePreview;
    [SerializeField] private GameObject blackHole;
    [SerializeField] private GameObject tempoPreview;
    [SerializeField] private GameObject tempo;

    private CurrentAbility _currentAbility;
    private CoreMotherBase _mb;

    private bool isPlacingAbilty;
    private bool isShowingPreview;

    private GameObject previewRef;

    private void Start()
    {
        _mb = FindObjectOfType<CoreMotherBase>();
    }

    private void Update()
    {
        var currentPos = Input.mousePosition;
        var worldPos =
            Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x, currentPos.y, -Camera.main.transform.position.z));
        worldPos.z = 0;
        playerTransform.position = worldPos;

        if (isPlacingAbilty)
        {
            previewRef.transform.position =
                new Vector3(referenceTransform.position.x, referenceTransform.position.y - 10);
            if (Input.GetMouseButtonDown(0))
            {
                switch (_currentAbility)
                {
                    case CurrentAbility.BlackHole:
                        SpawnBlackHole();
                        break;
                    case CurrentAbility.TempoPlanet:
                        SpawnTempoPlanet();
                        break;
                }

                _currentAbility = CurrentAbility.None;
            }
        }
    }

    public void OnRepairShieldAbilityClick()
    {
        isShowingPreview = !isShowingPreview;
        _mb.ShowShieldsPreview(isShowingPreview);
    }

    public void OnTempoPlanetAbilityClick()
    {
        isShowingPreview = false;
        _mb.ShowShieldsPreview(isShowingPreview);

        if (isPlacingAbilty) return;
        if (PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.tempoPlanetCost))
        {
            _currentAbility = CurrentAbility.TempoPlanet;
            isPlacingAbilty = true;
            previewRef = Instantiate(tempoPreview,
                referenceTransform.position, tempoPreview.transform.rotation);
        }
    }

    private void SpawnTempoPlanet()
    {
        isPlacingAbilty = false;
        Destroy(previewRef.gameObject);
        Instantiate(tempo, referenceTransform.position, tempo.transform.rotation);
    }

    public void OnBlackHoleAbilityClick()
    {
        isShowingPreview = false;
        _mb.ShowShieldsPreview(isShowingPreview);

        if (isPlacingAbilty) return;
        if (PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.blackHoleCost))
        {
            _currentAbility = CurrentAbility.BlackHole;
            isPlacingAbilty = true;
            previewRef = Instantiate(blackHolePreview,
                referenceTransform.position, blackHolePreview.transform.rotation);
        }
    }

    private void SpawnBlackHole()
    {
        isPlacingAbilty = false;
        Destroy(previewRef.gameObject);
        Instantiate(blackHole, referenceTransform.position, blackHole.transform.rotation);
    }
}