using UnityEngine;

namespace Motherbase
{
    public class ShieldPreview : MonoBehaviour
    {
        [SerializeField] private CoreMotherBase mb;

        [SerializeField] private Hand _hand;
        private int _index;
        private PlayerCurrency _playerCurrency;
        private Controller _controller;

        // Start
        private void Start()
        {
            _playerCurrency = PlayerCurrency.Instance;
            _controller = Controller.Instance;
        }

        private void OnMouseDown()
        {
            if (!_playerCurrency.SpendMoney(_playerCurrency.shieldCost))
            {
                return;
            }
            mb.SpawnShield(_index);
            gameObject.SetActive(false);
            _hand.OnPlaceShield();
            Controller.Instance.GameStart();
            /*CostUI.Instance.Hide();*/
        }

        public void SetIndex(int pIndex)
        {
            _index = pIndex;
        }
    }
}