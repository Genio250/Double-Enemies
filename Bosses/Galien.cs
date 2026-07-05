using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoubleEnemies
{
    public static class Galien
    {
        public static void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
        private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "GG_Ghost_Galien" || arg1.name == "Deepnest_40")
            {
                GameObject scythe = GameObject.Find("Galien Hammer");
                DoubleEnemies.Duplicate(scythe, false);

                GameObject scythe2 = GameObject.Find("Galien Hammer(EnemyDupe)");
                scythe2.transform.position += new Vector3(10, 0, 0);
                PlayMakerFSM scythe2FSM = scythe2.LocateMyFSM("Control");
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => scythe2FSM.SendEvent("READY"));
            }
        }
    }
}