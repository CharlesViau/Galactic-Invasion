using Core;

namespace Ability.AbilityUI
{
    public class SpellUIManager : MonoBehaviourManager<SpellUI, SpellUIType, SpellUI.Args, SpellUIManager>
    {
        protected override string PrefabLocation => "Prefabs/SpellUIType/";
    }
}
