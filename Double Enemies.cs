using HutongGames.PlayMaker.Actions;
using IL;
using IL.HutongGames.PlayMaker.Actions;
using InControl;
using Modding;
using On.HutongGames.PlayMaker.Actions;
using Satchel;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Mono.Security.X509.X520;
using QoL;
using MonoMod.RuntimeDetour;

namespace DoubleEnemies
{
    public static class Toggles
    {
        public static bool mod = true, PV = false, Nosk = false; public static int amount;
    }
    public class DoubleEnemies : Mod, IMenuMod
    {
        public DoubleEnemies() : base("Double Enemies") { }
        public override string GetVersion() => "0.1.1";
        public bool ToggleButtonInsideMenu => false;

        public List<string> Bosses = ["Giant Fly", "Giant Buzzer Col", "Giant Buzzer Col (1)", "False Knight New", "False Knight Dream",
            "Hornet Boss 1", "Mawlek Body", "Hornet Boss 2", "Fluke Mother", "Mantis Lord", "Mega Fat Bee", "Mega Fat Bee (1)",
            "Infected Knight", "Lost Kin", "Mimic Spider", "Jar Collector", "Lancer", "Mantis Traitor Lord", "Grey Prince", "Nightmare Grimm Boss",
            "Grimm Boss", "HK Prime", "Sheo Boss", "Ghost Warrior Hu", "Ghost Warrior Slug", "Ghost Warrior Galien", "Ghost Warrior Markoth",
            "Ghost Warrior Xero", "Ghost Warrior Marmu", "Ghost Warrior No Eyes", "Dung Defender", "White Defender", "Dream Mage Lord",
            "Dream Mage Lord Phase2", "Mage Lord", "Mage Lord Phase2", "Mage Knight", "Hollow Knight Boss", "Mega Jellyfish"];

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry
                {
                    /*Name = "Mod toggle",
                    Description = null,
                    Values = new string[] { "Off", "On" },
                    Saver = (i) => Toggles.mod = i == 1,
                    Loader = () => Toggles.mod ? 1 : 0*/
                }
            };
        }
        public int counter = 0;
        public override void Initialize()
        {
            Log("Initializing");
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;
            if (ModHooks.GetMod("QoL") is Mod)
            {
                QoLcheck.BossFix();
                Log("Reading QoL");
            }
            On.HealthManager.TakeDamage += HealthManager_TakeDamage;
        }

        private void HealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            //Log("Started Hit " + counter);
            GameObject Boss = null, Boss2 = null;
            bool commit = false;
            foreach(string s in Bosses) 
            {
                GameObject go = GameObject.Find(s);
                GameObject go2 = GameObject.Find(s + "(EnemyDupe)");

                if (go != null)
                {
                    if (self == go.GetComponent<HealthManager>())
                    {
                        Boss = go;
                        Boss2 = go2;
                        commit = true;
                        break;
                    }
                    else if (self == go2.GetComponent<HealthManager>())
                    {
                        Boss = go2;
                        Boss2 = go;
                        commit = true;
                        break;
                    }
                }
            }
            //Log("Got the target for hit " + counter);
            if (commit && Boss2!=null)
            {
                HealthManager copy = Boss2.GetComponent<HealthManager>();  
                if (!Boss2.name.Contains("Mega Fat Bee"))
                {
                    orig(self, hitInstance);
                    if (copy.hp - hitInstance.DamageDealt > 0) copy.hp -= hitInstance.DamageDealt;
                    else orig(copy, hitInstance);
                }
                else
                {
                    if (copy.hp - hitInstance.DamageDealt > 0)
                    {
                        orig(self, hitInstance);
                        copy.hp -= hitInstance.DamageDealt;
                    }
                    else
                    {
                        orig(self, hitInstance);
                        copy.hp -= 200;
                        orig(copy, hitInstance);
                    }
                }

                
            }
            else orig(self, hitInstance);
            //Log("Finished hit " + counter);
            counter++;
        }

        private GameObject HatchlingPrefab => field ??=
            ((HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool)HeroController.instance.transform.Find("Charm Effects").
            gameObject.LocateMyFSM("Hatchling Spawn").Fsm.GetState("Hatch").Actions[2]).gameObject.Value;

        private bool ModHooks_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            if (!isAlreadyDead && !enemy.name.Contains("(EnemyDupe)") && Toggles.mod)
            {
                bool tp = false;
                float wait = 0;
                if (enemy.name.Contains("Mega Fat Bee")) wait = 2;
                else if (enemy.name.Contains("Sheo")) { wait = 0.1f; tp = true; }
                else if (enemy.name.Contains("Nightmare Grimm Boss")) wait = 7;
                //else if (enemy.name.Contains("Mega Jellyfish GG")) wait = 0;
                else if (enemy.name.Contains("Ghost Warrior Markoth")) wait = 1;
                else if (enemy.name.Contains("Mawlek Body")) { wait = 0.1f; tp = true; }
                //else if (enemy.name.Contains("Lobster")) wait = 5;
                else if (enemy.name.Contains("Mage Balloon Spawner")) wait = 1;
                else if (enemy.name.Contains("Jar Collector")) wait = 0.1f;
                else if (enemy.name.Contains("Grimm Boss")) { wait = 0.1f; tp = true; }
                //else if (enemy.name.Contains("Mega Moss Charger")) wait = 8;
                else if (enemy.name.Contains("Giant Fly")) wait = 0.5f;
                else if (enemy.name.Contains("False Knight New")) { wait = 0.3f; tp = true; }
                else if (enemy.name.Contains("Hornet Boss 1")) { wait = 3; tp = true; }
                else if (enemy.name.Contains("Giant Buzzer Col") && GameManager.instance.sceneName == "Room_Colosseum_Bronze") { wait = 10; Log("VFK"); }
                // else if (enemy.name.Contains("Zombie Beam Miner Rematch")) { wait = 3; tp = true; }
                else wait = 0; // && !enemy.name.Contains("Mage Lord Phase2")
                if (!enemy.name.Contains("Radiance") && !(enemy.name == "Head") && !(enemy.name == "Lobster")
                    && !(enemy.name == "Mega Jellyfish GG") && !(enemy.name == "Mega Moss Charger")
                    && !enemy.name.Contains("Zombie Beam Miner") && !(enemy.name == "Sly Boss"))
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(wait, () => Duplicate(enemy, tp));
                if (enemy.name.Contains("Galien"))
                {
                    GameObject scythe = GameObject.Find("Galien Hammer");
                    Duplicate(scythe, false);
                    scythe.transform.position += new Vector3(10, 0, 0);
                }
            }
            foreach (string s in Bosses)
            {
                if (enemy.name == s && s != "Mega Jellyfish") enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);
            }
            if (enemy.name == "Grey Prince(EnemyDupe)" || enemy.name == "Grimm Boss(EnemyDupe)")
                enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);
            return isAlreadyDead; //Zote
        }

        private void Duplicate(GameObject enemy, bool tp)
        {
            GameObject New = UnityEngine.GameObject.Instantiate(enemy);
            New.name = enemy.name + "(EnemyDupe)";
            if(tp) New.transform.position = enemy.transform.position;
            Log("Duping " + enemy.name);

            //Take the actions to fix NKG
            if (enemy.name.Contains("Nightmare Grimm Boss"))
            {
                HealthManager NkgHp = enemy.GetComponent<HealthManager>();
                int a = NkgHp.hp;
                GameObject RealBat = GameObject.Find("Real Bat");
                GameObject RealBatDupe = GameObject.Find("Real Bat(EnemyDupe)");
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () => StaggerNKG());
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2, () => { New.manageHealth(a); enemy.manageHealth(a); });
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2.5f, () =>
                { 
                    RealBat.transform.position = new Vector3(1000, 1000, 0);
                    RealBatDupe.transform.position = new Vector3(1000, 1000, 0);
                    Log("TP Real Bat");
                });
            }

            //Random offset to Collector and VFK vengeflies and Warrior Follies
            Vector2 spawn2 = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 spawn3 = new Vector3(spawn2.x, spawn2.y, 0);
            if (enemy.name == "Jellyfish GG(Clone)")
                New.transform.position = enemy.transform.position + spawn3 * 3; 
            if (enemy.name.Contains("Buzzer") && !enemy.name.Contains("Giant"))
            {
                if (GameManager.instance.sceneName == "GG_Collector" || GameManager.instance.sceneName == "Ruins2_11")
                    spawn3 = new Vector3(Math.Abs(spawn2.x), Math.Abs(spawn2.y), 0);
                New.transform.position = enemy.transform.position + spawn3 * 2;
            }
            if (enemy.name.Contains("Mage Balloon Spawner"))
                New.transform.position = enemy.transform.position + spawn3;

            //Offsets 1d enemies in their axis
            if (enemy.name.Contains("Plant Turret") || enemy.name.Contains("Plant Trap") || enemy.name.Contains("Acid Walker"))
                if ((enemy.transform.rotation.eulerAngles.z < 100 || enemy.transform.rotation.eulerAngles.z > 200) && enemy.transform.rotation.eulerAngles.z > 10)
                {
                    New.transform.position = enemy.transform.position + new Vector3(0, 1, 0);
                }
                else New.transform.position = enemy.transform.position + new Vector3(1, 0, 0);
        }
        private void StaggerNKG()
        {
            for (int i = 12; i > 0; i--)
            {
                GameObject hatchling = UnityEngine.Object.Instantiate(HatchlingPrefab);
                hatchling.transform.position = new Vector3(67, 30, 0);
            }
        }


        /* private void Stagger()
         {
             GameObject Hatchling = new GameObject(
         }*/

        /*
         *  Sync Boss Phases to doubled hp
         *  Spawns that are alraedy duped
         *  Shadow creepers add to 1d enemies
         *  Some enemies can't be duped
         *  Doesn't work: Enraged, WK, CG, Mossie, Lobster, Nosket
         *  Oro and Mato phase 2
         *  3 Mantises???? Into normal SoB
         *  Mawlek throws an error
         *  Ow Dream warriors don't have extra hp
         *  Mimics get deleted  
         *  Spawn an ow uumuu in gh
         *  
         *  FSM and IL Hooks:
         *  QoL PV and OW Nosk
         *  Potentially dupe Xero spawn swords
         *  Improve NKG
         *  Idk why ascended warrior sometimes tps oob
         *  Give Galien Scythe an animation
         *  
         *  Hard:
         *  Hive Knight cannot be duped
         *  Uumuu and Wosk don't have ai
         *  Sly breaks
         *  WD underground follows the main
         *  Tyrant Orbs follows the main
         *  
        */
    }

    static class QoLcheck
    {
        public static void BossFix()
        {
            Toggles.PV = true;
            Toggles.Nosk = true;
            QoL.Modules.SkipCutscenes.PureVesselRoar = false;
            
        }
        /*private static IEnumerator HKPrimeSkip(Scene arg1)
        {
            if (arg1.name != "GG_Hollow_Knight") yield break;

            yield return null;

            PlayMakerFSM control = GameObject.Find("HK Prime(EnemyDupe)").LocateMyFSM("Control");

            control.Fsm.GetState("Init").ChangeTransition("FINISHED", "Intro Roar");
            control.GetAction<HutongGames.PlayMaker.Actions.Wait>("Intro 2", 1).time = 0.01f;
            control.GetAction<HutongGames.PlayMaker.Actions.Wait>("Intro 1", 1).time = 0.01f;
            control.GetAction<HutongGames.PlayMaker.Actions.Wait>("Intro Roar", 1).time = 1f;
        }*/

    }
}