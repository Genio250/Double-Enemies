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
    public static class SMaster
    {
        public static void SMasterAI()
        {
            GameObject Spinner = GameObject.Find("Orb Spinner");
            GameObject Spinner2 = GameObject.Instantiate(Spinner);
            Spinner2.name = "Orb Spinner(EnemyDupe)";
            PlayMakerFSM Sfsm = Spinner2.LocateMyFSM("Spin Control");
            PlayMakerFSM Sfsm2 = Spinner2.LocateMyFSM("deparent_and_follow");

            GameObject Master = GameObject.Find("Mage Lord(EnemyDupe)");
            PlayMakerFSM Mfsm = Master.LocateMyFSM("Mage Lord");
            Mfsm.FsmVariables.GetFsmGameObject("Orb Spinner").Value = Spinner2;
            Sfsm.FsmVariables.GetFsmGameObject("Parent").Value = Master;
            Sfsm2.FsmVariables.GetFsmGameObject("Parent").Value = Master;
            Spinner2.transform.SetParent(Master.transform);
        }
    }
}