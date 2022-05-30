using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.BossCrabby
{
    public class BossBombingStates : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<BombControllerAndPlatform>();
            spawner.StartBombing();
        }
    }
}