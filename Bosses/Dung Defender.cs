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
        private static int DungDefAvoider = 0;
        private static void DungDefFollow()
        {
            if (GameManager.instance.sceneName == "GG_Dung_Defender" || GameManager.instance.sceneName == "Waterways_05")
            {
                GameObject Burrow = GameObject.Find("Burrow Effect(EnemyDupe)");
                GameObject DD = GameObject.Find("Dung Defender(EnemyDupe)");
                if(DD != null) Burrow.transform.position = new Vector3(DD.transform.position.x, Burrow.transform.position.y, Burrow.transform.position.z);
            }
            else if (DungDefAvoider == 1)
            {
                DungDefAvoider--;
                ModHooks.HeroUpdateHook -= DungDefFollow;
            }
        }

        public static void DungDefAI()
        {
            GameObject DD = GameObject.Find("Dung Defender(EnemyDupe)");
            PlayMakerFSM Dfsm = DD.LocateMyFSM("Dung Defender");
            GameObject Burrow = GameObject.Find("Burrow Effect");
            GameObject Burrow2 = GameObject.Instantiate(Burrow);
            Burrow2.name = "Burrow Effect(EnemyDupe)";

            Dfsm.FsmVariables.GetFsmGameObject("Burrow Effect").Value = Burrow2;

            ModHooks.HeroUpdateHook += DungDefFollow;
            DungDefAvoider = 1;
        }
    }
}