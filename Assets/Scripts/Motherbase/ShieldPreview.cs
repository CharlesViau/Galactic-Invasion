using UnityEngine;

namespace Motherbase
{
    public class ShieldPreview : MonoBehaviour
    {
        [SerializeField] private CoreMotherBase mb;
        private int _index;
        private PlayerCurrency _playerCurrency;

        // Start
        private void Start()
        {
            _playerCurrency = PlayerCurrency.Instance;
        }

        private void OnMouseDown()
        {
            if (!_playerCurrency.SpendMoney(_playerCurrency.shieldCost))
            {
                MessageUI.Instance.SetText("Not enough money!");
                MessageUI.Instance.Show();
                return;
            }

            mb.SpawnShield(_index);
            gameObject.SetActive(false);
            CostUI.Instance.Hide();
        }

        public void SetIndex(int pIndex)
        {
            _index = pIndex;
        }
    }
}