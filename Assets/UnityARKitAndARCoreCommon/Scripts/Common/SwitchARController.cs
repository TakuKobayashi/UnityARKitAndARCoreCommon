namespace ARKitAndARCoreCommon
{
    using UnityEngine;

    public class SwitchARController : MonoBehaviour
    {
        [SerializeField] private GameObject arkitController;
        [SerializeField] private GameObject arcoreController;

        private void Awake()
        {
#if UNITY_IOS
            Util.InstantiateTo(this.gameObject, arkitController);
#elif UNITY_ANDROID
            Util.InstantiateTo(this.gameObject, arcoreController);
#endif
        }
    }
}