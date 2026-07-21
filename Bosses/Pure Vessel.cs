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
        public static void PVSkip()
        {
            PlayMakerFSM fsm = GameObject.Find("HK Prime").LocateMyFSM("Control");
            PlayMakerFSM fsmd = GameObject.Find("HK Prime(EnemyDupe)").LocateMyFSM("Control");
            if (Toggles.QoL)
            {
                if (QoL.Modules.SkipCutscenes.PureVesselRoar)
                {
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.3f, () =>
                    {
                        fsmd.ChangeTransition("Intro 1", "FINISHED", "Intro 3");
                        fsmd.SetState("Intro 1");
                        fsmd.SendEvent("FINISHED");
                        fsmd.InsertCustomAction("Intro 6", () =>
                        {
                            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1.0f, () =>
                            {
                                fsmd.SendEvent("END");
                            });
                        }, 2);
                    });
                }
            }
            fsm.FsmVariables.FindFsmInt("Half HP").Value *= 2;
            fsmd.FsmVariables.FindFsmInt("Half HP").Value *= 2;
            fsm.FsmVariables.FindFsmInt("Quarter HP").Value *= 2;
            fsmd.FsmVariables.FindFsmInt("Quarter HP").Value *= 2;

            GameObject Blast1 = GameObject.Find("HK Prime Blast");
            GameObject Blast1Dupe = GameObject.Instantiate(Blast1, Blast1.transform.parent);
            PlayMakerFSM Blast1DupeFSM = Blast1Dupe.LocateMyFSM("Control");
            Blast1Dupe.name = "HK Prime Blast(EnemyDupe)";
            Blast1Dupe.transform.parent = null;

            GameObject Blast2 = GameObject.Find("HK Prime Blast (1)");
            GameObject Blast2Dupe = GameObject.Instantiate(Blast2, Blast2.transform.parent);
            PlayMakerFSM Blast2DupeFSM = Blast2Dupe.LocateMyFSM("Control");
            Blast2Dupe.name = "HK Prime Blast (1)(EnemyDupe)";
            Blast2Dupe.transform.parent = null;

            GameObject Blast3 = GameObject.Find("HK Prime Blast (2)");
            GameObject Blast3Dupe = GameObject.Instantiate(Blast3, Blast3.transform.parent);
            PlayMakerFSM Blast3DupeFSM = Blast3Dupe.LocateMyFSM("Control");
            Blast3Dupe.name = "HK Prime Blast (2)(EnemyDupe)";
            Blast3Dupe.transform.parent = null;

            GameObject Blast4 = GameObject.Find("HK Prime Blast (3)");
            GameObject Blast4Dupe = GameObject.Instantiate(Blast4, Blast4.transform.parent);
            PlayMakerFSM Blast4DupeFSM = Blast4Dupe.LocateMyFSM("Control");
            Blast4Dupe.name = "HK Prime Blast (3)(EnemyDupe)";
            Blast4Dupe.transform.parent = null;

            GameObject Blast5 = GameObject.Find("HK Prime Blast (4)");
            GameObject Blast5Dupe = GameObject.Instantiate(Blast5, Blast5.transform.parent);
            PlayMakerFSM Blast5DupeFSM = Blast5Dupe.LocateMyFSM("Control");
            Blast5Dupe.name = "HK Prime Blast (4)(EnemyDupe)";
            Blast5Dupe.transform.parent = null;

            GameObject Blast6 = GameObject.Find("HK Prime Blast (5)");
            GameObject Blast6Dupe = GameObject.Instantiate(Blast6, Blast6.transform.parent);
            PlayMakerFSM Blast6DupeFSM = Blast6Dupe.LocateMyFSM("Control");
            Blast6Dupe.name = "HK Prime Blast (5)(EnemyDupe)";
            Blast6Dupe.transform.parent = null;

            fsmd.Fsm.GetState("Focus Burst").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    Blast1DupeFSM.SendEvent("BLAST");
                    Blast2DupeFSM.SendEvent("BLAST");
                    Blast3DupeFSM.SendEvent("BLAST");
                    Blast4DupeFSM.SendEvent("BLAST");
                    Blast5DupeFSM.SendEvent("BLAST");
                    Blast6DupeFSM.SendEvent("BLAST");
                    Modding.Logger.Log("HK Prime Dupe Blasted");
                }
            };
        }
    }
}