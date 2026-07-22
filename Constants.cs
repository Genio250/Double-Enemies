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
            "Dream Mage Lord Phase2", "Mage Lord", "Mage Lord Phase2", "Mage Knight", "Hollow Knight Boss", "Mantis Lord S1", "Mantis Lord S2",
            "Mantis Lord S3", "Mega Jellyfish", "Lobster", "Mega Moss Charger", "Hornet Nosk", "Mega Jellyfish GG", "Hive Knight", "Sly Boss",
            "Oro", "Mato", "Black Knight 1", "Black Knight 2", "Black Knight 3", "Black Knight 4", "Black Knight 5", "Black Knight 6",
            "Mega Zombie Beam Miner (1)", "Zombie Beam Miner Rematch", "Royal Gaurd", "Royal Gaurd (1)", "Mushroom Brawler 1",
            "Mushroom Brawler 2", "Pale Lurker", "Flamebearer Small(Clone)", "Flamebearer Med(Clone)", "Flamebearer Large(Clone)",
            "Black Knight 7", "Black Knight 8", "Black Knight 9", "Black Knight 10", "Black Knight 11", "Black Knight 12", "Black Knight 13",
            "Black Knight 14", "Black Knight 15", "Cagney Carnation", "Dryya2(Clone)", "Tiso(Clone)", "Hegemol", "Zemer(Clone)"];

        public static List<string> Exceptions = ["Radiance", "Head", "Tinger", "Mantis Lord", "Hegemol", "Zemer", "Isma", "Tiso", "Fly 1", "Fly 2", "Fly 3",
            "Fly 4", "Fly 5", "Fly 6", "Shell"];

        public static List<string> Tripled = ["Dream Mage Lord Phase2", "Mini Flower", "Chomper"];

        public static List<string> Offset = ["Turret", "Plant Trap", "Acid Walker", "Ceiling Dropper", "Abyss Crawler", "Egg Sac", "Fat Fluke",
        "Crystallised Lazer Bug", "Mines Crawler", "Great Shield Zombie Bottom", "Moss Knight", "Mantis", "Fung Crawler", "Flip Hopper", "Zombie Shield",
        "Hopper"];
    }
    public static class Toggles
    {
        public static bool mod = true, onlyboss = false, drops = true, colo = false;
        public static bool QoL = false, Everwatchers = false, Cagney = false, PaleCourt = false;
        public static int amount;
    }
}