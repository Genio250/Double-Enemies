global using static Modding.Logger;
using CagneyCarnation;
using Everwatchers;
using FiveKnights;
using FiveKnights.Dryya;
using FiveKnights.Hegemol;
using FiveKnights.Tiso;
using FiveKnights.Zemer;
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
using static UnityEngine.ParticleSystem;

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
                    Name = "Crazy Colo",
                    Description = "Disable the fixed wave emission and have a billion enemies at once!",
                    Values = new string[] { "Off", "On" },
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
        public static bool Mantis = true, GrimmChild = true;
        public static GameObject MantisLord = null;

        public override void Initialize()
        {
            Log("Initializing");
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;
            ModHooks.BeforeSceneLoadHook += ModHooks_BeforeSceneLoadHook;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            //Bosses.Initialize();
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
            Log(PlayerData.instance.GetInt("flamesCollected"));
            Log(PlayerData.instance.GetInt("killsFlameBearerSmall"));
            Log(PlayerData.instance.GetInt("flamesRequired"));
        }

        void Test()
        {
            Log(PlayerData.instance.GetInt("flamesCollected"));
            PlayerData.instance.SetInt("flamesCollected", PlayerData.instance.GetInt("flamesCollected") + 1);
            Log(PlayerData.instance.GetInt("flamesCollected"));
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Log("Attempting to give AI");
                Test();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                Log("Attempting to give AI");
                Bow();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Log("Bleh");

            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                Log(GameObject.Find("Grimm Scene").LocateMyFSM("Initial Scene").ActiveStateName);
            }
            /*if (Input.GetKeyDown(KeyCode.K))
            {
                GameObject Battle = GameObject.Find("Battle Scene v2");
                PlayMakerFSM fsmB = Battle.LocateMyFSM("Battle Control");
                Log(fsmB.FsmVariables.GetFsmInt("Battle Enemies").Value);
            }*/
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
            GameObject GrimmUI = GameObject.Find("Grimm Flame UI");
            if (GrimmUI != null && GrimmChild)
            {
                GrimmUI.LocateMyFSM("Control").AddCustomAction("Set 1", () =>
                {
                    if (PlayerData.instance.GetInt("flamesCollected") > 3)
                    {
                        GrimmUI.LocateMyFSM("Control").SendEvent("3");
                    }
                    GrimmChild = false;
                });
            }
            else if (GrimmUI == null && !GrimmChild) GrimmChild = true;
        }

        public void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            GameManager.instance.StartCoroutine(RoomFixes(arg1));

            IEnumerator RoomFixes(Scene arg)
            {
                yield return new WaitForFinishedEnteringScene();
                Log(arg.name);
                string s = arg.name;
                if (s == "Crossroads_08")
                {
                    Arenas.Aspids();
                }
                else if (s == "Crossroads_04")
                {

                }
                else if (s == "GG_Ghost_Galien" || s == "Deepnest_40")
                {
                    Bosses.GalienScythe();
                }
                else if (s == "Room_Colosseum_Bronze")
                {
                    Arenas.Colo1();
                }
                else if (s == "Room_Colosseum_Silver")
                {
                    Arenas.Colo2();
                }
                else if (s == "Grimm_Main_Tent")
                {
                    PlayMakerFSM Tent = GameObject.Find("Grimm Scene").LocateMyFSM("Initial Scene");
                    Tent.GetFirstActionOfType<IntCompare>("Check").greaterThan = null;
                    Tent.SetState("Init");
                    Log(Tent.ActiveStateName);

                    Tent.GetValidState("Level Up To 2").Actions[8] = new CustomFsmAction
                    {
                        method = () =>
                        {
                            PlayerData.instance.SetInt("flamesCollected", PlayerData.instance.GetInt("flamesCollected") - 3);
                        }
                    };
                }
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

                if (enemy.name.Contains("Giant Buzzer Col") && GameManager.instance.sceneName == "Room_Colosseum_Bronze") skip = true;
                if (GameManager.instance.sceneName == "Mines_18" || GameManager.instance.sceneName == "Mines_32") skip = true;

                if (GameManager.instance.sceneName == "Fungus3_39" && enemy.name.Contains("Acid Walker"))
                {
                    skip = true;
                    GameObject.Destroy(enemy);
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
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.HornetAI());
                    });
                if (enemy.name == "Hornet Boss 2")
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () =>
                    {
                        GameObject.Find("Hornet Boss 2(EnemyDupe)").transform.position = enemy.transform.position;
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () => Bosses.HornetAI());
                    });
                if (enemy.name == "Giant Fly" && GameManager.instance.sceneName == "Crossroads_04")
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.GruzAI());
                if (enemy.name == "Oro")
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.OroYMatoAI());
                if (enemy.name.Contains("Galien"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.GalienMinis());
                if (enemy.name.Contains("Mega Moss Charger"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.MossyAI());
                if (enemy.name.Contains("Lobster"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(5.1f, () => Bosses.LobsterAI());
                if (enemy.name.Contains("Hornet Nosk"))
                    Bosses.NosketAI();
                if (enemy.name.Contains("Mimic Spider"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.NoskAI());
                if (enemy.name.Contains("Mega Jellyfish GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Bosses.UumuuAI());
                if (enemy.name.Contains("Hive Knight"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Bosses.HiveKnightAI());
                if (enemy.name.Contains("Sly Boss"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Bosses.SlyAI());
                if (enemy.name.Contains("Nightmare Grimm Boss"))
                    Bosses.NkgAI();
                if (enemy.name.Contains("HK Prime"))
                    Bosses.PVSkip();
                if (enemy.name.Contains("Black Knight 1") && enemy.name.Length != 15)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.2f, () => Bosses.WatchersAI());
                if (enemy.name.Contains("Mega Zombie Beam Miner (1)") && GameManager.instance.sceneName.Contains("GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.2f, () => Bosses.CrystalAI());
                if (enemy.name.Contains("Zombie Beam Miner Rematch") && GameManager.instance.sceneName.Contains("GG"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(3.1f, () => Bosses.EnragedAI());
                if (enemy.name.Contains("White Defender"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.WhiteDefAI());
                if (enemy.name.Contains("Dung Defender"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.DungDefAI());
                if (enemy.name.Contains("Mage Lord") && !enemy.name.Contains("Phase2") && !enemy.name.Contains("Dream"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Bosses.SMasterAI());
                if (enemy.name.Contains("Dream Mage Lord") && !enemy.name.Contains("Phase2"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Bosses.STyrantAI());
                if (enemy.name.Contains("Xero"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.XeroAI());
                if (enemy.name.Contains("Fluke Mother"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Bosses.FlukeAI(enemy));
                if (enemy.name.Contains("Pale Lurker"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Bosses.LurkerAI());
                if (enemy.name.Contains("Flamebearer"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Enemies.GrimmkinAI(enemy.name));
                if (enemy.name.Contains("Cagney Carnation") && Toggles.Cagney == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Cagney.CagneyAI());
                if (enemy.name.Contains("Dryya2(Clone)") && Toggles.PaleCourt == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => Dryya.DryyaAI());
                if (enemy.name.Contains("Tiso(Clone)") && Toggles.PaleCourt == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () => Tiso.TisoAI());
                if (enemy.name.Contains("Hegemol") && Toggles.PaleCourt == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () => Hegemol.HegemolAI());
                if (enemy.name.Contains("Zemer(Clone)") && Toggles.PaleCourt == true)
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () => Zemer.ZemerAI());
                if (enemy.name.Contains("Hatcher") && !enemy.name.Contains("Baby Spawner")) Enemies.HatcherAI(enemy);
                if (enemy.name.Contains("Mushroom Brawler 1"))
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => Arenas.Ogres());


                if (enemy.name.Contains("Mantis Lord"))
                {
                    Bosses.MantisLords(Mantis, enemy);
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

            if (enemy.name.Contains("White Defender") && GameManager.instance.sceneName.Contains("GG"))
            {
                New.LocateMyFSM("Dung Defender").RemoveAction("Init 2", 0);
                Modding.Logger.Log("DOING");
            }

            if (enemy.name.Contains("Hatcher Baby Spawner"))
            {
                New.transform.parent = GameObject.Find("Hatcher Cage").transform;
                Modding.Logger.Log(New.transform.parent);
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