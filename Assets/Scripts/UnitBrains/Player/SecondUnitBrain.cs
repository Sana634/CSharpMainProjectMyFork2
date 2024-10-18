<<<<<<< HEAD
﻿using System.Collections.Generic;//Подключает пространство имен, которое содержит общие типы коллекций, такие как List
using GluonGui.Dialog;// Подключает пространство имен для работы с диалогами в игровом интерфейсе
using Model.Runtime.Projectiles;//Подключает пространство имен, относящееся к моделям снарядов, позволяя использовать их в коде.
using UnityEngine;// Подключает пространство имен Unity, предоставляющее доступ к основным функциональным возможностям движка.
=======
﻿using System.Collections.Generic;
using GluonGui.Dialog;
using Model.Runtime.Projectiles;
using UnityEngine;
>>>>>>> 55cb42b9d28f7cfa58eeb034c4b97b2b79900bdd

namespace UnitBrains.Player//Определяет пространство имен для логики, связанной с игроком.
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain//Объявляет класс SecondUnitBrain, который наследует поведение от DefaultPlayerUnitBrain.
    {
        public override string TargetUnitName => "Cobra Commando";//Задает имя целевой единицы
        private const float OverheatTemperature = 3f;//Константа, определяющая порог температуры перегрева.
        private const float OverheatCooldown = 2f;//Константа, определяющая время охлаждения после перегрева.
        private float _temperature = 0f;//Переменная для хранения текущей температуры.
        private float _cooldownTime = 0f;//Переменная для отслеживания времени охлаждения.
        private bool _overheated;//Логическая переменная, указывающая, перегрелся ли снаряд.

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)//Переопределяет метод для генерации снарядов, принимая цели и список для добавления снарядов.
        {
            float overheatTemperature = OverheatTemperature;// Локальная переменная для хранения порога перегрева.

            // Получаем текущую температуру 
            float currentTemperature = GetTemperature();

            // Если текущая температура больше или равна температуре перегрева, выходим из метода
            if (currentTemperature >= overheatTemperature)//Проверяет, не перегрелся ли снаряд. Если перегрет, выполнение метода прекращается.
            {//
                return; // Прервать выполнение метода
            }

            // Проверяем температуру и добавляем снаряды в список
            for (int i = 0; i <= currentTemperature; i++)
            {
<<<<<<< HEAD
=======

>>>>>>> 55cb42b9d28f7cfa58eeb034c4b97b2b79900bdd


                var projectile = CreateProjectile(forTarget);//Создает новый снаряд, нацеливаясь на указанную позицию.
                AddProjectileToList(projectile, intoList);//Добавляет созданный снаряд в список.
            }

            // Увеличиваем температуру после выполнения всех действий
            IncreaseTemperature();
        }

        public override Vector2Int GetNextStep()//метод возвращает соседнюю клетку, куда должен следовать юнит
        {
            return base.GetNextStep();
        }

<<<<<<< HEAD
        protected override List<Vector2Int> SelectTargets()//Метод для выбора цели.
        {
            List<Vector2Int> result = GetReachableTargets();//Получает список доступных для атаки целей.

            if (result.Count == 0)//Если список пуст, возвращает новый пустой список.
            {
                return new List<Vector2Int>();
            }

            Vector2Int target = new Vector2Int();//Переменная для хранения лучшей цели.
            float minDistance = float.MaxValue;//Инициализирует минимальное расстояние максимальным значением.
            foreach (Vector2Int targetenemy in result)//Перебирает все доступные цели.
            {
                float distance = DistanceToOwnBase(targetenemy);//Вычисляет расстояние до своей базы.
                if (distance < minDistance)//Если найдено более близкое расстояние, обновляет минимальное и целевое значение.
=======
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
>>>>>>> 55cb42b9d28f7cfa58eeb034c4b97b2b79900bdd
                {
                    minDistance = distance;
                    target = targetenemy;
                }
            }

<<<<<<< HEAD
            result.Clear();//Очищает список доступных целей.
            result.Add(target);//Добавляет лучшую цель в список.

            while (result.Count > 1)//Удаляет лишние цели до одной (если они были).
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;//Возвращает список с единственной целью.

=======
            result.Clear();
            result.Add(target);

            while (result.Count > 1)
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;
                     
>>>>>>> 55cb42b9d28f7cfa58eeb034c4b97b2b79900bdd

        }

        public override void Update(float deltaTime, float time)// Переопределяет метод обновления; принимает временные параметры
        {
            if (_overheated)//Проверяет, перегрет ли снаряд.
            {
                _cooldownTime += Time.deltaTime;//Увеличивает время охлаждения на прошедшее время.
                float t = _cooldownTime / (OverheatCooldown / 10);//Нормализует время охлаждения.
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);//Интерполирует температуру от максимума до нуля, основанное на времени охлаждения.
                if (t >= 1)// Если охлаждение завершено, сбрасывает таймер и перегрев.
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()//Метод для получения текущей температуры.
        {
            if (_overheated) return (int)OverheatTemperature;//Если перегрев, возвращает максимальную температуру.
            else return (int)_temperature;//В противном случае возвращает текущее значение температуры.
        }

        private void IncreaseTemperature()//Метод для увеличения температуры.
        {
            _temperature += 1f;//Увеличивает текущее значение температуры на 1.
            if (_temperature >= OverheatTemperature) _overheated = true;//Если температура достигает максимума, устанавливает состояние перегрев
        }
    }
}