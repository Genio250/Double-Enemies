using FiveKnights;
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

namespace DoubleEnemies
{
    public static class Tiso
    {
        public static void TisoAI()
        {
            GameObject.Find("Tiso(Clone)").name = "Tiso(Clone)Orig";
            BossLoader.LoadTisoBundle();
            BossLoader.CreateTiso();
            GameObject.Find("Tiso(Clone)").name = "Tiso(Clone)(EnemyDupe)";
            GameObject.Find("Tiso(Clone)Orig").name = "Tiso(Clone)";

            GameObject.Find("Tiso(Clone)").GetComponent<HealthManager>().hp *= 2;
            GameObject.Find("Tiso(Clone)(EnemyDupe)").GetComponent<HealthManager>().hp *= 2;
        }
    }
}