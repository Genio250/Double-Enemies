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
        public static void NoskAI()
        {
            GameObject Nosk = GameObject.Find("Mimic Spider");
            PlayMakerFSM fsm = Nosk.LocateMyFSM("Mimic Spider");

            GameObject Nosk2 = GameObject.Find("Mimic Spider(EnemyDupe)");
            PlayMakerFSM fsm2 = Nosk2.LocateMyFSM("Mimic Spider");

            if (!GameManager.instance.sceneName.Contains("GG"))
            {
                fsm.AddCustomAction("Wake", () =>
                {
                    fsm2.SetState("Encountered");
                    Nosk.transform.position -= new Vector3(2, 0, 0);
                });
            }

            fsm.GetFirstActionOfType<IntCompare>("Roof Jump?").integer2.Value *= 2;
            fsm2.GetFirstActionOfType<IntCompare>("Roof Jump?").integer2.Value *= 2;
        }
    }
}