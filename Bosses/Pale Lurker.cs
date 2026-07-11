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
    public static class PaleLurker
    {
        public static void LurkerAI()
        {
            bool digger = true;
            GameObject Lurker = GameObject.Find("Pale Lurker");
            PlayMakerFSM fsm = Lurker.LocateMyFSM("Lurker Control");
            GameObject Lurker2 = GameObject.Find("Pale Lurker(EnemyDupe)");
            PlayMakerFSM fsm2 = Lurker2.LocateMyFSM("Lurker Control");

            fsm.InsertCustomAction("Get High", () =>
            {
                fsm2.SendEvent("START");
            }, 0);

            fsm.InsertCustomAction("Alert Anim", () =>
            {
                fsm2.SendEvent("TOOK DAMAGE");
            }, 0);

            fsm.InsertCustomAction("Dig Out", () =>
            {
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Lurker2.transform.position = Lurker.transform.position);
            }, 0);

            fsm.InsertCustomAction("Dig 2", () =>
            {
                if (digger)
                {
                    fsm2.SetState("Hop Antic");
                    fsm2.SendEvent("HERO CAST SPELL");
                    GameObject Ball2 = GameObject.Find("Pale Lurker(EnemyDupe)/Slash Ball");
                    if (Ball2 != null)
                    {
                        PlayMakerFSM Ballfsm2 = Ball2.LocateMyFSM("Control");
                        Ballfsm2.SendEvent("OUT");
                    }
                    digger = false;
                }
                else digger = true;
            }, 0);

            fsm2.InsertCustomAction("Dig 2", () =>
            {
                if (digger)
                {
                    fsm.SetState("Hop Antic");
                    fsm.SendEvent("HERO CAST SPELL");
                    GameObject Ball = GameObject.Find("Pale Lurker/Slash Ball");
                    if (Ball != null)
                    {
                        PlayMakerFSM Ballfsm = Ball.LocateMyFSM("Control");
                        Ballfsm.SendEvent("OUT");
                    }
                    digger = false;
                }
                else digger = true;
            }, 0);

        }
    }
}