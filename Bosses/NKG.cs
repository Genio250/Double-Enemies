using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using Modding;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoubleEnemies
{
    public static partial class Bosses
    {
        public static void NkgAI()
        {
            GameObject NKGDupe = GameObject.Find("Nightmare Grimm Boss(EnemyDupe)");
            PlayMakerFSM fsm = NKGDupe.LocateMyFSM("Control");
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => fsm.SendEvent("WAKE"));
        }
    }
}