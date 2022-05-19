using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public delegate void OnResetDemo();
    public OnResetDemo onResetDemo;
}
