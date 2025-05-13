using LCPS.SlipForge.Engine;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject HealthUnitPrefab;
        [SerializeField] private int HealthUnitRowCount = 10;
        private readonly Stack<HealthUnit> _healthStack = new();
        private float _width;
        private float _height;

        private Observable<int> _maxHealthObserver;
        private Observable<int> _currentHealthObserver;

        private void Start()
        {
            SetWidthAndHeight();

            _maxHealthObserver = DataTracker.GetObservable(data => data.MaxHealth);
            _currentHealthObserver = DataTracker.GetObservable(data => data.CurrentHealth);

            _maxHealthObserver.Subscribe(UpdateHealthBar);
            _currentHealthObserver.Subscribe(UpdateHealthUnits);
        }

        private void OnDestroy()
        {
            _maxHealthObserver.UnSubscribe(UpdateHealthBar);
            _currentHealthObserver.UnSubscribe(UpdateHealthUnits);
        }

        private void UpdateHealthUnits(int health)
        {
            if (_healthStack.Count == 0)
            {
                return;
            }

            // this could be more efficient by assuming the health is always decreasing
            // or increasing; that is, we could stop the recursive calls when
            // healthUnit.IsFilled != _healthStack.Peek().IsFilled (if stack isn't empty)
            //     HOWEVER this increases the conditional complexity of the code for minimal gain
            //     and the health bar should not be updated frequently or be large enough to warrent this
            HealthUnit healthUnit = _healthStack.Pop();
            healthUnit.IsFilled = _healthStack.Count < health;
            UpdateHealthUnits(health);
            _healthStack.Push(healthUnit);
        }

        private void UpdateHealthBar(int maxHealth)
        {
            while (_healthStack.Count < maxHealth)
            {
                AddHealthUnit();
            }
            while (_healthStack.Count > maxHealth)
            {
                RemoveHealthUnit();
            }

            UpdateHealthUnits(_currentHealthObserver.Value);
        }

        private void AddHealthUnit()
        {
            GameObject healthUnit = Instantiate(HealthUnitPrefab, transform);
            // place health unit to the right of the last health unit
            healthUnit.transform.position = GetNextPosition();
            _healthStack.Push(healthUnit.GetComponent<HealthUnit>());
        }

        private void RemoveHealthUnit()
        {
            // alternatively disable and cache them into a separate stack to be reused
            // max health should not be changing frequently enough to warrent this
            Destroy(_healthStack.Pop().gameObject);
        }

        private Vector3 GetNextPosition()
        {
            // ensure the width and height are up to date
            SetWidthAndHeight();

            // either place the health unit at the start of the health bar
            // or to the right of the last health unit
            Vector3 pos = transform.position;
            if (_healthStack.Count > 0)
            {
                pos = _healthStack.Peek().transform.position;

                // if the health unit would be placed outside the health bar
                // it is mod HealthUnitRowCount == 0 because the new health unit hasn't been added yet
                if (_healthStack.Count % HealthUnitRowCount == 0 && _healthStack.Count > 0)
                {
                    pos = new Vector3(transform.position.x, pos.y - _height, pos.z);
                }
                else
                {
                    pos += new Vector3(_width, 0, 0);
                }
            }
            return pos;
        }

        private void SetWidthAndHeight()
        {
            if (_healthStack.Count == 0)
                (_width, _height) = HealthUnitPrefab.GetComponent<RectTransform>().GetGlobalSize();
            else
                (_width, _height) = _healthStack.Peek().GetComponent<RectTransform>().GetGlobalSize();
        }
    }
}
