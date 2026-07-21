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
        public static void EnragedBeamer(string s, PlayMakerFSM fsm)
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

        public static PlayMakerFSM EnragedRainer(char n)
        {
            GameObject Rain = null;
            if(n == '0') Rain = GameObject.Find("Laser Turret Mega");
            else Rain = GameObject.Find("Laser Turret Mega (" + n + ")");
            GameObject Rain2 = GameObject.Instantiate(Rain);
            Rain2.name = Rain.name + "(EnemyDupe)";
            PlayMakerFSM Rfsm = Rain2.LocateMyFSM("Laser Bug Mega");
            return Rfsm;
        }
        public static void EnragedAI()
        {
            GameObject CG = GameObject.Find("Zombie Beam Miner Rematch(EnemyDupe)");
            PlayMakerFSM fsm = CG.LocateMyFSM("Beam Miner");

            EnragedBeamer("Beam", fsm);
            EnragedBeamer("Beam Ball", fsm);
            EnragedBeamer("Beam Impact", fsm);

            PlayMakerFSM Rain1 = EnragedRainer('0');
            PlayMakerFSM Rain2 = EnragedRainer('1');
            PlayMakerFSM Rain3 = EnragedRainer('2');
            PlayMakerFSM Rain4 = EnragedRainer('3');

            fsm.Fsm.GetState("Laser Shoot").Actions[0] = new CustomFsmAction()
            {
                method = () => {
                    Rain1.SendEvent("LASER SHOOT");
                    Rain2.SendEvent("LASER SHOOT");
                    Rain3.SendEvent("LASER SHOOT");
                    Rain4.SendEvent("LASER SHOOT");
                    Modding.Logger.Log("EG Dupe Shot");
                }
            };

            ModHooks.HeroUpdateHook += EnragedDeleter;
        }

        private static void EnragedDeleter()
        {
            GameObject go = GameObject.Find("Zombie Beam Miner Rematch(EnemyDupe)");
            if (go == null)
            {
                GameObject.Destroy(GameObject.Find("Beam(EnemyDupe)"));
                GameObject.Destroy(GameObject.Find("Beam Ball(EnemyDupe)"));
                GameObject.Destroy(GameObject.Find("Beam Impact(EnemyDupe)"));
                Modding.Logger.Log("Success");
                ModHooks.HeroUpdateHook -= EnragedDeleter;
            }
        }
    }
}