using HutongGames.PlayMaker.Actions;
using IL.HutongGames.PlayMaker.Actions;
using InControl;
using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Mono.Security.X509.X520;

namespace DoubleEnemies
{
    public static class Toggles
    {
        public static bool mod; public static int amount;
    }
    public class DoubleEnemies : Mod, IMenuMod
    {
        public DoubleEnemies() : base("Double Enemies") { }
        public override string GetVersion() => "0.1.0";
        public bool ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry
                {
                    Name = "Mod toggle",
                    Description = null,
                    Values = new string[] { "Off", "On" },
                    Saver = (i) => Toggles.mod = i == 1,
                    Loader = () => Toggles.mod ? 1 : 0
                }/*, new IMenuMod.MenuEntry
                {
                    Name = "Dashmaster",
                    Description = "Amount of duplicate enemies. Check ReadMe for exceptions above 2",
                    Values = new string[] { "2", "3", "4", "5" },
                    Saver = opt => Toggles.amount = opt,
                    Loader = () => Toggles.amount
                }*/
            };
        }
        public override void Initialize()
        {
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;
        }

        private bool ModHooks_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            if (!isAlreadyDead && !enemy.name.Contains("(EnemyDupe)") && Toggles.mod)
            {
                GameObject New = UnityEngine.GameObject.Instantiate(enemy);
                New.name = enemy.name + "(EnemyDupe)";
            }
            return isAlreadyDead;
        }

        /*
         *  Spawns that are alraedy duped
         *  Add AbsRad dependency
         *  Don't dupe tyrant phase 2
         *  Wait on mega baloon spawner
         *  Vengeflies and shadow creepers just sync up, so maybe wait on them
         *  Doesn't work: Hive Knight, Uumuu, NKG, Enraged, Sheo, WK, Mawlek, CG, Wosk
         *  PV fast animation
         *  Oro and Mato phase 2
         *  Galien Scythe
         *  TMG is in the foreground
         *  Maybe also add wait to Markoth
         *  Potentially dupe Xero spawn swords
         *  Duping flukmarm flukeefeys would be funny
         *  3 Mantises???? Into normal SoB
         *  
         *  QoL OW Nosk
         *  
         *  Hard:
         *  Sly breaks
         *  WD underground follows the main
         *  Tyrant Orbs follows the main
         *  
        */
    }
}