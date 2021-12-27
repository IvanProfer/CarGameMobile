using DG.Tweening;
using UnityEngine;

namespace _Tween.Scripts.Examples.DOTween
{
    public class TweenCubeMove : MonoBehaviour
    {
        [SerializeField] private bool _reverse;
        [SerializeField] private float _duration;
        [SerializeField] private Vector3 _endPosition;


        [ContextMenu(nameof(Play))]
        public void Play()
        {
            if (_reverse)
                transform.DOMove(_endPosition, _duration).From();
            else
                transform.DOMove(_endPosition, _duration);
        }
    }
}
