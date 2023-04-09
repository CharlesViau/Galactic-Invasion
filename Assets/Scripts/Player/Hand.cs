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
    [SerializeField] private TowerAbilityButton towerAbilityButtons;
    private Animator _animator;
    private CurrentAbility _currentAbility;
    private bool _isFirstShieldPlaced;
    private bool _isPlacingAbilty;
    private bool _isRepairing;
    private bool _isShowingPreview;
    private bool _isUpgrading;
    private CoreMotherBase _mb;
    private Menu_ingame _menuInGame;
    private GameObject _previewRef;
    private List<Shield> _shields;

    private void Start()
    {
        _mb = FindObjectOfType<CoreMotherBase>();
        _animator = GetComponent<Animator>();
        _shields = _mb.GetShieldList();
        _menuInGame = Menu_ingame.Instance;

        foreach (var s in _shields) s.deathEvent += OnShieldDestroy;
    }

    private void Update()
    {
        if (_menuInGame.IsPaused) return;

        var currentPos = Input.mousePosition;
        if (Camera.main != null)
        {
            var worldPos =
                Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x - 80, currentPos.y + 20,
                    -Camera.main.transform.position.z));
            worldPos.z = 0;
            playerTransform.position = worldPos;
        }

        if (_isPlacingAbilty)
        {
            var position = referenceTransform.position;
            _previewRef.transform.position =
                new Vector3(position.x, position.y - 10);
            if (Input.GetMouseButtonDown(0))
                if (_previewRef.GetComponent<SpellUI>().CanBePlaced())
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
                _isPlacingAbilty = false;
                Destroy(_previewRef.gameObject);
            }
        }
        else if (_isShowingPreview)
        {
            if (!Input.GetMouseButtonDown(1)) return;
            _isRepairing = false;
            _isShowingPreview = false;
            _mb.ShowShieldsPreview(_isShowingPreview);
            _animator.SetTrigger("Cancel");
        }
        else if (_isRepairing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isRepairing = false;
                _mb.ShieldsSelectable(false);
                _animator.SetTrigger("Cancel");
            }

            if (Input.GetMouseButtonDown(0)) OnRepair();
        }
        else if (_isUpgrading)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isUpgrading = false;
                _mb.ShieldsSelectable(false);
                _animator.SetTrigger("Cancel");
            }

            if (Input.GetMouseButtonDown(0)) OnUpgrade();
        }
    }

    private void OnDestroy()
    {
        foreach (var s in _shields) s.deathEvent -= OnShieldDestroy;
    }

    public void OnPlaceShieldAbilityClick()
    {
        if (PlayerCurrency.Instance.Balance < PlayerCurrency.Instance.shieldCost)
        {
            MessageUI.Instance.Show("Not enough money!");
            return;
        }

        _isShowingPreview = true;
        if (!_isFirstShieldPlaced)
            _mb.ShowFirstShieldPreview(true);
        else
            _mb.ShowShieldsPreview(_isShowingPreview);
        _animator.SetTrigger("Shoot");
    }

    public void OnPlaceShield()
    {
        _isFirstShieldPlaced = true;
        _animator.SetTrigger("Punch");
        _isShowingPreview = false;
        towerAbilityButtons.towerAbilityPurchased = true;
        towerAbilityButtons.UpdateOtherAbilityButtons();
    }

    public void OnRepairClick()
    {
        if (PlayerCurrency.Instance.Balance < PlayerCurrency.Instance.repairCost)
        {
            MessageUI.Instance.Show("Not enough money!");
            return;
        }

        _mb.ShieldsSelectable(true);
        _animator.SetTrigger("Click");
        _isRepairing = true;
    }

    private void OnRepair()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            if (hit.transform.CompareTag("Shield"))
            {
                if (hit.transform.gameObject.GetComponent<Shield>().IsMaxHp()) return;
                if (!PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.repairCost)) return;

                hit.transform.gameObject.GetComponent<Shield>().Repair();
                _isRepairing = false;
                _animator.SetTrigger("Punch");
                _mb.ShieldsSelectable(false);
            }
    }

    public void OnUpgradeClick()
    {
        if (PlayerCurrency.Instance.Balance < PlayerCurrency.Instance.upgradeCost)
        {
            MessageUI.Instance.Show("Not enough money!");
            return;
        }

        _mb.ShieldsSelectable(true, true);
        _animator.SetTrigger("Click");
        _isUpgrading = true;
    }

    private void OnUpgrade()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            if (hit.transform.CompareTag("Shield"))
            {
                if (hit.transform.gameObject.GetComponent<Shield>().IsMaxLvl()) return;
                if (!PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.upgradeCost)) return;

                hit.transform.gameObject.GetComponent<Shield>().Upgrade();
                _isUpgrading = false;
                _animator.SetTrigger("Thumbs up");
                _mb.ShieldsSelectable(false);
            }
    }

    public void OnTempoPlanetAbilityClick()
    {
        _isShowingPreview = false;
        _mb.ShowShieldsPreview(_isShowingPreview);

        if (_isPlacingAbilty) return;
        if (!PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.tempoPlanetCost)) return;
        _animator.SetTrigger("Grab");
        _currentAbility = CurrentAbility.TempoPlanet;
        _isPlacingAbilty = true;
        _previewRef = Instantiate(tempoPreview,
            referenceTransform.position, tempoPreview.transform.rotation);
    }

    private void SpawnTempoPlanet()
    {
        _animator.SetTrigger("Drop");
        _isPlacingAbilty = false;
        Destroy(_previewRef.gameObject);
        Instantiate(tempo, referenceTransform.position, tempo.transform.rotation);
    }

    public void OnBlackHoleAbilityClick()
    {
        _isShowingPreview = false;
        _mb.ShowShieldsPreview(_isShowingPreview);

        if (_isPlacingAbilty) return;
        if (!PlayerCurrency.Instance.SpendMoney(PlayerCurrency.Instance.blackHoleCost)) return;
        _animator.SetTrigger("Grab");
        _currentAbility = CurrentAbility.BlackHole;
        _isPlacingAbilty = true;
        _previewRef = Instantiate(blackHolePreview,
            referenceTransform.position, blackHolePreview.transform.rotation);
    }

    private void SpawnBlackHole()
    {
        _animator.SetTrigger("Drop");
        _isPlacingAbilty = false;
        Destroy(_previewRef.gameObject);
        Instantiate(blackHole, referenceTransform.position, blackHole.transform.rotation);
    }

    private void OnShieldDestroy()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) _animator.SetTrigger("Thumbs down");
    }
}