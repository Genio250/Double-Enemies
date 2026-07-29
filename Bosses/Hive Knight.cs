using HutongGames.PlayMaker.Actions;
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
        public static void HiveKnightAI()
        {
            string name = "Hive Knight", control = "Control";
            GameObject Boss = GameObject.Find(name);
            PlayMakerFSM fsm = Boss.LocateMyFSM(control);
            GameObject BossDupe = GameObject.Find(name + "(EnemyDupe)");
            PlayMakerFSM fsmd = BossDupe.LocateMyFSM(control);

            
            fsm.FsmVariables.FindFsmInt("P2 HP").Value *= 2;
            fsm.FsmVariables.FindFsmInt("P3 HP").Value *= 2;
            fsmd.FsmVariables.FindFsmInt("P2 HP").Value *= 2;
            fsmd.FsmVariables.FindFsmInt("P3 HP").Value *= 2;

            GameObject Globs = GameObject.Find("Globs");
            GameObject DupeGlobs = GameObject.Instantiate(Globs, Globs.transform.parent);
            PlayMakerFSM DupeGlobsFSM = DupeGlobs.LocateMyFSM("Control");
            DupeGlobs.name = "Globs(EnemyDupe)";


            GameObject Bee1 = GameObject.Find("Bee Dropper");
            GameObject DupeBee1 = GameObject.Instantiate(Bee1, Bee1.transform.parent);
            PlayMakerFSM DupeBee1FSM = DupeBee1.LocateMyFSM("Control");
            DupeBee1.name = "Bee Dropper(Backup)";
            DupeBee1.transform.parent = null;

            GameObject Bee2 = GameObject.Find("Bee Dropper (1)");
            GameObject DupeBee2 = GameObject.Instantiate(Bee2, Bee2.transform.parent);
            PlayMakerFSM DupeBee2FSM = DupeBee2.LocateMyFSM("Control");
            DupeBee2.name = "Bee Dropper (1)(Backup)";
            DupeBee2.transform.parent = null;

            GameObject Bee3 = GameObject.Find("Bee Dropper (2)");
            GameObject DupeBee3 = GameObject.Instantiate(Bee3, Bee3.transform.parent);
            PlayMakerFSM DupeBee3FSM = DupeBee3.LocateMyFSM("Control");
            DupeBee3.name = "Bee Dropper (2)(Backup)";
            DupeBee3.transform.parent = null;

            GameObject Bee4 = GameObject.Find("Bee Dropper (3)");
            GameObject DupeBee4 = GameObject.Instantiate(Bee4, Bee4.transform.parent);
            PlayMakerFSM DupeBee4FSM = DupeBee4.LocateMyFSM("Control");
            DupeBee4.name = "Bee Dropper (3)(Backup)";
            DupeBee4.transform.parent = null;

            GameObject Bee5 = GameObject.Find("Bee Dropper (4)");
            GameObject DupeBee5 = GameObject.Instantiate(Bee5, Bee5.transform.parent);
            PlayMakerFSM DupeBee5FSM = DupeBee5.LocateMyFSM("Control");
            DupeBee5.name = "Bee Dropper (4)(Backup)";
            DupeBee5.transform.parent = null;

            GameObject Bee6 = GameObject.Find("Bee Dropper (5)");
            GameObject DupeBee6 = GameObject.Instantiate(Bee6, Bee6.transform.parent);
            PlayMakerFSM DupeBee6FSM = DupeBee6.LocateMyFSM("Control");
            DupeBee6.name = "Bee Dropper (5)(Backup)";
            DupeBee6.transform.parent = null;

            GameObject Bee7 = GameObject.Find("Bee Dropper (6)");
            GameObject DupeBee7 = GameObject.Instantiate(Bee7, Bee7.transform.parent);
            PlayMakerFSM DupeBee7FSM = DupeBee7.LocateMyFSM("Control");
            DupeBee7.name = "Bee Dropper (6)(Backup)";
            DupeBee7.transform.parent = null;

            GameObject DupeBee1Dupe = GameObject.Instantiate(Bee1, Bee1.transform.parent);
            PlayMakerFSM DupeBee1FSMDupe = DupeBee1Dupe.LocateMyFSM("Control");
            DupeBee1Dupe.name = "Bee Dropper(Backup)(EnemyDupe)";
            DupeBee1Dupe.transform.parent = null;

            GameObject DupeBee2Dupe = GameObject.Instantiate(Bee2, Bee2.transform.parent);
            PlayMakerFSM DupeBee2FSMDupe = DupeBee2Dupe.LocateMyFSM("Control");
            DupeBee2Dupe.name = "Bee Dropper (1)(Backup)(EnemyDupe)";
            DupeBee2Dupe.transform.parent = null;

            GameObject DupeBee3Dupe = GameObject.Instantiate(Bee3, Bee3.transform.parent);
            PlayMakerFSM DupeBee3FSMDupe = DupeBee3Dupe.LocateMyFSM("Control");
            DupeBee3Dupe.name = "Bee Dropper (2)(Backup)(EnemyDupe)";
            DupeBee3Dupe.transform.parent = null;

            GameObject DupeBee4Dupe = GameObject.Instantiate(Bee4, Bee4.transform.parent);
            PlayMakerFSM DupeBee4FSMDupe = DupeBee4Dupe.LocateMyFSM("Control");
            DupeBee4Dupe.name = "Bee Dropper (3)(Backup)(EnemyDupe)";
            DupeBee4Dupe.transform.parent = null;

            GameObject DupeBee5Dupe = GameObject.Instantiate(Bee5, Bee5.transform.parent);
            PlayMakerFSM DupeBee5FSMDupe = DupeBee5Dupe.LocateMyFSM("Control");
            DupeBee5Dupe.name = "Bee Dropper (4)(Backup)(EnemyDupe)";
            DupeBee5Dupe.transform.parent = null;

            GameObject DupeBee6Dupe = GameObject.Instantiate(Bee6, Bee6.transform.parent);
            PlayMakerFSM DupeBee6FSMDupe = DupeBee6Dupe.LocateMyFSM("Control");
            DupeBee6Dupe.name = "Bee Dropper (5)(Backup)(EnemyDupe)";
            DupeBee6Dupe.transform.parent = null;

            GameObject DupeBee7Dupe = GameObject.Instantiate(Bee7, Bee7.transform.parent);
            PlayMakerFSM DupeBee7FSMDupe = DupeBee7Dupe.LocateMyFSM("Control");
            DupeBee7Dupe.name = "Bee Dropper (6)(Backup)(EnemyDupe)";
            DupeBee7Dupe.transform.parent = null;

            GameObject Bee1Dupe = GameObject.Instantiate(Bee1, Bee1.transform.parent);
            PlayMakerFSM Bee1FSMDupe = Bee1Dupe.LocateMyFSM("Control");
            Bee1Dupe.name = "Bee Dropper(Backup)(EnemyDupe)";
            Bee1Dupe.transform.parent = null;

            GameObject Bee2Dupe = GameObject.Instantiate(Bee2, Bee2.transform.parent);
            PlayMakerFSM Bee2FSMDupe = Bee2Dupe.LocateMyFSM("Control");
            Bee2Dupe.name = "Bee Dropper (1)(EnemyDupe)";
            Bee2Dupe.transform.parent = null;

            GameObject Bee3Dupe = GameObject.Instantiate(Bee3, Bee3.transform.parent);
            PlayMakerFSM Bee3FSMDupe = Bee3Dupe.LocateMyFSM("Control");
            Bee3Dupe.name = "Bee Dropper (2)(EnemyDupe)";
            Bee3Dupe.transform.parent = null;

            GameObject Bee4Dupe = GameObject.Instantiate(Bee4, Bee4.transform.parent);
            PlayMakerFSM Bee4FSMDupe = Bee4Dupe.LocateMyFSM("Control");
            Bee4Dupe.name = "Bee Dropper (3)(EnemyDupe)";
            Bee4Dupe.transform.parent = null;

            GameObject Bee5Dupe = GameObject.Instantiate(Bee5, Bee5.transform.parent);
            PlayMakerFSM Bee5FSMDupe = Bee5Dupe.LocateMyFSM("Control");
            Bee5Dupe.name = "Bee Dropper (4)(EnemyDupe)";
            Bee5Dupe.transform.parent = null;

            GameObject Bee6Dupe = GameObject.Instantiate(Bee6, Bee6.transform.parent);
            PlayMakerFSM Bee6FSMDupe = Bee6Dupe.LocateMyFSM("Control");
            Bee6Dupe.name = "Bee Dropper (5)(EnemyDupe)";
            Bee6Dupe.transform.parent = null;

            GameObject Bee7Dupe = GameObject.Instantiate(Bee7, Bee7.transform.parent);
            PlayMakerFSM Bee7FSMDupe = Bee7Dupe.LocateMyFSM("Control");
            Bee7Dupe.name = "Bee Dropper (6)(EnemyDupe)";
            Bee7Dupe.transform.parent = null;

            PlayMakerFSM Bee1FSM = Bee1.LocateMyFSM("Control");
            PlayMakerFSM Bee2FSM = Bee2.LocateMyFSM("Control");
            PlayMakerFSM Bee3FSM = Bee3.LocateMyFSM("Control");
            PlayMakerFSM Bee4FSM = Bee4.LocateMyFSM("Control");
            PlayMakerFSM Bee5FSM = Bee5.LocateMyFSM("Control");
            PlayMakerFSM Bee6FSM = Bee6.LocateMyFSM("Control");
            PlayMakerFSM Bee7FSM = Bee7.LocateMyFSM("Control");


            fsmd.Fsm.GetState("Glob Strike").Actions[3] = new CustomFsmAction()
            {
                method = () => {
                    DupeGlobsFSM.SendEvent("FIRE");
                }
            };

            fsmd.Fsm.GetState("Roar Recover").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    DupeBee1FSM.SendEvent("SWARM");
                    DupeBee2FSM.SendEvent("SWARM");
                    DupeBee3FSM.SendEvent("SWARM");
                    DupeBee4FSM.SendEvent("SWARM");
                    DupeBee5FSM.SendEvent("SWARM");
                    DupeBee6FSM.SendEvent("SWARM");
                    DupeBee7FSM.SendEvent("SWARM");

                    if (!Toggles.onlyboss)
                    {
                        DupeBee1FSMDupe.SendEvent("SWARM");
                        DupeBee2FSMDupe.SendEvent("SWARM");
                        DupeBee3FSMDupe.SendEvent("SWARM");
                        DupeBee4FSMDupe.SendEvent("SWARM");
                        DupeBee5FSMDupe.SendEvent("SWARM");
                        DupeBee6FSMDupe.SendEvent("SWARM");
                        DupeBee7FSMDupe.SendEvent("SWARM");
                    }

                }
            };

            fsm.Fsm.GetState("Roar Recover").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    Bee1FSM.SendEvent("SWARM");
                    Bee2FSM.SendEvent("SWARM");
                    Bee3FSM.SendEvent("SWARM");
                    Bee4FSM.SendEvent("SWARM");
                    Bee5FSM.SendEvent("SWARM");
                    Bee6FSM.SendEvent("SWARM");
                    Bee7FSM.SendEvent("SWARM");

                    if (!Toggles.onlyboss)
                    {
                        Bee1FSMDupe.SendEvent("SWARM");
                        Bee2FSMDupe.SendEvent("SWARM");
                        Bee3FSMDupe.SendEvent("SWARM");
                        Bee4FSMDupe.SendEvent("SWARM");
                        Bee5FSMDupe.SendEvent("SWARM");
                        Bee6FSMDupe.SendEvent("SWARM");
                        Bee7FSMDupe.SendEvent("SWARM");
                    }
                }
            };

            if (GameManager.instance.sceneName.Contains("GG"))
            {
                fsmd.SendEvent("WAKE");
            }
            else
            {
                GameObject battle = GameObject.Find("Battle Scene");
                PlayMakerFSM bfsm = battle.LocateMyFSM("Control");
                bfsm.AddCustomAction("Hive Knight", () =>
                {
                    fsmd.SendEvent("WAKE");
                });

                if (!Toggles.onlyboss) return;

                bfsm.Fsm.GetState("Droppers").Actions[0] = new CustomFsmAction()
                {
                    method = () => {
                        Bee1FSMDupe.SendEvent("SWARM");
                        Bee1FSM.SendEvent("SWARM");
                        Bee2FSMDupe.SendEvent("SWARM");
                        Bee2FSM.SendEvent("SWARM");
                        Bee3FSMDupe.SendEvent("SWARM");
                        Bee3FSM.SendEvent("SWARM");
                        Bee4FSMDupe.SendEvent("SWARM");
                        Bee4FSM.SendEvent("SWARM");
                        Bee5FSMDupe.SendEvent("SWARM");
                        Bee5FSM.SendEvent("SWARM");
                        Bee6FSMDupe.SendEvent("SWARM");
                        Bee6FSM.SendEvent("SWARM");
                        Bee7FSMDupe.SendEvent("SWARM");
                        Bee7FSM.SendEvent("SWARM");
                    }
                };
            }
        }
    }
}