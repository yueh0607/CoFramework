using CoFramework;
using CoFramework.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    IEnumerator WaitTest()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("1");
    }

    async CoTask AsyncTest()
    {
        await WaitTest();
        Debug.Log("2");
    }

    void Start()
    {
        Framework.CreateModule<TaskModule>(null);
        AsyncTest().WithToken(out var token);
        token.Yield();
        token.Continue();
        token.Cancel();
    }


    // Update is called once per frame
    void Update()
    {
        Framework.DriveUpdate(Time.deltaTime);
    }
}
