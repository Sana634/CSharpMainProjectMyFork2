using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitBrains.Player;  // Подключаем пространство имен, которое содержит DefaultPlayerUnitBrain

public class ThirdUnitBrain : DefaultPlayerUnitBrain
{
    private enum State
    {
        Moving,
        Attacking,
        Transitioning
    }

    private State currentState = State.Moving;  // Начальное состояние
    private bool isTransitioning = false;
    private float transitionTimer = 0f;
    private const float transitionDuration = 1f;  // Длительность перехода
    private const float actionDelay = 1f;          // Задержка перед действием
    private float actionTimer = 0f;                // Таймер для отслеживания задержки
    private bool canPerformAction = true;          //  можно ли выполнять действия

    // Свойство TargetUnitName
    public string TargetUnitName
    {
        get
        {
            return "Ironclad Behemoth";
        }
    }

    private void Update()
    {
        HandleTransition();

        if (canPerformAction)
        {
            // проверяем какой тип действия нужно выполнить
            switch (currentState)
            {
                case State.Moving:
                    Move();
                    break;

                case State.Attacking:
                    Attack();
                    break;
            }
        }
        else
        {
            // Увеличиваем таймер задержки
            actionTimer += Time.deltaTime;
            if (actionTimer >= actionDelay)
            {
                // После задержки разрешаем выполнение действия
                canPerformAction = true;
                actionTimer = 0f; // Сбрасываем таймер
            }
        }
    }

    private void HandleTransition()
    {
        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            if (transitionTimer >= transitionDuration)
            {
                isTransitioning = false; // Завершить
                transitionTimer = 0f; // Сбросить
                canPerformAction = true; // Разрешаем выполнение действий после перехода
            }
        }
    }

    // Метод для перемещения
    public void Move()
    {
        // перемещ

        // После начала движения запрещаем новые команды на время действия
        canPerformAction = false;
        isTransitioning = true; // Входим в состояние перехода
        currentState = State.Attacking; // Меняем состояние на "Атакуем" после движения
    }

    // Метод для атаки
    public void Attack()
    {
        // Логика атакии

        // После атаки назначаем задержку перед новым действием
        canPerformAction = false;
        isTransitioning = true; // Опять входим в состояние перехода
        currentState = State.Moving; // Теперь изменяем состояние на "Движемся"
    }
}