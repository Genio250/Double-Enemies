using FiveKnights.Dryya;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DoubleEnemies
{
    public static class Dryya
    {
        public static void DryyaAI()
        {
            GameObject Dryya = GameObject.Find("Dryya2(Clone)(EnemyDupe)");
            PlayMakerFSM fsm = Dryya.LocateMyFSM("Control");
            PlayMakerFSM ori = GameObject.Find("Dryya2(Clone)").LocateMyFSM("Control");

            fsm.Fsm.GetFsmInt("Phase 2 HP").Value *= 2;
            fsm.Fsm.GetFsmInt("Phase 3 HP").Value *= 2;
            ori.Fsm.GetFsmInt("Phase 2 HP").Value *= 2;
            ori.Fsm.GetFsmInt("Phase 3 HP").Value *= 2;

            Modding.Logger.Log(fsm.FsmVariables.FindFsmInt("Phase 3 HP").Value);

            fsm.Fsm.GetState("Phase Check").Actions[0] = new CustomFsmAction()
            {
                method = () => fsm.Fsm.GetFsmInt("HP").Value = Dryya.GetComponent<HealthManager>().hp
            };

            string version = ModHooks.GetMod("Pale Court").GetVersion();
            Modding.Logger.Log(version);
            if (version[4] == '1')
            {
                fsm.Fsm.GetState("Knockout Transition").Actions[0] = new CustomFsmAction()
                {
                    method = () => fsm.Fsm.GetFsmInt("HP").Value = Dryya.GetComponent<HealthManager>().hp
                };


                GameObject Dryya1 = GameObject.Find("Dryya2(Clone)");
                var setup1 = Dryya1.GetComponent<DryyaSetup>();

                FieldInfo stagger1 = typeof(DryyaSetup).GetField
                    (
                        "MaxStaggerHits",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                stagger1.SetValue(setup1, 24);

                FieldInfo final1 = typeof(DryyaSetup).GetField
                    (
                        "Phase3HP",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                final1.SetValue(setup1, fsm.FsmVariables.FindFsmInt("Phase 3 HP").Value);

                GameObject Dryya2 = GameObject.Find("Dryya2(Clone)(EnemyDupe)");
                var setup2 = Dryya2.GetComponent<DryyaSetup>();
                FieldInfo stagger2 = typeof(DryyaSetup).GetField
                    (
                        "MaxStaggerHits",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                stagger2.SetValue(setup2, 24);

                FieldInfo final2 = typeof(DryyaSetup).GetField
                    (
                        "Phase3HP",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                final2.SetValue(setup2, fsm.FsmVariables.FindFsmInt("Phase 3 HP").Value);
            }
        }
    }
}