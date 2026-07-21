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
        public static void SlyAI()
        {
            GameObject SlyDupe = GameObject.Find("Sly Boss(EnemyDupe)");
            PlayMakerFSM fsm = SlyDupe.LocateMyFSM("Control");
            //Log(fsm.ActiveStateName);

            GameObject Spin = GameObject.Find("Spin Tink");
            GameObject Spin2 = GameObject.Instantiate(Spin);
            PlayMakerFSM Spin2FSM = Spin2.LocateMyFSM("Follow");
            Spin2.name = "Spin Tink(EnemyDupe)";
            Spin2.transform.parent = null;
            fsm.FsmVariables.GetFsmGameObject("Spin Tink").Value = Spin2;
            Spin2FSM.FsmVariables.GetFsmGameObject("Parent").Value = SlyDupe;
            Spin2.transform.SetParent(SlyDupe.transform);

            GameObject Stun = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Stun Nail");
            //Log(Stun);
            GameObject Stun2 = GameObject.Instantiate(Stun);
            PlayMakerFSM Stun2FSM = Stun2.LocateMyFSM("Stun Nail");
            Stun2.name = "Stun Nail(EnemyDupe)";
            fsm.FsmVariables.GetFsmGameObject("Stun Nail").Value = Stun2;
            Stun2FSM.FsmVariables.GetFsmGameObject("Sly").Value = SlyDupe;
            Stun2.transform.SetParent(SlyDupe.transform);

            GameObject Cyclone = GameObject.Find("Cyclone Tink");
            GameObject Cyclone2 = GameObject.Instantiate(Cyclone, Cyclone.transform.parent);
            PlayMakerFSM Cyclone2FSM = Cyclone2.LocateMyFSM("Follow");
            Cyclone2.name = "Cyclone Tink(EnemyDupe)";
            fsm.FsmVariables.GetFsmGameObject("Cyclone Tink").Value = Cyclone2;
            Cyclone2FSM.FsmVariables.GetFsmGameObject("Parent").Value = SlyDupe;
            Cyclone2.transform.SetParent(SlyDupe.transform);
        }
    }
}