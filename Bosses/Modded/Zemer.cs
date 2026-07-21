using FiveKnights;
using FiveKnights.Zemer;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DoubleEnemies
{
    public static class Zemer
    {
        public static void ZemerAI()
        {
            GameObject.Find("Zemer(Clone)").name = "Zemer(Clone)Orig";
            BossLoader.LoadZemerBundle();
            BossLoader.LoadZemerSound();
            BossLoader.CreateZemer();
            GameObject.Find("Zemer(Clone)").name = "Zemer(Clone)(EnemyDupe)";
            GameObject.Find("Zemer(Clone)Orig").name = "Zemer(Clone)";

            GameObject Zem1 = GameObject.Find("Zemer(Clone)");
            GameObject Zem2 = GameObject.Find("Zemer(Clone)(EnemyDupe)");

            Zem1.GetComponent<HealthManager>().hp *= 2;
            Zem2.manageHealth(Zem1.GetComponent<HealthManager>().hp);

            ModHooks.HeroUpdateHook += DeathCheck;
        }

        private static void DeathCheck()
        {
            GameObject Zem1 = GameObject.Find("Zemer(Clone)");
            GameObject Zem2 = GameObject.Find("Zemer(Clone)(EnemyDupe)");
            var setup = Zem1.GetComponent<ZemerControllerP2>();

            if (setup != null)
            {
                Zem1.GetComponent<HealthManager>().hp *= 2;
                Zem2.manageHealth(Zem1.GetComponent<HealthManager>().hp);
                Phaser(Zem1, "Phase2HP", CustomWP.lev > 0 ? 3000 : 2700);
                Phaser(Zem2, "Phase2HP", CustomWP.lev > 0 ? 3000 : 2700);
                Phaser(Zem1, "Phase3HP", CustomWP.lev > 0 ? 1900 : 1600);
                Phaser(Zem2, "Phase3HP", CustomWP.lev > 0 ? 1900 : 1600);
                ModHooks.HeroUpdateHook -= DeathCheck;
            }
        }

        private static void Phaser(GameObject Zem, string Phase, int hp)
        {
            var setup = Zem.GetComponent<ZemerControllerP2>();
            FieldInfo P12 = typeof(ZemerControllerP2).GetField
                (
                    Phase,
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
            P12.SetValue(setup, hp);
        }
    }
}