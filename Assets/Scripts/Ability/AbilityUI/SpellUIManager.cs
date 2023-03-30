using System.Collections.Generic;
using Core;
using Player;
using UnityEngine.UI;

namespace Ability.AbilityUI
{
    public class SpellUIManager : MonoBehaviourManager<SpellUI, SpellUIType, SpellUI.Args, SpellUIManager>
    {
        private Button _selectedButton;
        public List<Button> spellButtons;

        protected override string PrefabLocation => "Prefabs/SpellUIType/";

        private void Start()
        {
            foreach (var button in spellButtons) button.onClick.AddListener(() => OnSpellButtonClicked(button));
        }

        private void OnSpellButtonClicked(Button clickedButton)
        {
            SelectButton(clickedButton);
            EnableAbility(clickedButton);
        }

        private void EnableAbility(Button button)
        {
            // AbilityHandler.Instance.OnAbilityPressEvent();
        }

        private void SelectButton(Button button)
        {
            _selectedButton = button;
        }
    }
}