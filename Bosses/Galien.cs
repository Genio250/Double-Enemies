using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using Mono.Security.X509.Extensions;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Modding.Logger;

namespace DoubleEnemies
{
    public static partial class Bosses
    {
        public static void GalienScythe()
        {
            GameObject scythe = GameObject.Find("Galien Hammer");
            DoubleEnemies.Duplicate(scythe, false);

            GameObject scythe2 = GameObject.Find("Galien Hammer(EnemyDupe)");
            scythe2.transform.position += new Vector3(10, 0, 0);
            PlayMakerFSM scythe2FSM = scythe2.LocateMyFSM("Control");

            if(GameManager.instance.sceneName.Contains("GG"))
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => scythe2FSM.SendEvent("READY"));
        }

        public static void GalienMinis()
        {
            GameObject Galien = GameObject.Find("Ghost Warrior Galien");
            PlayMakerFSM fsm = Galien.LocateMyFSM("Summon Minis");

            GameObject Galien2 = GameObject.Find("Ghost Warrior Galien(EnemyDupe)");
            PlayMakerFSM fsm2 = Galien2.LocateMyFSM("Summon Minis");

            fsm.AddCustomAction("Summon Antic", () => fsm2.SendEvent("TOOK DAMAGE"));
            fsm.AddCustomAction("Summon Antic 2", () => fsm2.SendEvent("TOOK DAMAGE"));

            fsm2.AddCustomAction("Summon Antic", () => fsm.SendEvent("TOOK DAMAGE"));
            fsm2.AddCustomAction("Summon Antic 2", () => fsm.SendEvent("TOOK DAMAGE"));

            Satchel.CoroutineHelper.WaitForFramesBeforeInvoke(3, () => Log(fsm.FsmVariables.GetFsmInt("Summon HP1")));
        }
    }
}