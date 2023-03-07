using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FreeTeam.BP.Services.Effects
{
    public class ShotTrail : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private LineRenderer lineRenderer = null;
        [SerializeField] private float size = 2f;
        [SerializeField] private float speed = 100f;
        #endregion

        #region Private
        private Vector3 curPosition = Vector3.zero;
        private Vector3 endPosition = Vector3.zero;
        private Vector3 direction = Vector3.zero;

        private readonly Vector3[] positions = new Vector3[2];
        #endregion

        #region Unity methods
        private void Awake() =>
            lineRenderer.positionCount = positions.Length;
        #endregion

        #region Public methods
        public void Emit(Vector3 startPosition, Vector3 endPosition)
        {
            this.endPosition = endPosition;
            direction = (endPosition - startPosition).normalized;
            curPosition = startPosition;

            TrailRoutine();
        }
        #endregion

        #region Coroutines
        private async void TrailRoutine()
        {
            while (Vector3.SqrMagnitude(endPosition - curPosition) > 0)
            {
                curPosition += direction * speed * Time.deltaTime;

                positions[1] = curPosition;
                positions[0] = curPosition - direction * size;

                lineRenderer.SetPositions(positions);

                await UniTask.Yield();
            }

            gameObject.SetActive(false);
        }
        #endregion
    }
}
