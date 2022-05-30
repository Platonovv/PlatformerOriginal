using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.BossCrabby
{
    public class BossSpawnerState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CrabbyProjectileSpawner>();
            spawner.LaunchProjectiles();
            
            var spawners = animator.GetComponent<CrabbyProjectileSpawner2>();
            spawners.LaunchProjectiles();
        }
    }
}