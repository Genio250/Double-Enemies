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
    public static partial class Bosses
    {
        public static void MossyAI()
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
        }
    }
}