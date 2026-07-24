using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using Modding;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoubleEnemies
{
    public static partial class Arenas
    {
        public static void Colo3()
        {
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
            PlayMakerFSM fsm = GameObject.Find("Colosseum Manager").LocateMyFSM("Battle Control");
            Log(fsm);
            if (!Toggles.colo)
            {
                foreach (var state in fsm.FsmStates)
                {
                    if (state.Name.Contains("Wave") && !state.Name.Contains("23"))
                    {
                        Log(state.Name);
                        if (fsm.GetFirstActionOfType<SetIntValue>(state.Name) != null)
                        {
                            fsm.GetFirstActionOfType<SetIntValue>(state.Name).intValue.Value *= 2;
                        }
                        else if (fsm.GetFirstActionOfType<IntAdd>(state.Name) != null)
                        {
                            fsm.GetFirstActionOfType<IntAdd>(state.Name).add.Value *= 2;
                        }
                        fsm.InsertCustomAction(state.Name, () => Log(state.Name), 0);
                    }
                }
            }
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () =>
            {
                List<GameObject> SoulTwisters = new List<GameObject>();
                List<GameObject> SoulWarriors = new List<GameObject>();
                GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (var obj in array)
                {
                    if (obj.name.Contains("(EnemyDupe)"))
                    {
                        Log(obj.name);
                        if (obj.name.Contains("Mage") && !obj.name.Contains("Knight"))
                        {
                            SoulTwisters.Add(obj);
                        }
                        if (obj.name.Contains("Knight"))
                        {
                            SoulWarriors.Add(obj);
                        }
                    }
                }

                fsm.InsertCustomAction("Wave 22", () =>
                {
                    Log("Bringing " + SoulTwisters[0].name);
                    SoulTwisters[0].LocateMyFSM("Mage").SendEvent("WAKE");
                }, 2);

                fsm.InsertCustomAction("Wave 22", () =>
                {
                    Log("Bringing " + SoulTwisters[1].name);
                    SoulTwisters[1].LocateMyFSM("Mage").SendEvent("WAKE");
                }, 3);

                fsm.InsertCustomAction("Wave 24", () =>
                {
                    Log("Bringing " + SoulWarriors[0].name);
                    SoulWarriors[0].LocateMyFSM("Mage Knight").SendEvent("BATTLE START");
                    GameObject ActiveKnight = GameObject.Find("Wave 24/Mage Knight");
                    Log(ActiveKnight);
                    ActiveKnight.name = "Mage Knight Col";
                    SoulWarriors[0].name = "Mage Knight Col(EnemyDupe)";

                    HPShare.DoubleHP(ActiveKnight, SoulWarriors[0]);
                }, 3);

                fsm.InsertCustomAction("Wave 26", () =>
                {
                    Log("Bringing " + SoulTwisters[2].name);
                    SoulTwisters[2].LocateMyFSM("Mage").SendEvent("WAKE");
                }, 3);

                fsm.AddCustomAction("Wave 28", () =>
                {
                    Log("Bringing " + SoulTwisters[3].name);
                    SoulTwisters[3].LocateMyFSM("Mage").SendEvent("WAKE");
                });

                fsm.AddCustomAction("Wave 29", () =>
                {
                    Log("Bringing " + SoulTwisters[4].name);
                    SoulTwisters[4].LocateMyFSM("Mage").SendEvent("WAKE");

                    Log("Bringing " + SoulWarriors[1].name);
                    SoulWarriors[1].LocateMyFSM("Mage Knight").SendEvent("BATTLE START");
                });

                fsm.AddCustomAction("Wave 46", () =>
                {
                    Log("Bringing " + SoulTwisters[5].name);
                    SoulTwisters[5].LocateMyFSM("Mage").SendEvent("WAKE");
                });

                fsm.AddCustomAction("Lancer Battle", () =>
                {
                    Log("Lancer battle");
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(4, () =>
                    {
                        GameObject Lob = GameObject.Find("Lobster");
                        GameObject dup = GameObject.Find("Lobster(EnemyDupe)");

                        dup.transform.position = Lob.transform.position;
                        Bosses.LobsterAI();
                    });
                });
            });


        }

        private static void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyUp(KeyCode.Y))
            {
                PlayMakerFSM fsm = GameObject.Find("Colosseum Manager").LocateMyFSM("Battle Control");
                fsm.SetState("Lancer Pause");
            }
        }
    }
}