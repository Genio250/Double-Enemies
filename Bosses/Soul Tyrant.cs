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
    public static class STyrant
    {
        public static void STyrantAI()
        {
            bool Done = true;
            GameObject Spinner = null;
            while (Done)
            {
                GameObject check = GameObject.Find("Orb Spinner");
                PlayMakerFSM fsm = check.LocateMyFSM("Summon Orbs");
                HutongGames.PlayMaker.FsmState state = fsm.Fsm.GetState("Spawn");
                int orbs = state.Actions.Length;
                if (orbs == 12)
                {
                    check.SetActive(false);
                }
                else
                {
                    Spinner = check;
                    Done = false;
                }
            }
            GameObject Spinner2 = GameObject.Instantiate(Spinner);
            Spinner2.name = "Orb Spinner(EnemyDupe)";
            PlayMakerFSM Sfsm = Spinner2.LocateMyFSM("Spin Control");
            PlayMakerFSM Sfsm2 = Spinner2.LocateMyFSM("deparent_and_follow");

            GameObject Tyrant = GameObject.Find("Dream Mage Lord(EnemyDupe)");
            PlayMakerFSM Tfsm = Tyrant.LocateMyFSM("Mage Lord");
            Tfsm.FsmVariables.GetFsmGameObject("Orb Spinner").Value = Spinner2;
            Sfsm.FsmVariables.GetFsmGameObject("Parent").Value = Tyrant;
            Sfsm2.FsmVariables.GetFsmGameObject("Parent").Value = Tyrant;
            Spinner2.transform.SetParent(Tyrant.transform);
        }
    }
}