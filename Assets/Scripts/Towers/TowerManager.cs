using Core;

namespace Towers
{
    public class TowerManager : MonoBehaviourManager<Tower,TowerTypes, Tower.Args, TowerManager>
    {
        protected override string PrefabLocation => "Prefabs/Towers/";
    }
}