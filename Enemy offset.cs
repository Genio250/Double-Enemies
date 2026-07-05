using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Satchel;
using Modding;
using UnityEngine;

namespace DoubleEnemies
{
    public static class Offset
    {
        public static void EnemyOffset(GameObject enemy, GameObject New)
        {
            //Random offset to Collector and VFK vengeflies, Uumuu Oomas, and Warrior Follies
            Vector2 spawn2 = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 spawn3 = new Vector3(spawn2.x, spawn2.y, 0);
            if (enemy.name == "Jellyfish GG(Clone)")
                New.transform.position = enemy.transform.position + spawn3 * 3;

            if (enemy.name.Contains("Buzzer") && !enemy.name.Contains("Giant"))
            {
                if (GameManager.instance.sceneName == "GG_Collector" || GameManager.instance.sceneName == "Ruins2_11")
                    spawn3 = new Vector3(Math.Abs(spawn2.x), Math.Abs(spawn2.y), 0);
                New.transform.position = enemy.transform.position + spawn3 * 2;
            }

            if (enemy.name.Contains("Mage Balloon Spawner") || enemy.name.Contains("White Palace Fly"))
                New.transform.position = enemy.transform.position + spawn3;

            float offset = UnityEngine.Random.Range(-1f, 1f);


            foreach (string s in Lists.Offset)
            {
                if (enemy.name.Contains(s))
                {
                    if ((enemy.transform.rotation.eulerAngles.z < 100 || enemy.transform.rotation.eulerAngles.z > 200) &&
                        (enemy.transform.rotation.eulerAngles.z > 10 && enemy.transform.rotation.eulerAngles.z < 300))
                    {
                        New.transform.position = enemy.transform.position + new Vector3(0, offset, 0);
                    }
                    else New.transform.position = enemy.transform.position + new Vector3(offset, 0, 0);
                }
            }
            //Offsets 1d enemies in their axis

            if(enemy.name.Contains("Royal Gaurd"))
            {
                New.transform.position = enemy.transform.position + new Vector3(2, 0, 0);
            }

            if (enemy.name.Contains("Flying Sentry"))
            {
                New.transform.position = enemy.transform.position + new Vector3(0, -2, 0);
            }


        }
    }
}