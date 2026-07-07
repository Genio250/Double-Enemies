using HutongGames.PlayMaker.Actions;
using IL;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using On;
using QoL;
using Everwatchers;
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
        public override string GetVersion() => "1.1.2";
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
        public static int counter = 0, togglerer = 0, mager = 0, mager2 = 0, mager3 = 0;
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
        }

       /* void Filler()
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

        void Bow()
        {
            GameObject Burrow = GameObject.Find("Burrow Effect(EnemyDupe)");
            PlayMakerFSM Bfsm = Burrow.LocateMyFSM("Burrow Effect");
            Log(Bfsm.ActiveStateName);
        }*/

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
                else if (enemy.name.Contains("Hornet Boss 1")) { wait = 3; tp = true; }
                else if (enemy.name.Contains("Zombie Beam Miner Rematch")) { wait = 3; tp = true; }
                else wait = 0;

                if (enemy.name == "Dream Mage Lord Phase2")
                {
                    if (mager == 0) mager++;
                    else
                    {
                        mager = 0;
                        skip = true;
                    }
                }
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

            if (enemy.name == "Dream Mage Lord Phase2")
            {
                enemy.manageHealth(enemy.GetComponent<HealthManager>().hp / 8);
                New.manageHealth(New.GetComponent<HealthManager>().hp / 8);
            }

            if (enemy.name == "Dream Mage Lord")
            {
                enemy.manageHealth(enemy.GetComponent<HealthManager>().hp / 2);
                New.manageHealth(New.GetComponent<HealthManager>().hp / 2);
            }

            if (enemy.name == "Mage Lord Phase2")
            {
                enemy.manageHealth(enemy.GetComponent<HealthManager>().hp / 2);
                New.manageHealth(New.GetComponent<HealthManager>().hp / 2);
            }

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
             *  Soul warrior in sanctum when coming from the top has no ai
             *  
             *  Pale Lurker
             *  OW Nosk
             *  OW CG and EG 
             *  Potentially dupe Xero spawn swords
             *  Idk why ascended warrior sometimes tps oob
             *  Fix OW Galien Scythe
             *  OW Uumuu
             *  
            */


        }
    }