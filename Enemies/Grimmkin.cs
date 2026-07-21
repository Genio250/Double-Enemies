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
        public static string current;

        public static void GrimmkinAI(string s)
        {
            current = s + "(EnemyDupe)";
            GameObject GrimmKin = GameObject.Find(s);
            PlayMakerFSM fsm = GrimmKin.LocateMyFSM("Control");
            GameObject GrimmKin2 = GameObject.Find(current);
            PlayMakerFSM fsm2 = GrimmKin2.LocateMyFSM("Control");

            fsm.InsertCustomAction("Set Level", () =>
            {
                fsm2.SendEvent("START");
            }, 0);
            fsm2.RemoveAction("Follow", 8);

            fsm.InsertCustomAction("Death Start", () =>
            {
                if(GrimmKin2 != null)
                {
                    if(GrimmKin2.GetComponent<HealthManager>().hp > 0)
                    {
                        fsm2.SendEvent("ZERO HP");
                    }
                }
            }, 0);

            fsm2.InsertCustomAction("Death Start", () =>
            {
                if (GrimmKin != null)
                {
                    if (GrimmKin.GetComponent<HealthManager>().hp > 0)
                    {
                        fsm.SendEvent("ZERO HP");
                    }
                }
            }, 0);

            if (Toggles.drops)
            {
                fsm2.InsertCustomAction("Explode", () =>
                {
                    PlayerData.instance.SetInt("flamesCollected", PlayerData.instance.GetInt("flamesCollected") + 1);
                }, 0);
            }
        }
    }
}