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
    public static class Xero
    {
        public static void XeroAI()
        {
            GameObject Xero = GameObject.Find("Ghost Warrior Xero(EnemyDupe)");
            PlayMakerFSM fsm = Xero.LocateMyFSM("Attacking");

            GameObject Sword1 = GameObject.Instantiate(GameObject.Find("Sword 1"));
            GameObject Sword2 = GameObject.Instantiate(GameObject.Find("Sword 2"));

            Sword1.name = "Sword 1(EnemyDupe)";
            Sword2.name = "Sword 2(EnemyDupe)";

            GameObject Home1 = GameObject.Find("Ghost Warrior Xero(EnemyDupe)/S1 Home");
            GameObject Home2 = GameObject.Find("Ghost Warrior Xero(EnemyDupe)/S2 Home");

            Sword1.LocateMyFSM("xero_nail").FsmVariables.GetFsmGameObject("Parent").Value = Xero;
            Sword2.LocateMyFSM("xero_nail").FsmVariables.GetFsmGameObject("Parent").Value = Xero;

            FsmOwnerDefault SwordOwner1 = new FsmOwnerDefault
            {
                OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                GameObject = new FsmGameObject { Value = Sword1 }
            };
            FsmOwnerDefault SwordOwner2 = new FsmOwnerDefault
            {
                OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                GameObject = new FsmGameObject { Value = Sword2 }
            };

            Sword1.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Home").gameObject = SwordOwner1;
            Sword2.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Home").gameObject = SwordOwner2;

            Sword1.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Home").target = Home1;
            Sword2.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Home").target = Home2;

            Sword1.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Returning").gameObject = SwordOwner1;
            Sword2.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Returning").gameObject = SwordOwner2;

            Sword1.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Returning").target = Home1;
            Sword2.LocateMyFSM("xero_nail").GetFirstActionOfType<DistanceFlySmooth>("Returning").target = Home2;

            fsm.FsmVariables.GetFsmGameObject("Sword 1").Value = Sword1;
            fsm.FsmVariables.GetFsmGameObject("Sword 2").Value = Sword2;
        }
    }
}