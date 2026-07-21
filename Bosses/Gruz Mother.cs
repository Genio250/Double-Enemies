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
using static Modding.Logger;

namespace DoubleEnemies
{
    public static partial class Bosses
    {
        public static void GruzAI()
        {
            GameObject Mom = GameObject.Find("Giant Fly(EnemyDupe)");
            Mom.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            PlayMakerFSM MomFly = Mom.LocateMyFSM("bouncer_control");

            Log(MomFly.ActiveStateName);
            MomFly.SendEvent("STOP");
            Log(MomFly.ActiveStateName);

            Mom.transform.position = GameObject.Find("Giant Fly").transform.position - new Vector3(0, 0, 0.1f);

            GameObject Battle = GameObject.Find("Battle Scene");
            PlayMakerFSM fsm = Battle.LocateMyFSM("Battle Control");
            fsm.InsertCustomAction("Start", () =>
            {
                fsm.FsmVariables.GetFsmInt("Battle Enemies").Value *= 4;
            }, 1);

            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook1;
        }

        private static void ModHooks_HeroUpdateHook1()
        {
            GameObject Burster = GameObject.Find("Corpse Big Fly Burster(Clone)");
            if (Burster != null)
            {
                Log("BLOWN UP");
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () =>
                {
                    Burster.name += "(EnemyDupe)";
                    Log(Burster);

                    GameObject Spawner1 = GameObject.Find("Fly Spawn");

                    GameObject Spawner2 = GameObject.Instantiate(Spawner1);
                    Spawner2.name = "Fly Spawn(EnemyDupe)";

                    GameObject Spawner3 = GameObject.Instantiate(Spawner1);
                    Spawner3.name = "Fly Spawn 2";

                    GameObject Spawner4 = GameObject.Instantiate(Spawner1);
                    Spawner4.name = "Fly Spawn 2(EnemyDupe)";


                    PlayMakerFSM Burst = Burster.LocateMyFSM("burster");
                    Burst.FsmVariables.GetFsmGameObject("Fly Spawn").Value = Spawner2;

                    Burst.CopyState("Spawn Flies 2", "Spawn Flies 3");
                    Burst.CopyState("Spawn Flies 2", "Spawn Flies 4");
                    Burst.AddTransition("Spawn Flies 2", "FLIES", "Spawn Flies 3");
                    Burst.AddCustomAction("Spawn Flies 2", () => Burst.SendEvent("FLIES"));
                    Burst.InsertCustomAction("Spawn Flies 3", () =>
                    {
                        Burst.FsmVariables.GetFsmGameObject("Fly Spawn").Value = Spawner3;
                    }, 0);

                    Burst.AddTransition("Spawn Flies 3", "FLIES", "Spawn Flies 4");
                    Burst.AddCustomAction("Spawn Flies 3", () => Burst.SendEvent("FLIES"));
                    Burst.InsertCustomAction("Spawn Flies 4", () =>
                    {
                        Burst.FsmVariables.GetFsmGameObject("Fly Spawn").Value = Spawner4;
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () =>
                        {

                            UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
                            foreach (GameObject obj in array)
                            {
                                if (obj.name == "Fly(EnemyDupe)")
                                {
                                    Log("Destroying" + obj.name);
                                    GameObject.Destroy(obj);
                                }
                                else if (obj.name.Contains("Fly") && obj.transform.position.y < 14.55f)
                                {
                                    Log("Teleporting " + obj.name);
                                    obj.transform.position = new Vector3(obj.transform.position.x, 14.55f, 0);
                                } //86.6, 25.5, 102.5

                                else if (obj.name.Contains("Fly") && obj.transform.position.y > 25.5f)
                                {
                                    Log("Teleporting " + obj.name);
                                    obj.transform.position = new Vector3(obj.transform.position.x, 25.5f, 0);
                                } //86.6, 25.5, 102.5

                                else if (obj.name.Contains("Fly") && obj.transform.position.x < 86.6f)
                                {
                                    Log("Teleporting " + obj.name);
                                    obj.transform.position = new Vector3(86.6f, obj.transform.position.y, 0);
                                } //86.6, 25.5, 102.5

                                else if (obj.name.Contains("Fly") && obj.transform.position.x > 102.5f)
                                {
                                    Log("Teleporting " + obj.name);
                                    obj.transform.position = new Vector3(102.5f, obj.transform.position.y, 0);
                                } //86.6, 25.5, 102.5
                            }
                            ;
                        });
                    }, 0);


                });
                ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook1;
            }
        }

    }
}