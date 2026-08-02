using Modding;
using Satchel;
using UnityEngine;

namespace DoubleEnemies
{
    public static partial class Enemies
    {
        public static void Mimics()
        {
            if (Toggles.onlyboss) return;
            Log("Mimic");
            GameObject[] arr = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in arr)
            {
                if (obj.name.Contains("Grub Mimic Top"))
                {
                    Log("Grub Mimic Bottle" + obj.name.Substring(14));
                    MimicFixer("Grub Mimic Bottle" + obj.name.Substring(14), obj.name);
                }
            }
        }

        public static void MimicFixer(string Bottle, string Top)
        {
            GameObject bot = GameObject.Find(Bottle);
            GameObject top = GameObject.Find(Top);
            GameObject bot2 = null, top2 = null;
            if (bot != null)
            {
                Log("Duping " + bot.name);
                bot2 = GameObject.Instantiate(bot);
                bot2.name += "(EnemyDupe)";
            }
            if (top != null)
            {
                Log("Duping " + top.name);
                top2 = GameObject.Instantiate(top);
                top2.name += "(EnemyDupe)";

                top.transform.position -= new Vector3(0.25f, 0, 0);
                top2.transform.position += new Vector3(0.25f, 0, 0);
            }
            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () =>
            {
                if (bot != null && bot.transform.childCount != 0)
                {
                    Log("Doing ts for " + bot.name + " because it has " + bot.transform.childCount + " children");
                    PlayMakerFSM topf = top2.LocateMyFSM("Grub Control");
                    PlayMakerFSM botf = bot2.LocateMyFSM("Bottle Control");

                    botf.FsmVariables.GetFsmGameObject("Grub").Value = top2;
                    topf.SendEvent("IN BOTTLE");
                }
            });
        }

    }
}