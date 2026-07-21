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
    public static partial class Bosses
    {
        public static void MantisLords(bool Mantis, GameObject enemy)
        {
            if (Mantis)
            {
                enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);
                if (enemy.name == "Mantis Lord")
                {
                    DoubleEnemies.Duplicate(enemy, false);
                    DoubleEnemies.MantisLord = UnityEngine.GameObject.Instantiate(enemy);
                    DoubleEnemies.MantisLord.name = "Memory Lord";
                    DoubleEnemies.MantisLord.SetActive(false);
                }
                else
                {
                    GameObject New = UnityEngine.GameObject.Instantiate(DoubleEnemies.MantisLord);
                    New.manageHealth(enemy.GetComponent<HealthManager>().hp);
                    New.name = enemy.name + "(EnemyDupe)";
                    New.SetActive(true);
                }
            }
            DoubleEnemies.Mantis = !Mantis;
        }
    }
}

/*            GameObject lordS1 = GameObject.Find("Mantis Lord S1");
            GameObject extra1 = GameObject.Instantiate(lordS1);
            extra1.name = "Mantis Lord S1(EnemyDupe)";

            GameObject lordS2 = GameObject.Find("Mantis Lord S2");
            GameObject extra2 = GameObject.Instantiate(lordS2);
            extra2.name = "Mantis Lord S2(EnemyDupe)";

            GameObject MantisBattle = GameObject.Find("Mantis Battle");
            PlayMakerFSM BattleControl = MantisBattle.LocateMyFSM("Battle Control");

            BattleControl.FsmVariables.FindFsmInt("Battle Enemies").Value = 4;

            GameObject BattleSub = GameObject.Find("Battle Sub");
            GameObject extraSub = GameObject.Instantiate(BattleSub, BattleSub.transform.parent);
            extraSub.name = "Battle Sub 2";

            PlayMakerFSM extraSubFSM = BattleSub.LocateMyFSM("Start");

            extraSubFSM.FsmVariables.FindFsmGameObject("Mantis 1").Value = GameObject.Find("Mantis Lord S1(EnemyDupe)");
            extraSubFSM.FsmVariables.FindFsmGameObject("Mantis 2").Value = GameObject.Find("Mantis Lord S2(EnemyDupe)");

            extraSubFSM.SetState("Init Pause");*/