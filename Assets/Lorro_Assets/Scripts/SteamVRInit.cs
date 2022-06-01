using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVRInit : MonoBehaviour
{
    //Modified version of file: https://gist.github.com/hon454/83b54708ee066e44af4d0b68a6862f02
    private void Start()
    {
        SteamVR_TrackedObject.EIndex trackerIndex = 0;
        SteamVR_TrackedObject.EIndex fallbackControllerIndex = 0;
        bool trackerFound = false;

        ETrackedPropertyError error = new ETrackedPropertyError();
        for (uint i = 0; i < 16; i++)
        {
            var type = new System.Text.StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_ControllerType_String, type, 64, ref error);

            EDeviceActivityLevel active = OpenVR.System.GetTrackedDeviceActivityLevel(i);

            Debug.Log("Found " + type.ToString() + " in state " + active.ToString());
            if (type.ToString().Contains("tracker") && active.ToString().Contains("UserInteraction"))
            {
                trackerIndex = (SteamVR_TrackedObject.EIndex)i;
                Debug.Log("Found " + type.ToString() + " in state " + active.ToString() + " on index: " + i);
                trackerFound = true;
            }else if (type.ToString().Contains("controller"))
            {
                fallbackControllerIndex = (SteamVR_TrackedObject.EIndex)i;
                Debug.Log("Set controller fallback to " + type.ToString() + " on index: " + i);
            }
        }

        if (trackerFound)
        {
            GetComponent<SteamVR_TrackedObject>().index = trackerIndex;
        }
        else
        {
            GetComponent<SteamVR_TrackedObject>().index = fallbackControllerIndex;
        }
    }
}
