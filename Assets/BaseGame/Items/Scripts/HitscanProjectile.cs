using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Weapon
{
    [RequireComponent(typeof(LineRenderer))]
    public class HitscanProjectile : Projectile
    {
        private LineRenderer _lineRenderer;

        public LayerMask Ignore;

        public GameObject HitFx;

        // Start is called before the first frame update
        void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = true;

            Assert.IsNotNull(_lineRenderer, $"Hitscan LineRenderer is null {name}");
        }

        void Update()
        {
            Direction = transform.forward;
        }

        void FixedUpdate()
        {
            _lineRenderer.SetPosition(1, new Vector3(0, 0, 100));

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Direction, out hit, 100, ~Ignore))
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy.Enemy>(out var component))
                {
                    component.TakeDamage(Damage * Time.deltaTime);
                }

                // Draw a line to show the path of the laser
                // Line Renderer should have two points, we only need to set the distance ont the second z value
                _lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));

                // If we have a hit fx, spawn it at the hit point
                if (HitFx != null)
                {
                    var hitFx = Instantiate(HitFx, hit.point, Quaternion.identity);

                    // Allign the up to the normal of the hit point
                    hitFx.transform.up = hit.normal;

                    Destroy(hitFx, 0.5f);
                }
            }
        }
    }
}