namespace ARKitAndARCoreCommon
{
    using UnityEngine;

    public class Util
    {
        public static void Normalize(Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localEulerAngles = Vector3.zero;
            t.localScale = Vector3.one;
        }

        public static GameObject InstantiateTo(GameObject parent, GameObject go)
        {
            GameObject ins = GameObject.Instantiate(
                go,
                parent.transform.position,
                parent.transform.rotation
            );
            ins.transform.parent = parent.transform;
            Normalize(ins.transform);
            return ins;
        }

        public static T InstantiateTo<T>(GameObject parent, GameObject go) where T : Component
        {
            return InstantiateTo(parent, go).GetComponent<T>();
        }

        public static T FindCompomentInChildren<T>(Transform root) where T : class
        {
            for (int i = 0; i < root.childCount; ++i)
            {
                Transform t = root.GetChild(i);
                T compoment = t.GetComponent<T>();
                if (!compoment.Equals(null))
                {
                    return compoment;
                }

                T childCompoment = FindCompomentInChildren<T>(t);
                if (childCompoment != null)
                {
                    return childCompoment;
                }
            }

            return null;
        }
    }
}