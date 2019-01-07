namespace ARKitAndARCoreCommon
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_IOS
    using UnityEngine.XR.iOS;
#endif

    public class ARDirectionalLight : MonoBehaviour
    {
        protected virtual void Awake()
        {
#if UNITY_IOS
            this.gameObject.AddComponent<UnityARAmbient>();
#endif
        }
    }
}