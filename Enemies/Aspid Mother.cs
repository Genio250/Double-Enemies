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
    public static partial class Enemies
    {
        public static void HatcherAI(GameObject enemy)
        {
            if (!Toggles.onlyboss) return;

            PlayMakerFSM fsm = enemy.LocateMyFSM("Hatcher");
            fsm.CopyState("Fire", "Fire 2");
            fsm.ChangeTransition("Fire", "WAIT", "Fire 2");
            fsm.RemoveAction("Fire 2", 9);

            GameObject copy = GameObject.Find(enemy.name + "(EnemyDupe)");
            PlayMakerFSM fsm2 = copy.LocateMyFSM("Hatcher");
            fsm2.CopyState("Fire", "Fire 2");
            fsm2.ChangeTransition("Fire", "WAIT", "Fire 2");
            fsm2.RemoveAction("Fire 2", 9);
        }
    }
}