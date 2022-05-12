using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVRInit : MonoBehaviour
{
    //https://gist.github.com/hon454/83b54708ee066e44af4d0b68a6862f02
    private void Start()
    {
        ETrackedPropertyError error = new ETrackedPropertyError();
        for (uint i = 0; i < 16; i++)
        {
            var result = new System.Text.StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);

            // if you want to get first tracker's index.
            if (result.ToString().Contains("tracker"))
            {
                GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                Debug.Log("Found Device on index "+i);
                break;
            }
        }
    }
}
