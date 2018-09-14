namespace ARKitAndARCoreCommon
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.iOS;

    public class ARKitController : ARControllerBase
    {
        [SerializeField] private GameObject fieldObjectAnchorRoot;
        [SerializeField] private GameObject remoteConnectionPrefab;
        [SerializeField] private GameObject AppearTouchPrefab;

        private UnityARAnchorManager unityARAnchorManager;

        protected override void Awake()
        {
            base.Awake();
            Util.InstantiateTo(this.gameObject, remoteConnectionPrefab);
            unityARAnchorManager = new UnityARAnchorManager();
            UnityARUtility.InitializePlanePrefab(TrackedPlanePrefab);
        }

        protected override void Update()
        {
            base.Update();

            LinkedList<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            Vector3 screenPosition = mainCamera.ScreenToViewportPoint(touch.position);
            ARPoint point = new ARPoint
            {
                x = screenPosition.x,
                y = screenPosition.y
            };

            // prioritize reults types
            ARHitTestResultType[] resultTypes = {
                ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                // if you want to use infinite planes use this:
                //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane,
                ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane,
                ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
            };

            for (int i = 0; i < resultTypes.Length; ++i)
            {
                List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes[i]);
                if (hitResults.Count > 0)
                {
                    Matrix4x4 matrix = hitResults[0].worldTransform;
                    var touchedObject = Instantiate(AppearTouchPrefab, UnityARMatrixOps.GetPosition(matrix), UnityARMatrixOps.GetRotation(matrix));
                    touchedObject.transform.LookAt(mainCamera.transform);
                    touchedObject.transform.rotation = Quaternion.Euler(0.0f, touchedObject.transform.rotation.eulerAngles.y, touchedObject.transform.rotation.z);
                    touchedObject.transform.parent = fieldObjectAnchorRoot.transform;
                    break;
               }
           }
       }
   }
}