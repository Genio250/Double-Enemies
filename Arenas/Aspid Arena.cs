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
    public static partial class Arenas
    {
        public static void Aspids()
        {
            if (Toggles.onlyboss) return;
            PlayMakerFSM fsm = GameObject.Find("Battle Scene").LocateMyFSM("Battle Control");
            fsm.GetFirstActionOfType<SetIntValue>("Wave 1").intValue = 6;
            fsm.GetFirstActionOfType<IntCompare>("Wave 1").integer2 = 4;

            GameObject Spawner1 = GameObject.Find("Spitter Summon v2");
            GameObject Aspid1 = GameObject.Instantiate(Spawner1);
            Aspid1.name = "Spitter Summon v2(EnemyDupe)";
            GameObject Spawner2 = GameObject.Find("Spitter Summon v2 (1)");
            GameObject Aspid2 = GameObject.Instantiate(Spawner2);
            Aspid2.name = "Spitter Summon v2 (1)(EnemyDupe)";

            fsm.AddCustomAction("Wave 2", () =>
            {
                Aspid1.LocateMyFSM("summon").SendEvent("SUMMON");
                Aspid2.LocateMyFSM("summon").SendEvent("SUMMON");
            });
        }
    }
}