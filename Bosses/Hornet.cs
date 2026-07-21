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
        public static GameObject HornetNeedle = null, HornetTink = null;
        public static int HornetFullHP = 0;
        public static void HornetAI()
        {
            GameObject Hornet = GameObject.Find("Hornet Boss 1(EnemyDupe)");
            if (Hornet == null)
            {
                Hornet = GameObject.Find("Hornet Boss 2(EnemyDupe)");
                HornetFullHP = Hornet.GetComponent<HealthManager>().hp;
                Modding.Logger.Log(HornetFullHP);
                Modding.Logger.Log(2 * HornetFullHP / 3);
                HornetPhaseFix();
            }
            PlayMakerFSM fsm = Hornet.LocateMyFSM("Control");

            GameObject NeedleOrig = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Needle");
            GameObject Needle = GameObject.Instantiate(NeedleOrig);
            Needle.name = "Needle(EnemyDupe)";
            PlayMakerFSM fsmn = Needle.LocateMyFSM("Control");

            GameObject TinkOrig = GameObject.Find("Needle Tink");
            GameObject Tink = GameObject.Instantiate(TinkOrig);
            Tink.name = "Needle Tink(EnemyDupe)";

            HornetNeedle = Needle;
            HornetTink = Tink;

            fsm.FsmVariables.GetFsmGameObject("Needle").Value = Needle;
            fsmn.RemoveAction("Init", 0);
            fsm.InsertCustomAction("Init", () =>
            {
                fsmn.FsmVariables.GetFsmGameObject("Parent").Value = Hornet;
            }, 0);

            ModHooks.HeroUpdateHook += HornetNeedleFollow;
        }

        private static void HornetNeedleFollow()
        {
            if(HornetNeedle != null)
            {
                HornetTink.transform.position = HornetNeedle.transform.position;
            }
            else ModHooks.HeroUpdateHook -= HornetNeedleFollow;
        }

        private static void HornetPhaseFix()
        {
            GameObject Hornet1 = GameObject.Find("Hornet Boss 2");
            GameObject Hornet2 = GameObject.Find("Hornet Boss 2(EnemyDupe)");
            PlayMakerFSM fsm1 = Hornet1.LocateMyFSM("Control");
            PlayMakerFSM fsm2 = Hornet2.LocateMyFSM("Control");

            fsm1.GetValidState("Barb?").Actions[0] = new CustomFsmAction()
            {
                method = () =>
                {
                    if(Hornet1.GetComponent<HealthManager>().hp > 2 * HornetFullHP / 3)
                    {
                        fsm1.SendEvent("FINISHED");
                    }
                }
            };

            fsm2.GetValidState("Barb?").Actions[0] = new CustomFsmAction()
            {
                method = () =>
                {
                    if (Hornet2.GetComponent<HealthManager>().hp > 2 * HornetFullHP / 3)
                    {
                        fsm2.SendEvent("FINISHED");
                    }
                }
            };
        }
    }
}