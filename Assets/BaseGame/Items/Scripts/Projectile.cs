using UnityEngine;
using LCPS.SlipForge.Enemy;

namespace LCPS.SlipForge.Weapon
{
    public class Projectile : MonoBehaviour
    {
		public ProjectileData Data;
		public float Size;
        public float Speed;
        public float Lifetime;
        public int EnemyPenetrationCount;
        private int _penetrations = 0;
        private float _timer;
        public Vector3 Direction;
        public Vector3 Origin;
        public int Damage;
        // Start is called before the first frame update
        void Start()
        {
            _timer = 0;
		}

        // Update is called once per frame
        void Update()
        {
            //transform.forward = Camera.main.transform.forward;
            transform.position += Direction * Time.deltaTime;
            _timer += Time.deltaTime;
            if (_timer > Lifetime)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Enemy.Enemy>(out var enemy))
            {
                enemy.TakeDamage(Damage);

                if (Data != null && Data.Stagger)
                {
                    enemy.Interrupted.Value = true;
                }

                if (_penetrations >= EnemyPenetrationCount) 
                {
                    Destroy(this.gameObject);
                }
                _penetrations++;
            }
            else if (!other.isTrigger && !other.gameObject.TryGetComponent<Projectile>(out var component2)) //Don't destroy if projectiles collide with other projectiles
            {
                Destroy(this.gameObject);
            }
        }
    }

}