using CoFramework.Tasks;
using CoFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPanel : UIPanel
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
