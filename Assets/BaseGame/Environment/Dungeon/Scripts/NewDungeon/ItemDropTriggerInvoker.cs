

using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    public class ItemDropTriggerInvoker : TriggerActionInvoker
    {
        GameObject[] drops;
        
        void Start()
        {

        }

        public override void Interact()
        {
            Debug.Log("HIT");
            drops = WeaponLog.Instance.GetWeapons();
            
            Assert.IsTrue(drops.Length > 0);

            foreach (var drop in drops)
            {
                Object.Instantiate(drop, OffCenter(PlayerInstance.Instance.gameObject.transform.position), Quaternion.identity, PlayerInstance.Instance.gameObject.transform.parent);
            }

            Destroy(this.gameObject);
        }

        private Vector3 OffCenter(Vector3 basePos)
        {
            System.Random rand = new System.Random();
            float xShift = (float)(rand.NextDouble() * 2 - 1);
            float zShift = (float)(rand.NextDouble() * 2 - 1);

            return new Vector3(basePos.x + xShift, basePos.y, basePos.z + zShift);
        }

    }
}
