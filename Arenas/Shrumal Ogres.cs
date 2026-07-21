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
        public static void Ogres()
        {
            GameObject Battle = GameObject.Find("Battle Scene v2");
            PlayMakerFSM fsmB = Battle.LocateMyFSM("Battle Control");

            GameObject Shroom1 = GameObject.Find("Mushroom Brawler 1");
            PlayMakerFSM fsmS1 = Shroom1.LocateMyFSM("Shroom Brawler");
            GameObject Shroom1D = GameObject.Find("Mushroom Brawler 1(EnemyDupe)");
            PlayMakerFSM fsmS1D = Shroom1D.LocateMyFSM("Shroom Brawler");

            fsmS1.Fsm.GetState("Wake").Actions[4] = new CustomFsmAction()
            {
                method = () => {
                    fsmS1D.SendEvent("WAKE");
                }
            };

            GameObject Shroom2 = GameObject.Find("Mushroom Brawler 2");
            PlayMakerFSM fsmS2 = Shroom2.LocateMyFSM("Shroom Brawler");
            GameObject Shroom2D = GameObject.Find("Mushroom Brawler 2(EnemyDupe)");
            PlayMakerFSM fsmS2D = Shroom2D.LocateMyFSM("Shroom Brawler");

            fsmS2.Fsm.GetState("Wake").Actions[4] = new CustomFsmAction()
            {
                method = () => {
                    fsmS2D.SendEvent("WAKE");
                    fsmB.FsmVariables.GetFsmInt("Battle Enemies").Value += 2;
                }
            };
        }
    }
}