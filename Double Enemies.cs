using HutongGames.PlayMaker.Actions;
using IL;
//using IL.HutongGames.PlayMaker.Actions;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using On;
//using On.HutongGames.PlayMaker.Actions;
using QoL;
using Satchel;
using Satchel.Futils;
using Satchel.Futils.Serialiser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Mono.Security.X509.X520;
using HutongGames.PlayMaker;
using Steamworks;

namespace DoubleEnemies
{

    public class DoubleEnemies : Mod, IMenuMod
    {
        public DoubleEnemies() : base("Double Enemies") { }
        public override string GetVersion() => "1.1.0";
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
                }
            };
        }
        public int counter = 0;
        public static bool Mantis = true;
        public static GameObject MantisLord = null;

        public override void Initialize()
        {
            Log("Initializing");
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;
            if (ModHooks.GetMod("QoL") is Mod)
            {
                Toggles.QoL = true;
                Log("Reading QoL");
            }
            ModHooks.BeforeSceneLoadHook += ModHooks_BeforeSceneLoadHook;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
            Galien.Initialize();
            HPShare.Initialize();
        }

        /*void Filler()
        {
            GameObject MossyDupe = GameObject.Find("Mega Moss Charger(EnemyDupe)");
            PlayMakerFSM fsm = MossyDupe.LocateMyFSM("Mossy Control");
            fsm.SendEvent("WAKE");
            fsm.Fsm.GetState("Hidden").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    fsm.SetState("Emerge Pause");
                }
            };

            Log(fsm.ActiveStateName);
            Log(fsm.FsmVariables.FindFsmInt("P2 HP").Value);
        }

        void Test()
        {
            GameObject MossyDupe = GameObject.Find("Pale Lurker(EnemyDupe)");
            PlayMakerFSM fsm = MossyDupe.LocateMyFSM("Lurker Control");
            Log(fsm.ActiveStateName);
        }

 */
        void Bow()
        {

        }

        private void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Log("Attempting to give AI");
                //Test();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                Log("Attempting to give AI");
                Bow();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                GameObject Battle = GameObject.Find("Battle Scene v2");
                PlayMakerFSM fsmB = Battle.LocateMyFSM("Battle Control");
                Log(fsmB.FsmVariables.GetFsmInt("Battle Enemies").Value);
            }

        }



        private string ModHooks_BeforeSceneLoadHook(string arg)
        {
            if(arg == "Fungus1_21")
            {
                Log("PreHornet");
                PlayerData.instance.SetInt("hornetGreenpath", 4);
            }
            return arg;
        }


        private bool ModHooks_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            if (!isAlreadyDead && !enemy.name.Contains("(EnemyDupe)"))
            {
                bool tp = false, skip = false;
                float wait = 0;
                if (enemy.name.Contains("Mega Fat Bee")) wait = 2;
                else if (enemy.name.Contains("Sheo")) { wait = 0.1f; tp = true; }
                else if (enemy.name.Contains("Nightmare Grimm Boss")) wait = 7;
                else if (enemy.name.Contains("Ghost Warrior Markoth")) wait = 1;
                else if (enemy.name.Contains("Shade Sibling")) wait = 1;
                else if (enemy.name.Contains("Mawlek Body")) { wait = 0.1f; tp = true; }
                else if (enemy.name.Contains("Lobster")) wait = 5;
                else if (enemy.name.Contains("Mage Balloon Spawner")) wait = 1;
                else if (enemy.name.Contains("Jar Collector")) wait = 0.1f;
                else if (enemy.name.Contains("Grimm Boss")) { wait = 0.1f; tp = true; }
                else if (enemy.name.Contains("Giant Fly")) wait = 0.5f;
                else if (enemy.name.Contains("False Knight New")) { wait = 0.3f; tp = true; }
                else if (enemy.name.Contains("Hornet Boss 1")) { wait = 3; tp = true; }
                else if (enemy.name.Contains("Zombie Beam Miner Rematch")) { wait = 3; tp = true; }
                else wait = 0;
                foreach (string s in Lists.Exceptions)
                {
                    if (enemy.name.Contains(s)) skip = true;
                }

                if (!skip)
                {
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(wait, () => Duplicate(enemy, tp));
                }

                if (enemy.name == "Oro")
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => OroYMato.OroYMatoAI());
                if (enemy.name.Contains("Mega Moss Charger"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Mossy.MossyAI());
                if (enemy.name.Contains("Lobster"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(5.1f, () => GodTamer.LobsterAI());
                if (enemy.name.Contains("Hornet Nosk"))
                    Nosket.NosketAI();
                if (enemy.name.Contains("Mega Jellyfish GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Uumuu.UumuuAI());
                if (enemy.name.Contains("Hive Knight"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => HiveKnight.HiveKnightAI());
                if (enemy.name.Contains("Sly Boss"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Sly.SlyAI());
                if (enemy.name.Contains("Black Knight 1"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.2f, () => Watchers.WatchersAI());
                if (enemy.name.Contains("Mega Zombie Beam Miner (1)") && GameManager.instance.sceneName.Contains("GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.2f, () => CG.CrystalAI());
                if (enemy.name.Contains("Zombie Beam Miner Rematch") && GameManager.instance.sceneName.Contains("GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(3.1f, () => EG.EnragedAI());
                if (enemy.name.Contains("Mushroom Brawler 1"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Ogres.OgresAI());

                if (enemy.name.Contains("Mantis Lord"))
                {
                    MantisFix.MantisLords(Mantis, enemy);
                }
            }
            

            return isAlreadyDead; //Zote
        }

        public static void Duplicate(GameObject enemy, bool tp)
        {
            GameObject New = UnityEngine.GameObject.Instantiate(enemy);
            New.name = enemy.name + "(EnemyDupe)";
            if(tp) New.transform.position = enemy.transform.position;
            Modding.Logger.Log("Duping " + enemy.name);

            //Take the actions to fix NKG
            if (enemy.name.Contains("Nightmare Grimm Boss"))
            {
                NKG.NkgAI();
            }

            if (enemy.name.Contains("HK Prime"))
            {
                PureVessel.PVSkip();
            }

            //HPShare.DoubleHP(enemy);
            HPShare.DoubleHP(enemy, New);

            Offset.EnemyOffset(enemy, New);
        }


            //Log(fsm.ActiveStateName);
            //Log(fsm.FsmVariables.FindFsmInt("P2 HP").Value);
            /*
             *  Sync Boss Phases to doubled hp
             *  Spawns that are alraedy duped
             *  Some enemies can't be duped
             *  Mimics get deleted 
             *  Arenas
             *  
             *  Pale Lurker
             *  OW Nosk
             *  OW CG and EG 
             *  Potentially dupe Xero spawn swords
             *  Idk why ascended warrior sometimes tps oob
             *  Fix OW Galien Scythe
             *  OW Uumuu
             *  WD underground follows the main
             *  Tyrant Orbs follows the main
             *  
            */


        }
    }