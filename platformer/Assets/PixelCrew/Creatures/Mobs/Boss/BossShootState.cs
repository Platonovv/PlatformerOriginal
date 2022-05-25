using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Boss.Tentacles;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossShootState : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.LaunchProjectiles();


            //var spawners = animator.GetComponent<TentacleAI>();
           // spawners.Laun
        }
    }
}
