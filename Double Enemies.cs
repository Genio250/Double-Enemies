using CagneyCarnation;
using Everwatchers;
using FiveKnights.Dryya;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using IL;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using On;
using QoL;
using Satchel;
using Satchel.Futils;
using Satchel.Futils.Serialiser;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using static Mono.Security.X509.X520;
using static UnityEngine.GraphicsBuffer;

namespace DoubleEnemies
{

    public class DoubleEnemies : Mod, IMenuMod
    {
        public DoubleEnemies() : base("Double Enemies") { }
        public override string GetVersion() => "1.1.4";
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
                },new IMenuMod.MenuEntry
                {
                    Name = "Only Double Bosses",
                    Description = "Disable the duplication of non boss enemies",
                    Values = new string[] { "On", "Off" },
                    Saver = (i) => Toggles.onlyboss = i == 0,
                    Loader = () => Toggles.onlyboss ? 0 : 1
                },new IMenuMod.MenuEntry
                {
                    Name = "Duplicate Drops",
                    Description = "Duplicate special enemy drops such as Grimmkin Flames",
                    Values = new string[] { "Off", "On" },
                    Saver = (i) => Toggles.drops = i == 1,
                    Loader = () => Toggles.drops ? 1 : 0
                }
            };
        }
        public static int counter = 0, togglerer = 0, trip = 0, dry = 0;
        public static bool Mantis = true;
        public static GameObject MantisLord = null;

        public override void Initialize()
        {
            Log("Initializing");
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;
            ModHooks.BeforeSceneLoadHook += ModHooks_BeforeSceneLoadHook;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
            Galien.Initialize();
            HPShare.Initialize();

            if (ModHooks.GetMod("QoL") is Mod)
            {
                Toggles.QoL = true;
                Log("Reading QoL");
            }

            if (ModHooks.GetMod("Everwatchers") is Mod)
            {
                Toggles.Everwatchers = true;
                Log("Reading Everwatchers");
            }

            if (ModHooks.GetMod("Cagney Carnation") is Mod)
            {
                Toggles.Cagney = true;
                Log("Reading Cagney Carnation");
            }

            if (ModHooks.GetMod("Pale Court") is Mod)
            {
                Toggles.PaleCourt = true;
                Log("Reading Pale Court");
            }
        }

        void Bow()
        {

        }

        void Test()
        {

        }

        private void ModHooks_HeroUpdateHook()
        {
            if (!Toggles.mod && togglerer == 0)
            {
                Log("Toggling off");
                ModHooks.OnEnableEnemyHook -= ModHooks_OnEnableEnemyHook;
                ModHooks.BeforeSceneLoadHook -= ModHooks_BeforeSceneLoadHook;
                togglerer++;
            }
            if (Toggles.mod && togglerer > 0)
            {
                Log("Toggling on");
                ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;
                ModHooks.BeforeSceneLoadHook += ModHooks_BeforeSceneLoadHook;
                togglerer = 0;
            }
            /*if (Input.GetKeyDown(KeyCode.P))
            {
                Log("Attempting to give AI");
                Test();
            }

            /*if (Input.GetKeyDown(KeyCode.I))
            {
                Log("Attempting to give AI");
                Bow();
            }
            /*if (Input.GetKeyDown(KeyCode.K))
            {
                GameObject Battle = GameObject.Find("Battle Scene v2");
                PlayMakerFSM fsmB = Battle.LocateMyFSM("Battle Control");
                Log(fsmB.FsmVariables.GetFsmInt("Battle Enemies").Value);
            }*/
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
                else if (enemy.name.Contains("Zombie Beam Miner Rematch")) { wait = 3; tp = true; }
                else wait = 0;

                foreach (string s in Lists.Tripled)
                {
                    if (enemy.name.Contains(s))
                    {
                        if (trip == 0) trip++;
                        else
                        {
                            trip = 0;
                            skip = true;
                        }
                    }
                }

                if (GameManager.instance.sceneName == "Mines_18" || GameManager.instance.sceneName == "Mines_32")
                {
                    skip = true;
                }

                if (enemy.name.Contains("Dryya2(Clone)"))
                {
                    if (dry == 0)
                    {
                        dry++;
                        skip = true;
                    }
                    else dry = 0;
                }

                foreach (string s in Lists.Exceptions)
                {
                    if (enemy.name.Contains(s)) skip = true;
                }

                if (Toggles.onlyboss)
                {
                    skip = true;
                    foreach (string s in Lists.Bosses)
                    {
                        if (enemy.name.Contains(s)) skip = false;
                    }
                }

                if (!skip)
                {
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(wait, () => Duplicate(enemy, tp));
                }
                if (enemy.name == "Hornet Boss 1")
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2, () =>
                    {
                        GameObject.Find("Hornet Boss 1(EnemyDupe)").transform.position = enemy.transform.position;
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Hornet.HornetAI());
                    });
                if (enemy.name == "Hornet Boss 2")
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () =>
                    {
                        GameObject.Find("Hornet Boss 2(EnemyDupe)").transform.position = enemy.transform.position;
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () => Hornet.HornetAI());
                    });
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
                if (enemy.name.Contains("Black Knight 1") && enemy.name.Length != 15)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.2f, () => Watchers.WatchersAI());
                if (enemy.name.Contains("Mega Zombie Beam Miner (1)") && GameManager.instance.sceneName.Contains("GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.2f, () => CG.CrystalAI());
                if (enemy.name.Contains("Zombie Beam Miner Rematch") && GameManager.instance.sceneName.Contains("GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(3.1f, () => EG.EnragedAI());
                if (enemy.name.Contains("Mushroom Brawler 1"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Ogres.OgresAI());
                if (enemy.name.Contains("White Defender"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => WD.WhiteDefAI());
                if (enemy.name.Contains("Dung Defender"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => DungD.DungDefAI());
                if (enemy.name.Contains("Mage Lord") && !enemy.name.Contains("Phase2") && !enemy.name.Contains("Dream"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => SMaster.SMasterAI());
                if (enemy.name.Contains("Dream Mage Lord") && !enemy.name.Contains("Phase2"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => STyrant.STyrantAI());
                if (enemy.name.Contains("Xero"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Xero.XeroAI());
                if (enemy.name.Contains("Pale Lurker"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => PaleLurker.LurkerAI());
                if (enemy.name.Contains("Flamebearer"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Grimmkin.GrimmkinAI(enemy.name));
                if (enemy.name.Contains("Cagney Carnation") && Toggles.Cagney == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Cagney.CagneyAI());
                if (enemy.name.Contains("Dryya2(Clone)") && Toggles.PaleCourt == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Dryya.DryyaAI());

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

            if (enemy.name.Contains("White Defender") && GameManager.instance.sceneName.Contains("GG"))
            {
                New.LocateMyFSM("Dung Defender").RemoveAction("Init 2", 0);
                Modding.Logger.Log("DOING");
            }

            HPShare.DoubleHP(enemy, New);

            Offset.EnemyOffset(enemy, New);
        }

            /*
             *  Sync Boss Phases to doubled hp
             *  Spawns that are alraedy duped
             *  Some enemies can't be duped
             *  Mimics get deleted 
             *  Arenas
             *  Soul warrior in sanctum when coming from the top has no ai
             *  
             *  OW Nosk
             *  OW CG and EG 
             *  Idk why ascended warrior sometimes tps oob
             *  Fix OW Galien Scythe
             *  OW Uumuu
             *  
            */


        }
    }