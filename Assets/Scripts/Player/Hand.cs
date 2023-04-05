using System.Collections.Generic;
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
    
    private Animator _animator;
    private CurrentAbility _currentAbility;
    private CoreMotherBase _mb;

    private bool isPlacingAbilty;
    private bool isShowingPreview;
    private bool isRepairing;

    private GameObject previewRef;

    private List<Shield> shields;

    private void Start()
    {
        _mb = FindObjectOfType<CoreMotherBase>();
        _animator = GetComponent<Animator>();
        shields = _mb.GetShieldList();
        
        foreach (Shield s in shields)
        {
            s.deathEvent += OnShieldDestroy;
        }
    }

    private void Update()
    {
        var currentPos = Input.mousePosition;
        var worldPos =
            Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x - 70, currentPos.y + 15, -Camera.main.transform.position.z));
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

            if (Input.GetMouseButtonDown(1))
            {
                switch (_currentAbility)
                {
                    case CurrentAbility.BlackHole:
                        PlayerCurrency.Instance.AddMoney(PlayerCurrency.Instance.blackHoleCost);
                        break;
                    case CurrentAbility.TempoPlanet:
                        PlayerCurrency.Instance.AddMoney(PlayerCurrency.Instance.tempoPlanetCost);
                        break;
                }
                _currentAbility = CurrentAbility.None;
                _animator.SetTrigger("Cancel");
                isPlacingAbilty = false;
                Destroy(previewRef.gameObject);
            }
        } else if (isShowingPreview)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isRepairing = false;
                isShowingPreview = false;
                _mb.ShowShieldsPreview(isShowingPreview);
                _animator.SetTrigger("Cancel");
            }
        } else if (isRepairing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isRepairing = false;
                _animator.SetTrigger("Cancel");
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnRepair();
            }
        }
    }

    public void OnPlaceShieldAbilityClick()
    {
        if (PlayerCurrency.Instance.Balance < PlayerCurrency.Instance.shieldCost)
        {
            MessageUI.Instance.Show("Not enough money!");
            return;
        }
        isShowingPreview = true;
        _mb.ShowShieldsPreview(isShowingPreview);
        _animator.SetTrigger("Shoot");
    }

    public void OnRepairClick()
    {
        if (PlayerCurrency.Instance.Balance < PlayerCurrency.Instance.repairCost)
        {
            MessageUI.Instance.Show("Not enough money!");
            return;
        }
        _animator.SetTrigger("Click");
        isRepairing = true;
    }

    private void OnRepair()
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
            if (hit.transform.CompareTag("Shield"))
            {
                hit.transform.gameObject.GetComponent<Shield>().Repair();
                isRepairing = false;
                _animator.SetTrigger("Punch");
            }
        }
    }

    public void OnPlaceShield()
    {
        _animator.SetTrigger("Punch");
        isShowingPreview = false;
    }

    public void OnUpgradeClick()
    {
        if (!_mb.IsMaxLVL() && PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.upgradeCost))
        {
            _mb.UpgradeShields();
            _animator.SetTrigger("Thumbs up");
        }
    }

    public void OnTempoPlanetAbilityClick()
    {
        isShowingPreview = false;
        _mb.ShowShieldsPreview(isShowingPreview);

        if (isPlacingAbilty) return;
        if (PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.tempoPlanetCost))
        {
            _animator.SetTrigger("Grab");
            _currentAbility = CurrentAbility.TempoPlanet;
            isPlacingAbilty = true;
            previewRef = Instantiate(tempoPreview,
                referenceTransform.position, tempoPreview.transform.rotation);
        }
    }

    private void SpawnTempoPlanet()
    {
        _animator.SetTrigger("Drop");
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
            _animator.SetTrigger("Grab");
            _currentAbility = CurrentAbility.BlackHole;
            isPlacingAbilty = true;
            previewRef = Instantiate(blackHolePreview,
                referenceTransform.position, blackHolePreview.transform.rotation);
        }
    }

    private void SpawnBlackHole()
    {
        _animator.SetTrigger("Drop");
        isPlacingAbilty = false;
        Destroy(previewRef.gameObject);
        Instantiate(blackHole, referenceTransform.position, blackHole.transform.rotation);
    }

    private void OnShieldDestroy()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.SetTrigger("Thumbs down");
        }
    }
    
    private void OnDestroy()
    {
        foreach (Shield s in shields)
        {
            s.deathEvent -= OnShieldDestroy;
        }
    }
}