using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchARController : MonoBehaviour {
    [SerializeField] private GameObject arkitController;
    [SerializeField] private GameObject arcoreController;

    private void Awake()
    {
#if UNITY_IPHONE
        GameObject instanciate = GameObject.Instantiate(
            arkitController,
            this.gameObject.transform.position,
            this.gameObject.transform.rotation
        );
        instanciate.transform.parent = this.transform;
        instanciate.transform.localPosition = Vector3.zero;
        instanciate.transform.localEulerAngles = Vector3.zero;
        instanciate.transform.localScale = Vector3.one;
#elif UNITY_ANDROID
        GameObject instanciate = GameObject.Instantiate(
            arcoreController,
            this.gameObject.transform.position,
            this.gameObject.transform.rotation
        );
        instanciate.transform.parent = this.transform;
        instanciate.transform.localPosition = Vector3.zero;
        instanciate.transform.localEulerAngles = Vector3.zero;
        instanciate.transform.localScale = Vector3.one;
#endif
    }
}
