using HutongGames.PlayMaker.Actions;
using Modding;
using Satchel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoubleEnemies
{
    public static class NKGFix
    {
        public static void NKG(GameObject enemy, GameObject New)
        {
            HealthManager NkgHp = enemy.GetComponent<HealthManager>();
            int a = NkgHp.hp;
            GameObject RealBat = GameObject.Find("Real Bat");
            GameObject RealBatDupe = GameObject.Find("Real Bat(EnemyDupe)");
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () =>
            {
                for (int i = 12; i > 0; i--)
                {
                    GameObject hatchling = UnityEngine.Object.Instantiate(HatchlingPrefab);
                    hatchling.transform.position = new Vector3(67, 30, 0);
                }
            }
                );
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2, () =>
            {
                New.manageHealth(a);
                enemy.manageHealth(a);
            }
            );
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2.5f, () =>
            {
                RealBat.transform.position = new Vector3(1000, 1000, 0);
                RealBatDupe.transform.position = new Vector3(1000, 1000, 0);
            }
            );
        }

        private static GameObject HatchlingPrefab => field ??=
            ((HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool)HeroController.instance.transform.Find("Charm Effects").
            gameObject.LocateMyFSM("Hatchling Spawn").Fsm.GetState("Hatch").Actions[2]).gameObject.Value;
    }
}