using Satchel;
using Satchel.Futils;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace DoubleEnemies
{
    public static class Misc
    {
        public static bool GrimmChild = true;
        public static void UIFlame()
        {
            GameObject GrimmUI = GameObject.Find("Grimm Flame UI");
            if (GrimmUI != null && GrimmChild)
            {
                GrimmUI.LocateMyFSM("Control").AddCustomAction("Set 1", () =>
                {
                    if (PlayerData.instance.GetInt("flamesCollected") > 3)
                    {
                        GrimmUI.LocateMyFSM("Control").SendEvent("3");
                    }
                    GrimmChild = false;
                });
            }
            else if (GrimmUI == null && !GrimmChild) GrimmChild = true;
        }

        public static void FlamePickupFix()
        {
            PlayMakerFSM Tent = GameObject.Find("Grimm Scene").LocateMyFSM("Initial Scene");
            Tent.GetFirstActionOfType<IntCompare>("Check").greaterThan = null;
            Tent.SetState("Init");
            Log(Tent.ActiveStateName);

            Tent.GetValidState("Level Up To 2").Actions[8] = new CustomFsmAction
            {
                method = () =>
                {
                    PlayerData.instance.SetInt("flamesCollected", PlayerData.instance.GetInt("flamesCollected") - 3);
                }
            };
        }

        public static void MawlekShard()
        {
            PlayMakerFSM fsm = GameObject.Find("Battle Scene").LocateMyFSM("Battle Control");
            fsm.InsertCustomAction("End Wait", () =>
            {
                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () =>
                {
                    GameObject mask = GameObject.Instantiate(GameObject.Find("Heart Piece"));
                    mask.name += "(EnemyDupe)";
                });
            }, 2);
        }

        public static void LifeBlood()
        {
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () =>
            {
                GameObject[] Cocoons = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject coco in Cocoons)
                {
                    if (coco.name.Contains("Health Cocoon"))
                    {
                        Log("Duping " + coco.name);
                        GameObject coco2 = GameObject.Instantiate(coco);
                        coco2.name += "Health Cocoon";
                    }
                }
            });
        }

        public static string HornetDialogueRemoval(string arg)
        {
            if (arg == "Fungus1_21")
            {
                Log("PreHornet");
                PlayerData.instance.SetInt("hornetGreenpath", 4);
            }
            return arg;
        }

    }
}