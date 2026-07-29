using Satchel;
using UnityEngine;

namespace DoubleEnemies
{
    public static partial class Bosses
    {
        public static void FlukeAI(GameObject enemy)
        {
            PlayMakerFSM fsm = enemy.LocateMyFSM("Fluke Mother");
            if (!Toggles.onlyboss)
            {
                fsm.CopyState("Spawn 2", "Spawn 4");
                fsm.CopyState("Spawn", "Spawn 3");
                fsm.ChangeTransition("Spawn 2", "FINISHED", "Spawn 4");
                fsm.ChangeTransition("Spawn", "FINISHED", "Spawn 3");
            }

            GameObject copy = GameObject.Find(enemy.name + "(EnemyDupe)");
            PlayMakerFSM fsm2 = copy.LocateMyFSM("Fluke Mother");
            if (!Toggles.onlyboss)
            {
                fsm2.CopyState("Spawn 2", "Spawn 4");
                fsm2.CopyState("Spawn", "Spawn 3");
                fsm2.ChangeTransition("Spawn 2", "FINISHED", "Spawn 4");
                fsm2.ChangeTransition("Spawn", "FINISHED", "Spawn 3");
            }

            UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in array)
            {
                if (obj.name.Contains("Fluke Fly Spawner") && obj.name.Contains("(EnemyDupe)"))
                {
                    obj.transform.parent = GameObject.Find("Hatcher Cage (2)").transform;
                    Log(obj.transform.parent);
                }
            }

            GameObject Cage = GameObject.Instantiate(GameObject.Find("Hatcher Cage (2)"));
            Cage.name = "Hatcher Cage (2)(EnemyDupe)";
            fsm2.FsmVariables.GetFsmGameObject("Cage").Value = Cage;

            Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(1, () =>
            {
                UnityEngine.Object[] array2 = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject obj in array2)
                {
                    if (obj.name.Contains("Fluke Fly Spawner") && obj.transform.parent == null)
                    {
                        Log("Destroying " + obj.name);
                        GameObject.Destroy(obj);
                    }
                }
            });
        }
    }
}