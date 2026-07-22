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
        public static void Colo2()
        {
            PlayMakerFSM fsm = GameObject.Find("Colosseum Manager").LocateMyFSM("Battle Control");
            Log(fsm);
            if (!Toggles.colo)
            {
                foreach (var state in fsm.FsmStates)
                {
                    if (state.Name.Contains("Wave"))
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

            fsm.GetFirstActionOfType<IntCompare>("Wave 30 Obble").integer2.Value *= 2;

            fsm.AddCustomAction("Wave 20", () =>
            {
                ModHooks.HeroUpdateHook += Colo2MimicFinder;
            });

            fsm.AddCustomAction("Wave 30 Obble", () =>
            {
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2.3f, () =>
                {
                    GameObject vfk = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Mega Fat Bee");
                    GameObject vfk2 = GameObject.Instantiate(vfk);
                    vfk2.SetActive(true);
                    vfk2.name = "Mega Fat Bee(EnemyDupe)";
                    vfk2.transform.position = vfk.transform.position - new Vector3(0, 5, 0);

                    HPShare.DoubleHP(vfk, vfk2);
                    Offset.EnemyOffset(vfk, vfk2);

                    GameObject Bee12 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Mega Fat Bee (1)");
                    GameObject Bee22 = GameObject.Instantiate(Bee12);
                    Bee22.SetActive(true);
                    Bee22.name = "Mega Fat Bee (1)(EnemyDupe)";
                    Bee22.transform.position = Bee12.transform.position - new Vector3(0, 5, 0);

                    HPShare.DoubleHP(Bee12, Bee22);
                    Offset.EnemyOffset(Bee12, Bee22);
                });
            });

        }

        private static void Colo2MimicFinder()
        {
            if(GameObject.Find("Grub Mimic Bottle Col(Clone)")  != null)
            {
                Log("Duping Mimic");
                GameObject mimic = GameObject.Instantiate(GameObject.Find("Grub Mimic Bottle Col(Clone)"));
                mimic.name += "(EnemyDupe)";
                ModHooks.HeroUpdateHook -= Colo2MimicFinder;
            }
        }
    }
}