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

    public class DoubleEnemies : Mod, IMenuMod
    {
        public DoubleEnemies() : base("Double Enemies") { }
        public override string GetVersion() => "0.1.1";
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
                QoLcheck.BossFix();
                Log("Reading QoL");
            }
            On.HealthManager.TakeDamage += HealthManager_TakeDamage;
            ModHooks.BeforeSceneLoadHook += ModHooks_BeforeSceneLoadHook;
        }

        private string ModHooks_BeforeSceneLoadHook(string arg)
        {
            if(arg == "Fungus1_21" && Toggles.mod)
            {
                Log("PreHornet");
                PlayerData.instance.SetInt("hornetGreenpath", 4);
            }
            return arg;
        }

        private void HealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            if (Toggles.mod)
            {
                GameObject Boss = null, Boss2 = null;
                bool commit = false;
                foreach (string s in Lists.Bosses)
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
                if (commit && Boss2 != null)
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
            else orig(self, hitInstance);
        }


        private bool ModHooks_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            if (!isAlreadyDead && !enemy.name.Contains("(EnemyDupe)") && Toggles.mod)
            {
                bool tp = false, skip = false;
                float wait = 0;
                if (enemy.name.Contains("Mega Fat Bee")) wait = 2;
                else if (enemy.name.Contains("Sheo")) { wait = 0.1f; tp = true; }
                else if (enemy.name.Contains("Nightmare Grimm Boss")) wait = 7;
                //else if (enemy.name.Contains("Mega Jellyfish GG")) wait = 0;
                else if (enemy.name.Contains("Ghost Warrior Markoth")) wait = 1;
                else if (enemy.name.Contains("Shade Sibling")) wait = 1;
                else if (enemy.name.Contains("Mawlek Body")) { wait = 0.1f; tp = true; }
                //else if (enemy.name.Contains("Lobster")) wait = 5;
                else if (enemy.name.Contains("Mage Balloon Spawner")) wait = 1;
                else if (enemy.name.Contains("Jar Collector")) wait = 0.1f;
                else if (enemy.name.Contains("Grimm Boss")) { wait = 0.1f; tp = true; }
                //else if (enemy.name.Contains("Mega Moss Charger")) wait = 8;
                else if (enemy.name.Contains("Giant Fly")) wait = 0.5f;
                else if (enemy.name.Contains("False Knight New")) { wait = 0.3f; tp = true; }
                else if (enemy.name.Contains("Hornet Boss 1")) { wait = 3; tp = true; }
                //else if (enemy.name.Contains("Giant Buzzer Col") && GameManager.instance.sceneName == "Room_Colosseum_Bronze") { wait = 10; Log("VFK"); }
                // else if (enemy.name.Contains("Zombie Beam Miner Rematch")) { wait = 3; tp = true; }
                else wait = 0;
                foreach (string s in Lists.Exceptions)
                {
                    if (enemy.name.Contains(s)) skip = true;
                }

                if(!skip) Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(wait, () => Duplicate(enemy, tp));

                if (enemy.name.Contains("Galien"))
                {
                    GameObject scythe = GameObject.Find("Galien Hammer");
                    Duplicate(scythe, false);
                    scythe.transform.position += new Vector3(10, 0, 0);
                }

                if (enemy.name.Contains("Mantis Lord"))
                {
                    MantisFix.MantisLords(Mantis, enemy);
                    //Log("Duping " + enemy.name + " fake");
                }
            }

            if (Toggles.mod)
            {
                foreach (string s in Lists.Bosses)
                {
                    if (enemy.name == s && s != "Mega Jellyfish" && !enemy.name.Contains("Mantis Lord") && !enemy.name.Contains("False Knight New"))
                        enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);
                }
                if (enemy.name == "Grey Prince(EnemyDupe)" || enemy.name == "Grimm Boss(EnemyDupe)")
                    enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);

                foreach (string s in Lists.DWarriorArenas)
                {
                    if (s == GameManager.instance.sceneName)
                    {
                        foreach (string z in Lists.DWarriors)
                        {
                            if (z + "(EnemyDupe)" == enemy.name)
                            {
                                enemy.manageHealth(2 * enemy.GetComponent<HealthManager>().hp);
                            }
                        }
                    }
                }
            }

            return isAlreadyDead; //Zote
        }

        public static void Duplicate(GameObject enemy, bool tp)
        {
            GameObject New = UnityEngine.GameObject.Instantiate(enemy);
            New.name = enemy.name + "(EnemyDupe)";
            if(tp) New.transform.position = enemy.transform.position;
            //Log("Duping " + enemy.name);

            //Take the actions to fix NKG
            if (enemy.name.Contains("Nightmare Grimm Boss"))
            {
                NKGFix.NKG(enemy, New);
            }

            //Random offset to Collector and VFK vengeflies, Uumuu Oomas, and Warrior Follies
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

            float offset = UnityEngine.Random.Range(-1f, 1f);

            //Offsets 1d enemies in their axis
            if (enemy.name.Contains("Turret") || enemy.name.Contains("Plant Trap") || enemy.name.Contains("Acid Walker") ||
                enemy.name.Contains("Abyss Crawler") || enemy.name.Contains("Ceiling Dropper"))
                //Shadow creeper
                if ((enemy.transform.rotation.eulerAngles.z < 100 || enemy.transform.rotation.eulerAngles.z > 200) &&
                    (enemy.transform.rotation.eulerAngles.z > 10 && enemy.transform.rotation.eulerAngles.z < 300))
                {
                    New.transform.position = enemy.transform.position + new Vector3(0, offset, 0);
                }
                else New.transform.position = enemy.transform.position + new Vector3(offset, 0, 0);
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
    }
}