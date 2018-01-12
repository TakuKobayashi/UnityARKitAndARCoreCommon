using UnityEngine;

namespace UnityARKitAndARCoreCommon
{
    public abstract class ARControllerBase : MonoBehaviour
    {
        [SerializeField] protected Camera mainCamera;
        [SerializeField] protected GameObject TrackedPlanePrefab;
        [SerializeField] private GameObject pointCloudPrefab;

        protected GameObject pointCloudObj;

        protected virtual void Awake()
        {

        }

        protected virtual void Update()
        {
        }
    }
}