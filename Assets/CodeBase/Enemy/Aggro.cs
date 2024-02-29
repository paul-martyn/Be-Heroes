using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public float Delay = 0f;
        
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Agent _agent;

        private bool _hasAggroTarget = false;
        private Coroutine _agroOffCoroutine;


        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            SwitchFollowOff();
        }

        private void TriggerEnter(Collider obj)
        {
            if (!_hasAggroTarget)
            {
                _hasAggroTarget = true;
                StopAgroOffCoroutine();
                SwitchFollowOn();
            }
        }

        private void TriggerExit(Collider obj)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;
                _agroOffCoroutine = StartCoroutine(SwitchFollowOffAfterDelay(Delay));
            }
        }
        
        private void StopAgroOffCoroutine()
        {
            if (_agroOffCoroutine != null)
                StopCoroutine(_agroOffCoroutine);
        }

        private IEnumerator SwitchFollowOffAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SwitchFollowOff();
        }

        private void SwitchFollowOff() => 
            _agent.enabled = false;

        private void SwitchFollowOn() => 
            _agent.enabled = true;
    }
}