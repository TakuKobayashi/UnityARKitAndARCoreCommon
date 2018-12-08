using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.iOS.Utils;


namespace UnityEngine.XR.iOS
{
    public class ARPointCloud 
    {
        IntPtr m_Ptr;

        internal IntPtr nativePtr { get { return m_Ptr; } }

        public int Count
        {
            get { return GetCount(); }
        }

        public Vector3[] Points 
        {
            get { return GetPoints(); }
        }

        public UInt64[] Identifiers
        {
            get { return GetIdentifiers();  }
        }

        internal static ARPointCloud FromPtr(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            return new ARPointCloud(ptr);
        }

        internal ARPointCloud(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentException("ptr may not be IntPtr.Zero");

            m_Ptr = ptr;
        }
#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        static extern int pointCloud_GetCount(IntPtr ptr);

        [DllImport("__Internal")]
        static extern IntPtr pointCloud_GetPointsPtr(IntPtr ptr);

        [DllImport("__Internal")]
        static extern IntPtr pointCloud_GetIdentifiersPtr(IntPtr ptr);
        
        int GetCount()
        {
            return pointCloud_GetCount(m_Ptr);
        }
    
        Vector3[] GetPoints()
        {
            IntPtr pointsPtr = pointCloud_GetPointsPtr (m_Ptr);
            int pointCount = Count;
            if (pointCount <= 0 || pointsPtr == IntPtr.Zero) 
            {
                return null;
            }
            else 
            {
                // Load the results into a managed array.
                var floatCount = pointCount * 4;
                float [] resultVertices = new float[floatCount];
                Marshal.Copy(pointsPtr, resultVertices, 0, floatCount);

                Vector3[] verts = new Vector3[pointCount];

                for (int count = 0; count < pointCount; count++)
                {
                    //convert to Unity coords system
                    verts[count].x = resultVertices[count * 4];
                    verts[count].y = resultVertices[count * 4 + 1];
                    verts[count].z = -resultVertices[count * 4 + 2];
                }

                return verts;
            }
        }

        UInt64[] GetIdentifiers()
        {
            IntPtr identifiersPtr = pointCloud_GetIdentifiersPtr(m_Ptr);
            int identifiersCount = Count;
            if (identifiersCount <= 0 || identifiersPtr == IntPtr.Zero) 
            {
                return null;
            }
            else 
            {
                // Load the results into a managed array.
                Int64 [] copyIdentifiers = new Int64[identifiersCount];
                Marshal.Copy(identifiersPtr, copyIdentifiers, 0, identifiersCount);

                UInt64 [] resultIdentifiers = new UInt64[identifiersCount];
                int index = 0;
                foreach (Int64 identifier in copyIdentifiers)
                {
                    //convert to UInt64
                    resultIdentifiers[index++] = (UInt64) identifier;
                }

                return resultIdentifiers;
            }
        }
#else
        Vector3[] editorPointData;
        UInt64[] editorPointIds;
        
        internal ARPointCloud(serializablePointCloud spc)
        {
            if (spc.pointCloudData != null) 
            {
                int numVectors = spc.pointCloudData.Length / (3 * sizeof(float));
                editorPointData = new Vector3[numVectors];
                for (int i = 0; i < numVectors; i++) 
                {
                    int bufferStart = i * 3;
                    editorPointData[i].x = BitConverter.ToSingle (spc.pointCloudData, (bufferStart) * sizeof(float));
                    editorPointData[i].y = BitConverter.ToSingle (spc.pointCloudData, (bufferStart+1) * sizeof(float));
                    editorPointData[i].z = BitConverter.ToSingle (spc.pointCloudData, (bufferStart+2) * sizeof(float));
                }
            } 
            else 
            {
                editorPointData = null;
            }

            if (spc.pointCloudIds != null)
            {
                int numIds = spc.pointCloudIds.Length;
                editorPointIds = new ulong[numIds];
                for (int i = 0; i < numIds; i++)
                {
                    editorPointIds[i] = BitConverter.ToUInt64(spc.pointCloudIds, i * sizeof(UInt64));
                }
            }
            else
            {
                editorPointIds = null;
            }
        }

        int GetCount()
        {
            if (editorPointData == null)
            {
                return 0;
            }
            else
            {
                return editorPointData.Length;
            }
        }

        Vector3[] GetPoints()
        {
            return editorPointData;
        }

        UInt64[] GetIdentifiers()
        {
            return editorPointIds;
        }
#endif


    }
}