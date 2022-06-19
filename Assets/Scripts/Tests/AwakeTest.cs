using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTest : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("This is AwakeTest instantiate Awake function method.");
    }

    private void OnEnable()
    {
        Debug.Log("This is AwakeTest instantiate OnEnable functin method");
    }

    private void Start()
    {
        Debug.Log("This is AwakeTest instantiate Start functin method");
    }
}
