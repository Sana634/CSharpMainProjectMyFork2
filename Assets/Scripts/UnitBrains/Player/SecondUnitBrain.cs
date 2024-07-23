using System.Collections.Generic;
using GluonGui.Dialog;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;

            // Получаем текущую температуру
            float currentTemperature = GetTemperature();

            // Если текущая температура больше или равна температуре перегрева, выходим из метода
            if (currentTemperature >= overheatTemperature)
            {
                return; // Прервать выполнение метода
            }

            // Проверяем температуру и добавляем снаряды в список
            for (int i = 0; i <= currentTemperature; i++)
            {


                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }

            // Увеличиваем температуру после выполнения всех действий
            IncreaseTemperature();
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        { 
            List<Vector2Int> result = GetReachableTargets();
        
            if (result.Count == 0)
            {
                return new List<Vector2Int>();  
            }

            Vector2Int target = new Vector2Int();
            float minDistance = float.MaxValue;
            foreach (Vector2Int targetenemy in result)
            { 
                float distance = DistanceToOwnBase(targetenemy);
                if (distance<minDistance)
                {
                    minDistance = distance;
                    target = targetenemy;
                }
            }

            result.Clear();
            result.Add(target);

            while (result.Count > 1)
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;
                     

        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}