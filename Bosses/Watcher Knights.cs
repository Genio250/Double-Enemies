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
    public static class Watchers
    {
        public static void WatchersAI()
        {
            GameObject Battle = GameObject.Find("Battle Control");
            PlayMakerFSM fsm = Battle.LocateMyFSM("Battle Control");

            GameObject Watcher1 = GameObject.Find("Black Knight 1(EnemyDupe)");
            PlayMakerFSM fsmW1 = Watcher1.LocateMyFSM("Black Knight");

            fsm.InsertCustomAction("Knight 1", () =>
            {
                Watcher1.transform.position = GameObject.Find("Black Knight 1").transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
                fsmW1.SendEvent("WAKE");
            }, 0);

            GameObject Watcher2 = GameObject.Find("Black Knight 2(EnemyDupe)");
            PlayMakerFSM fsmW2 = Watcher2.LocateMyFSM("Black Knight");

            fsm.InsertCustomAction("Knight 2", () =>
            {
                Watcher2.transform.position = GameObject.Find("Black Knight 2").transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
                fsmW2.SendEvent("WAKE");
            }, 0);

            if (GameManager.instance.sceneName == "GG_Watcher_Knights" || !PlayerData.instance.GetBool("watcherChandelier"))
            {
                GameObject Watcher3 = GameObject.Find("Black Knight 3(EnemyDupe)");
                PlayMakerFSM fsmW3 = Watcher3.LocateMyFSM("Black Knight");

                fsm.InsertCustomAction("Knight 3", () =>
                {
                    if (GameManager.instance.sceneName == "GG_Watcher_Knights" || !PlayerData.instance.GetBool("watcherChandelier"))
                    {
                        Watcher3.transform.position = GameObject.Find("Black Knight 3").transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
                        fsmW3.SendEvent("WAKE");
                    }
                }, 0);
            }

            GameObject Watcher4 = GameObject.Find("Black Knight 4(EnemyDupe)");
            PlayMakerFSM fsmW4 = Watcher4.LocateMyFSM("Black Knight");

            fsm.InsertCustomAction("Knight 4", () =>
            {
                Watcher4.transform.position = GameObject.Find("Black Knight 4").transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
                fsmW4.SendEvent("WAKE");
            }, 0);

            GameObject Watcher5 = GameObject.Find("Black Knight 5(EnemyDupe)");
            PlayMakerFSM fsmW5 = Watcher5.LocateMyFSM("Black Knight");

            fsm.InsertCustomAction("Knight 5", () =>
            {
                Watcher5.transform.position = GameObject.Find("Black Knight 5").transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
                fsmW5.SendEvent("WAKE");
            }, 0);

            GameObject Watcher6 = GameObject.Find("Black Knight 6(EnemyDupe)");
            PlayMakerFSM fsmW6 = Watcher6.LocateMyFSM("Black Knight");

            fsm.InsertCustomAction("Knight 6", () =>
            {
                Watcher6.transform.position = GameObject.Find("Black Knight 6").transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
                fsmW6.SendEvent("WAKE");
            }, 0);


            fsm.FsmVariables.GetFsmInt("Battle Enemies").Value *= 2;

            fsm.GetValidState("Knight 1").GetFirstActionOfType<IntCompare>().integer2 = new FsmInt { Value = 10 };
            fsm.GetValidState("Knight 2").GetFirstActionOfType<IntCompare>().integer2 = new FsmInt { Value = 10 };
            fsm.GetValidState("Knight 3").GetFirstActionOfType<IntCompare>().integer2 = new FsmInt { Value = 8 };
            fsm.GetValidState("Skip").GetFirstActionOfType<IntAdd>().add = new FsmInt { Value = -2 };
            fsm.GetValidState("Knight 4").GetFirstActionOfType<IntCompare>().integer2 = new FsmInt { Value = 6 };
            fsm.GetValidState("Knight 5").GetFirstActionOfType<IntCompare>().integer2 = new FsmInt { Value = 4 };
        }
    }
}