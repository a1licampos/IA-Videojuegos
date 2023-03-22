using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToPaint : MonoBehaviour
{
    public static WaitToPaint Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    AnotherClass a = new AnotherClass();
    //    a.Foo();
    //}

    public void Run(IEnumerator cor)
    {
        StartCoroutine(cor);
    }
}

public class AnotherClass
{
    public AnotherClass()
    {
        //WaitToPaint ab = new WaitToPaint();
    }
    public void Foo()
    {
        //CoRunner.Instance.Run(Do());
        WaitToPaint ab = new WaitToPaint();
        ab.Run(Do());
    }

    IEnumerator Do()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Done!");
    }
}
