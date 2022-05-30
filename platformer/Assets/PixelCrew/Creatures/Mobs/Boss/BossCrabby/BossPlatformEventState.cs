using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.BossCrabby
{
    public class BossPlatformEventState: StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<PlatformAndSpikeController>();
            spawner.StartBombing();
        }
    }
}