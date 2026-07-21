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
        private static int WhiteDefAvoider = 0;
        private static void WhiteDefFollow()
        {
            if (GameManager.instance.sceneName.Contains("White_Defender"))
            {
                GameObject Burrow = GameObject.Find("Burrow Effect(EnemyDupe)");
                GameObject WD = GameObject.Find("White Defender(EnemyDupe)");
                if(WD != null) Burrow.transform.position = new Vector3(WD.transform.position.x, Burrow.transform.position.y, Burrow.transform.position.z);
            }
            else if (WhiteDefAvoider == 1)
            {
                WhiteDefAvoider--;
                ModHooks.HeroUpdateHook -= WhiteDefFollow;
            }
        }

        public static void WhiteDefAI()
        {
            GameObject WD = GameObject.Find("White Defender(EnemyDupe)");
            PlayMakerFSM Dfsm = WD.LocateMyFSM("Dung Defender");
            GameObject Burrow = GameObject.Find("Burrow Effect");
            GameObject Burrow2 = GameObject.Instantiate(Burrow);
            Burrow2.name = "Burrow Effect(EnemyDupe)";

            Dfsm.FsmVariables.GetFsmGameObject("Burrow Effect").Value = Burrow2;
            Dfsm.RemoveAction("Init 2", 0);

            ModHooks.HeroUpdateHook += WhiteDefFollow;
            WhiteDefAvoider = 1;
        }
    }
}