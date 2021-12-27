using DG.Tweening;
using UnityEngine;

namespace _Tween.Scripts.Examples.DOTween
{
    public class TweenCubePath : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private PathType _pathType = PathType.Linear;
        [SerializeField] private Transform[] _points;


        [ContextMenu(nameof(Play))]
        public void Play()
        {
            Vector3[] positions = CreatePositions(_points);
            transform.DOPath(positions, _duration, _pathType);
        }

        private Vector3[] CreatePositions(Transform[] points)
        {
            int length = points.Length;
            var positions = new Vector3[length];

            for (int i = 0; i < length; i++)
                positions[i] = points[i].position;

            return positions;
        }
    }
}
