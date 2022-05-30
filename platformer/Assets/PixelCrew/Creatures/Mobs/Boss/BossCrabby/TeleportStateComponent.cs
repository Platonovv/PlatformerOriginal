using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.BossCrabby
{
    public class TeleportStateComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _transform;
        [SerializeField] private PolygonCollider2D _collider;
        
        private Coroutine _coroutine;


        [ContextMenu("teleport!")]
        public void Teleport()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Transform());
        }

        private IEnumerator Transform()
        {
            var target = _collider.GetComponent<PolygonCollider2D>();
            target.enabled = false;
            var newPosition = _rigidbody.position + new Vector2(-21.6f, 10f);
            var invert = new Vector3(-4, 4, 1);
            _rigidbody.MovePosition(newPosition);
            _transform.localScale = invert;

            yield return new WaitForSeconds(1f);
            target.enabled = true;
            _coroutine = null;
        }
    }
}