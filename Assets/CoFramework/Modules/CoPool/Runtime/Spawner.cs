using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public interface ISpawner<T>
{


    public IEnumerator<T> OnCreate();

    public void OnDestroy(T item);

    public void OnGet(T item);

    public void OnSet(T item);


  
}


