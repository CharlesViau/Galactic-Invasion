using UnityEngine;

namespace Motherbase
{
    public class ShieldPreview : MonoBehaviour
    {
        [SerializeField] private CoreMotherBase mb;

        [SerializeField] private Hand _hand;
        private Color _baseColor;
        private Controller _controller;
        private int _index;
        private Material _material;
        private PlayerCurrency _playerCurrency;
        private Color _selectedColor;

        // Start
        private void Start()
        {
            _playerCurrency = PlayerCurrency.Instance;
            _controller = Controller.Instance;
            _baseColor = new Color((float)188 / 255, (float)173 / 255, (float)173 / 255, 1f);
            _selectedColor = new Color((float)102 / 255, (float)212 / 255, (float)75 / 255, 1f);
            _material = gameObject.GetComponent<Renderer>().material;
        }

        private void OnMouseDown()
        {
            if (Menu_ingame.Instance.IsPaused) return;

            if (!_playerCurrency.SpendMoney(_playerCurrency.shieldCost)) return;
            mb.SpawnShield(_index);
            gameObject.SetActive(false);
            _hand.OnPlaceShield();
            Controller.Instance.GameStart();
        }

        private void OnMouseEnter()
        {
            if (Menu_ingame.Instance.IsPaused) return;

            _material.color = _selectedColor;
        }

        private void OnMouseExit()
        {
            if (Menu_ingame.Instance.IsPaused) return;

            _material.color = _baseColor;
        }

        public void SetIndex(int pIndex)
        {
            _index = pIndex;
        }
    }
}