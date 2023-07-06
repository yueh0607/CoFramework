using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoFramework.UI;
using CoFramework.Tasks;

public class APanel : UIPanel
{
    protected override async CoTask OnClose()
    {
        await CoTask.CompletedTask;
    }

    protected override async CoTask OnCreate()
    {
        await CoTask.CompletedTask;
    }

    protected override async CoTask OnDestroy()
    {
        await CoTask.CompletedTask;
    }

    protected override async CoTask OnOpen()
    {
        await CoTask.CompletedTask;
    }

    protected override void Update()
    {
     
    }
}
