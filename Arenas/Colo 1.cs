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
        public static void Colo1()
        {
            if (!Toggles.onlyboss) return;
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
            fsm.InsertCustomAction("Wave 15", () =>
            {
                GameObject vfk = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Giant Buzzer Col");
                GameObject vfk2 = GameObject.Instantiate(vfk);
                vfk2.SetActive(true);
                vfk2.name = "Giant Buzzer Col(EnemyDupe)";
                vfk2.transform.position = vfk.transform.position;

                HPShare.DoubleHP(vfk, vfk2);
                Offset.EnemyOffset(vfk, vfk2);
            }, 3);
        }
    }
}