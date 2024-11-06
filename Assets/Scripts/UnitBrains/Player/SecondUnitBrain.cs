using System.Collections.Generic;
using Model;
using Model.Runtime.Projectiles;
using Unity.VisualScripting;
using UnityEngine;
using UnitBrains;
using UnitBrains.Pathfinding;
using System.IO;
using Utilities;

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

        private List<Vector2Int> TargetsOutOfRange = new List<Vector2Int>();

        private static int _unitIndex = 0;
        private const int MaxTargets = 4;
        public int UnitID { get; private set; }

        public SecondUnitBrain()
        {
            UnitID = _unitIndex++;
            Debug.Log($"Unit id is: {UnitID}");
        }

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            float temp = GetTemperature();

            if (temp >= overheatTemperature) return;

            for (int i = 0; i <= temp; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }

            IncreaseTemperature();

        }

        public override Vector2Int GetNextStep()
        {
            Vector2Int target = TargetsOutOfRange.Count > 0 ? TargetsOutOfRange[0] : unit.Pos;

            if (IsTargetInRange(target))
            {
                return unit.Pos;
            }
            else
            {
                return unit.Pos.CalcNextStepTowards(target);
            }

        }

        protected override List<Vector2Int> SelectTargets()//переопределяет метод для выбора целей для атаки или перемещения.
        {
            List<Vector2Int> result = new List<Vector2Int>();//создает новый список для хранения выбранных целей.
            Vector2Int targetPosition;//объявляет переменную для хранения позиции выбранной цели.

            TargetsOutOfRange.Clear();//очищает список целей

            foreach (Vector2Int target in GetAllTargets()) // итерация по всем доступным целям, возвращаемым методом GetAllTargets.
            {
                TargetsOutOfRange.Add(target);//добавление каждой цели в список TargetsOutOfRange.
            }

            if (TargetsOutOfRange.Count == 0) //проверка, если список целей пуст.
            {
                int enemyBaseId = IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId; // определяет идентификатор базы противника
                Vector2Int enemyBase = runtimeModel.RoMap.Bases[enemyBaseId];//получает позицию базы противника из карты.
                TargetsOutOfRange.Add(enemyBase);//добавляет базу противника в список целей.
            }
            else
            {
                SortByDistanceToOwnBase(TargetsOutOfRange); //сортирует список целей в зависимости от их расстояния до базы юнита.

                int targetIndex = UnitID % MaxTargets;//использует уникальный идентификатор юнита для определения индекса цели, выбирая цель из отсортированного списка.

                if (targetIndex > (TargetsOutOfRange.Count - 1)) //проверяет, выходит ли индекс за пределы доступных целей.
                {
                    targetPosition = TargetsOutOfRange[0];//сли индекс превышает количество целей, устанавливается первая цель.
                }
                else
                {
                    if (targetIndex == 0)
                    {
                        targetPosition = TargetsOutOfRange[targetIndex];
                    }
                    else
                    {
                        targetPosition = TargetsOutOfRange[targetIndex - 1];
                    }

                }

                if (IsTargetInRange(targetPosition)) //проверяет, находится ли выбранная цель в пределах досягаемости.
                    result.Add(targetPosition);//добавляет цель в результирующий список.
            }

            return result;

        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown / 10);
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
            if (_overheated) return (int)OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}