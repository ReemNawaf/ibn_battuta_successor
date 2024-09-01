using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARSubsystems;

namespace myNameSpace
{
    [CreateAssetMenu(menuName = "Image Marker Library", fileName = "Image Marker Library")]
    public class ImageMarkerLibrary : ScriptableObject
    {
        public List<ImageMarkerData> ImageMarkerDataList;
    }
}

[System.Serializable]
public class ImageMarkerData
{
    public string Name;
    //public XRReferenceImage referenceImage;
    public GameObject PrefabToInstantiate;

}
