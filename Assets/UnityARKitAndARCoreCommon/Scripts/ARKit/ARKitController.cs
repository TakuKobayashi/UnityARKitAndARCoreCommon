using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

namespace UnityARKitAndARCoreCommon
{
    public class ARKitController : ARControllerBase
    {
        [SerializeField] private GameObject fieldObjectAnchorRoot;
        [SerializeField] private GameObject remoteConnectionPrefab;

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

            List<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                // かぶさってるので処理キャンセル（タップver）
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
                ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
            };

            for (int i = 0; i < resultTypes.Length; ++i)
            {
                List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes[i]);
                if (hitResults.Count > 0)
                {
                   Vector3 touchPosition = UnityARMatrixOps.GetPosition(hitResults[0].worldTransform);
                   break;
               }
           }
       }
   }
}