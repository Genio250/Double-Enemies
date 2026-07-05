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
    public static class OroYMato
    {
        public static void OroYMatoAI()
        {
            GameObject OrigOro = GameObject.Find("Oro");
            PlayMakerFSM OrigOfsm = OrigOro.LocateMyFSM("nailmaster");
            GameObject OrigMato = GameObject.Find("Oro");
            PlayMakerFSM OrigMfsm = OrigMato.LocateMyFSM("nailmaster");
            GameObject Oro = GameObject.Find("Oro(EnemyDupe)");
            PlayMakerFSM Ofsm = Oro.LocateMyFSM("nailmaster");
            GameObject Mato = GameObject.Find("Mato(EnemyDupe)");
            PlayMakerFSM Mfsm = Mato.LocateMyFSM("nailmaster");

            Mfsm.FsmVariables.FindFsmGameObject("Brother").Value = Oro;
            Ofsm.FsmVariables.FindFsmGameObject("Brother").Value = Mato;
            Ofsm.RemoveAction("Call Mato", 2);

            Ofsm.FsmVariables.FindFsmInt("P2 HP").Value *= 2;
            OrigOfsm.FsmVariables.FindFsmInt("P2 HP").Value *= 2;

            Ofsm.InsertCustomAction("Call Mato", () =>
            {
                Mfsm.SendEvent("ENTER");
            }, 2);

            Ofsm.RemoveAction("Defeated", 0);
            Mfsm.RemoveAction("Defeated", 0);

            OrigMfsm.InsertCustomAction("Bow", () =>
            {
                Mfsm.SendEvent("BOW");
                Ofsm.SendEvent("BOW");
            }, 0);
        }
    }
}