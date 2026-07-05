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
    public static class Nosket
    {
        public static void NosketAI()
        {
            string name = "Hornet Nosk", control = "Hornet Nosk";
            GameObject Boss = GameObject.Find(name);
            PlayMakerFSM fsm = Boss.LocateMyFSM(control);
            GameObject BossDupe = GameObject.Find(name + "(EnemyDupe)");
            PlayMakerFSM fsmd = BossDupe.LocateMyFSM(control);
            fsm.InsertCustomAction("Set Pos", () =>
            {
                fsmd.SendEvent("START");
            }, 0);
            fsm.FsmVariables.FindFsmInt("Half HP").Value *= 2;
            fsmd.FsmVariables.FindFsmInt("Half HP").Value *= 2;
        }
    }
}