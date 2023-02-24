using Core;

namespace Projectiles
{
    public class ProjectileManager : MonoBehaviourManager<Projectile, ProjectileTypes, Projectile.Args, ProjectileManager>
    {
        protected override string PrefabLocation => "Prefabs/Projectiles/";
    }
}
