using UnityEngine;

namespace UnityARKitAndARCoreCommon
{
    public class SwitchARController : MonoBehaviour
    {
        [SerializeField] private GameObject arkitController;
        [SerializeField] private GameObject arcoreController;

        private void Awake()
        {
#if UNITY_IPHONE
            Util.InstantiateTo(this.gameObject, arkitController);
#elif UNITY_ANDROID
            Util.InstantiateTo(this.gameObject, arcoreController);
#endif
        }
    }
}