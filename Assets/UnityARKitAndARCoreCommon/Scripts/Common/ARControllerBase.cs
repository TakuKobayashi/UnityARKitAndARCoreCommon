namespace ARKitAndARCoreCommon
{
    using UnityEngine;

    public abstract class ARControllerBase : MonoBehaviour
    {
        [SerializeField] protected GameObject ARMainCameraPrefab;
        [SerializeField] protected GameObject TrackedPlanePrefab;
        [SerializeField] private GameObject PointCloudPrefab;

        protected GameObject pointCloudObj;
        protected Camera mainCamera;

        protected virtual void Awake()
        {
            Camera defaultCamera = Camera.main;
            if (defaultCamera != null)
            {
                defaultCamera.enabled = false;
                defaultCamera.gameObject.SetActive(false);
            }
            GameObject mainCameraManager = Util.InstantiateTo(this.gameObject, ARMainCameraPrefab);
            mainCamera = Util.FindCompomentInChildren<Camera>(mainCameraManager.transform);
            if (mainCamera == null)
            {
                mainCamera = mainCameraManager.GetComponent<Camera>();
            }
        }

        protected virtual void Update()
        {
        }
    }
}