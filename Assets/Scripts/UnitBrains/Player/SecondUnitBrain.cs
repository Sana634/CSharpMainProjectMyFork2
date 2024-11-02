using System.Collections.Generic;//Подключает пространство имен, которое содержит общие типы коллекций, такие как List
using GluonGui.Dialog;// Подключает пространство имен для работы с диалогами в игровом интерфейсе
using Model.Runtime.Projectiles;//Подключает пространство имен, относящееся к моделям снарядов, позволяя использовать их в коде.
using UnityEngine;// Подключает пространство имен Unity, предоставляющее доступ к основным функциональным возможностям движка.

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


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static int quantity_unit = 0;
        private int number_unit; // Номер юнита
        private const int max_goals = 3; // Максимальное количество целей для выбора
        private Vector3 playerBasePosition; // Позиция базы игрока 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
        public SecondUnitBrain(Vector3 basePosition)
        {
            playerBasePosition = basePosition; // Инициализация позиции базы
            number_unit = ++quantity_unit; // Присвоение уникального номера юниту
        }

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


                var projectile = CreateProjectile(forTarget);//Создает новый снаряд, нацеливаясь на указанную позицию.
                AddProjectileToList(projectile, intoList);//Добавляет созданный снаряд в список.
            }

            // Увеличиваем температуру после выполнения всех действий
            IncreaseTemperature();
        }
        public override Vector2Int GetNextStep() // Получение следующего шага
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets() // Метод выбора целей
        {
            List<Vector2Int> reachableTargets = GetReachableTargets(); // Получаем список доступных целей

            if (reachableTargets.Count == 0) return new List<Vector2Int>(); // Проверка на пустой список

            Vector2Int bestTarget = new Vector2Int(); //Переменная для хранения лучшей цели.
            float minDistance = float.MaxValue;//Инициализирует минимальное расстояние максимальным значением

            foreach (Vector2Int enemyTarget in reachableTargets) // Перебор доступных целей
            {
                float distance = DistanceToOwnBase(enemyTarget); //Вычисляет расстояние до своей базы.
                if (distance < minDistance) //Если найдено более близкое расстояние, обновляет минимальное и целевое значение.
                {
                    minDistance = distance;
                    bestTarget = enemyTarget;
                }
            }

            List<GameObject> potentialTargets = FindPotentialTargets(); // Находим потенциальные цели
            SortByDistanceToOwnBase(potentialTargets); // Сортируем по расстоянию до базы
            return potentialTargets.Count > max_goals
                ? potentialTargets.GetRange(0, max_goals).ConvertAll(target => new Vector2Int((int)target.transform.position.x, (int)target.transform.position.y))
                : new List<Vector2Int>(); // Возвращаем отсортированные цели
        }

        private void SortByDistanceToOwnBase(List<GameObject> targets) // Сортировка целей по расстоянию до базы
        {
            targets.Sort((a, b) => {
                float distanceA = Vector3.Distance(a.transform.position, playerBasePosition);
                float distanceB = Vector3.Distance(b.transform.position, playerBasePosition);
                return distanceA.CompareTo(distanceB);
            });
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