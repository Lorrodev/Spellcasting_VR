using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVRInit : MonoBehaviour
{
    [SerializeField]
    private float refreshInterval = 2f;

    private float lastRefresh = 0f;

    //Modified version of file: https://gist.github.com/hon454/57ba590a4f3f33d6172f9c1d7f60ad32
    private void Start()
    {
        GetController();
    }

    private void Update()
    {
        lastRefresh += Time.deltaTime;

        if (lastRefresh >= refreshInterval)
        {
            lastRefresh = 0f;
            GetController();
        }
    }

    private void GetController()
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

            if (type.ToString().Contains("tracker") && active.ToString().Contains("UserInteraction"))
            {
                trackerIndex = (SteamVR_TrackedObject.EIndex)i;
                trackerFound = true;
            }
            else if ((type.ToString().Contains("controller") || type.ToString().Contains("knuckles")) && active.ToString().Contains("UserInteraction"))
            {
                fallbackControllerIndex = (SteamVR_TrackedObject.EIndex)i;
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
