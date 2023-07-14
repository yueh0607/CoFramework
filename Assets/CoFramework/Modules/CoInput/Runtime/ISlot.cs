using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CoFramework.Inputs
{
    public interface IKeyBoardSlot
    {
        public void OnSlotEnter();
        public void OnSlotStay();
        public void OnSlotExit();
    }

    public interface ITouchSlot
    {
        public void OnSlotEnter(Vector2 position);
        public void OnSlotStay(Vector2 position);
        public void OnSlotExit(Vector2 position);
    }
}
