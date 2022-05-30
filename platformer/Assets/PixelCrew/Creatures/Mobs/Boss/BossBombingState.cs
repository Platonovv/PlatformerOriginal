using PixelCrew.Creatures.Mobs.Boss.Bomb;
using PixelCrew.Creatures.Mobs.Boss.BossCrabby;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossBombingState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<BombController>();
            spawner.StartBombing();
        }
    }
}