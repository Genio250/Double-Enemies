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
    public static class CG
    {
        public static void Beamer(string s, PlayMakerFSM fsm)
        {
            GameObject Beam = GameObject.Find(s);
            GameObject Beam2 = GameObject.Instantiate(Beam);
            Beam2.SetActive(false);
            Beam2.name = s + "(EnemyDupe)";
            Beam2.LocateMyFSM("destroy_if_gameobject_null").RemoveState("Check");
            Beam2.LocateMyFSM("destroy_if_gameobject_null").RemoveState("Destroy");
            Beam2.SetActive(true);

            fsm.FsmVariables.FindFsmGameObject(s).Value = Beam2;
        }

        public static PlayMakerFSM Rainer(char n)
        {
            GameObject Rain = GameObject.Find("Laser Turret Mega (" + n + ")");
            GameObject Rain2 = GameObject.Instantiate(Rain);
            Rain2.name = "Laser Turret Mega (" + n + ")(EnemyDupe)";
            PlayMakerFSM Rfsm = Rain2.LocateMyFSM("Laser Bug Mega");
            return Rfsm;
        }
        public static void CrystalAI()
        {
            GameObject CG = GameObject.Find("Mega Zombie Beam Miner (1)(EnemyDupe)");
            PlayMakerFSM fsm = CG.LocateMyFSM("Beam Miner");
            PlayMakerFSM origfsm = GameObject.Find("Mega Zombie Beam Miner (1)").LocateMyFSM("Beam Miner");

            fsm.Fsm.GetState("Roar End").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    GameObject Roar = GameObject.Find("Mega Zombie Beam Miner (1)(EnemyDupe)/Roar Wave Emitter(Clone)");
                    PlayMakerFSM Rfsm = Roar.LocateMyFSM("emitter");
                    Rfsm.SendEvent("END");
                }
            };

            Beamer("Beam", fsm);
            Beamer("Beam Ball", fsm);
            Beamer("Beam Impact", fsm);

            PlayMakerFSM Rain1 = Rainer('1');
            PlayMakerFSM Rain2 = Rainer('2');
            PlayMakerFSM Rain3 = Rainer('3');
            PlayMakerFSM Rain4 = Rainer('4');

            fsm.Fsm.GetState("Lasers").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    Rain1.SendEvent("LASER SHOOT");
                    Rain2.SendEvent("LASER SHOOT");
                    Rain3.SendEvent("LASER SHOOT");
                    Rain4.SendEvent("LASER SHOOT");
                    Modding.Logger.Log("CG Dupe Shot");
                }
            };
        }
    }
}