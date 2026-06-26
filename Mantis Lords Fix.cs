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
    public static class MantisFix
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