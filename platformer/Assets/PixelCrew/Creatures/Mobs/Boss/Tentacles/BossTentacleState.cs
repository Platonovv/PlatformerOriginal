using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.Tentacles
{
    public class BossTentacleState: StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<TentaclesProjectileSpawner1>();
            spawner.LaunchTentacles();
            
            var spawners = animator.GetComponent<TentaclesProjectileSpawner2>();
            spawners.LaunchTentacles();
        }
    }
}