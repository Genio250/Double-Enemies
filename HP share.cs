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
    public static class HPShare
    {
        public static void Initialize()
        {
            On.HealthManager.TakeDamage += HealthManager_TakeDamage;
        }
        private static int counter = 0;
        public static void HealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            GameObject Boss = null, Boss2 = null;
            bool commit = false;
            foreach (string s in Lists.Bosses)
            {
                GameObject go = GameObject.Find(s);
                GameObject go2 = GameObject.Find(s + "(EnemyDupe)");

                if (go != null)
                {
                    if (self == go.GetComponent<HealthManager>())
                    {
                        Boss = go;
                        Boss2 = go2;
                        commit = true;
                        break;
                    }
                    else if (self == go2.GetComponent<HealthManager>())
                    {
                        Boss = go2;
                        Boss2 = go;
                        commit = true;
                        break;
                    }
                }
            }
            if (commit && Boss2 != null)
            {
                HealthManager copy = Boss2.GetComponent<HealthManager>();
                if (!Boss2.name.Contains("Mega Fat Bee"))
                {
                    orig(self, hitInstance);
                    if (copy.hp - hitInstance.DamageDealt > 0) copy.hp -= hitInstance.DamageDealt;
                    else orig(copy, hitInstance);
                }
                else
                {
                    if (copy.hp - hitInstance.DamageDealt > 0)
                    {
                        orig(self, hitInstance);
                        copy.hp -= hitInstance.DamageDealt;
                    }
                    else
                    {
                        orig(self, hitInstance);
                        copy.hp -= 200;
                        orig(copy, hitInstance);
                    }
                }
            }
            else orig(self, hitInstance);
            counter++;
        }

        public static void DoubleHP(GameObject enemy, GameObject New)
        {
            foreach (string s in Lists.Bosses)
            {
                if (enemy.name.Contains(s) && s != "Mega Jellyfish" && !enemy.name.Contains("Mantis Lord") && !enemy.name.Contains("False Knight New"))
                    enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);
            }
            Satchel.CoroutineHelper.WaitForFramesBeforeInvoke(1, () => New.manageHealth(enemy.GetComponent<HealthManager>().hp));
            
        }
    }
}