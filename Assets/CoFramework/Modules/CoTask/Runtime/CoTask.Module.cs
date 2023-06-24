using CoFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskModule : IModule
{
    private MonoBehaviour mono;
    public MonoBehaviour Mono => mono;
    public void OnCreate(CreateParameters parameters)
    {
        GameObject driver = new GameObject("[Module.CoTask.Driver]");
        GameObject.DontDestroyOnLoad(driver);
        mono = driver.AddComponent<TaskDriver>();
    }

    public event Action UpdateTask=null;

    public void OnDestroy()
    {
        UpdateTask=null;
        GameObject.Destroy(mono.gameObject);
    }

    public void OnUpdate()
    {
        UpdateTask?.Invoke();
    }
}
