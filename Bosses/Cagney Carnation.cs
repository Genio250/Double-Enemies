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
    public static class Cagney
    {
        public static void CagneyAI()
        {
            GameObject cagney = GameObject.Find("Cagney Carnation(EnemyDupe)");
            PlayMakerFSM fsm = cagney.LocateMyFSM("Control");
            fsm.Fsm.GetState("Phase Check").Actions[0] = new CustomFsmAction()
            {
                method = () => fsm.Fsm.GetFsmInt("HP").Value = cagney.GetComponent<HealthManager>().hp
            };
        }
    }
}