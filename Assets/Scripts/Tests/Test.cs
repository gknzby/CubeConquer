using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeConquer.Managers;

public class Test : MonoBehaviour
{
    public GameObject awakeTestObject;
    private void Awake()
    {
        GameObject.Instantiate(awakeTestObject, this.transform);
        Debug.Log("This is AwakeTest base Awake function method.");
    }
    private void OnEnable()
    {
        Debug.Log("This is AwakeTest base OnEnable functin method");
    }
    // Start is called before the first frame update
    void Start()
    {
        IGameManager igm = ManagerProvider.GetManager<IGameManager>();
        igm.TestFunc("This is IGameManager test string.");

        Debug.Log("This is AwakeTest base Start function method.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
