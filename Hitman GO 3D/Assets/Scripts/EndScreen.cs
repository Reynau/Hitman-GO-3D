using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EndScreen : MonoBehaviour {
    public PostProcessingProfile defaultProfile;
    public PostProcessingProfile blurProfile;
    public PostProcessingBehaviour cameraPostProcess;

    public void EnableCameraBlur(bool enable)
    {
        if (defaultProfile != null && blurProfile != null && cameraPostProcess != null)
        {
            cameraPostProcess.profile = (enable) ? blurProfile : defaultProfile;
        }
    }
}
