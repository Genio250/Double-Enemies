using FiveKnights;
using FiveKnights.Hegemol;
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
    public static class Hegemol
    {
        public static void HegemolAI()
        {
            GameObject.Find("Hegemol").name = "HegemolOrig";
            BossLoader.LoadHegemolBundle();
            BossLoader.LoadHegemolSound();
            BossLoader.CreateHegemol();
            GameObject.Find("Hegemol").name = "Hegemol(EnemyDupe)";
            GameObject.Find("HegemolOrig").name = "Hegemol";

            GameObject Heg1 = GameObject.Find("Hegemol");

            GameObject Heg2 = GameObject.Find("Hegemol(EnemyDupe)");

            Phaser(Heg1, "Phase1HP", CustomWP.lev > 0 ? 1300 : 1200);
            Phaser(Heg2, "Phase1HP", CustomWP.lev > 0 ? 1300 : 1200);
            Phaser(Heg1, "Phase2HP", CustomWP.lev > 0 ? 1600 : 1400);
            Phaser(Heg2, "Phase2HP", CustomWP.lev > 0 ? 1600 : 1400);
            Phaser(Heg1, "Phase3HP", CustomWP.lev > 0 ? 1900 : 1600);
            Phaser(Heg2, "Phase3HP", CustomWP.lev > 0 ? 1900 : 1600);
        }

        private static void Phaser(GameObject Heg, string Phase, int hp)
        {
            var setup = Heg.GetComponent<HegemolController>();
            FieldInfo P12 = typeof(HegemolController).GetField
                (
                    Phase,
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
            P12.SetValue(setup, hp);
        }
    }
}