using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;

namespace DoubleEnemies
{
    public static class Lists
    {
        public static List<string> Bosses = ["Giant Fly", "Giant Buzzer Col", "Giant Buzzer Col (1)", "False Knight New", "False Knight Dream",
            "Hornet Boss 1", "Mawlek Body", "Hornet Boss 2", "Fluke Mother", "Mega Fat Bee", "Mega Fat Bee (1)", "Mantis Lord",
            "Infected Knight", "Lost Kin", "Mimic Spider", "Jar Collector", "Lancer", "Mantis Traitor Lord", "Grey Prince", "Nightmare Grimm Boss",
            "Grimm Boss", "HK Prime", "Sheo Boss", "Ghost Warrior Hu", "Ghost Warrior Slug", "Ghost Warrior Galien", "Ghost Warrior Markoth",
            "Ghost Warrior Xero", "Ghost Warrior Marmu", "Ghost Warrior No Eyes", "Dung Defender", "White Defender", "Dream Mage Lord",
            "Dream Mage Lord Phase2", "Mage Lord", "Mage Lord Phase2", "Mage Knight", "Hollow Knight Boss", "Mega Jellyfish", "Mantis Lord S1",
            "Mantis Lord S2", "Mantis Lord S3"];

        public static List<string> DWarriors = ["Ghost Warrior Hu", "Ghost Warrior Slug", "Ghost Warrior Galien", "Ghost Warrior Markoth",
            "Ghost Warrior Xero", "Ghost Warrior Marmu", "Ghost Warrior No Eyes"];

        public static List<string> DWarriorArenas = ["Fungus2_32", "Cliffs_02", "Deepnest_40", "Deepnest_East_10", "RestingGrounds_04",
            "Fungus3_40", "Fungus1_35"];

        public static List<string> Exceptions = ["Radiance", "Head", "Tinger", "Lobster", "Mega Jellyfish GG", "Mega Moss Charger",
            "Mantis Lord", "Zombie Beam Miner", "Sly Boss", "Oro", "Mato"];
    }
    public static class Toggles
    {
        public static bool mod = true, PV = false, Nosk = false;
        public static int amount;
    }
}