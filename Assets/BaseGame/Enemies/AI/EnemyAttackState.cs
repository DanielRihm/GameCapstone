using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Enemy.AI
{
    public class EnemyAttackState : MonoBehaviour
    {
        void OnEnable()
        {
            Debug.Log($"Attack state got activated {typeof(EnemyAttackState).AssemblyQualifiedName}");
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
