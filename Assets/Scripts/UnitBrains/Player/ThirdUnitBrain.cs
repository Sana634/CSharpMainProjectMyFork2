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

    // Свойство TargetUnitName
    public string TargetUnitName
    {
        get
        {
            return "Ironclad Behemoth";
        }
    }

      private void HandleTransition()
    {
        transitionTimer += Time.deltaTime;
        if (transitionTimer >= transitionDuration)
        {
            isTransitioning = false; // Завершить переход
            transitionTimer = 0f;

            // Переключение состояния после завершения перехода
            currentState = currentState == State.Moving ? State.Attacking : State.Moving;
        }
    }

    public void Move()//Проверка находится ли юнит в переходе Если да, то выполнение метода прерывается и юнит не движется
    {
        // Блокировка движения, если происходит переход
        if (isTransitioning)
        {
           return;
        }

        // Если в состоянии атаки, начать переход в состояние движения
        if (currentState == State.Attacking)
        {
            StartTransition(State.Moving);
            return;
        }

     }

    public void Attack()
    {
        // Блокировка атаки, если происходит переход
        if (isTransitioning)
        {
           return;
        }

        // Если в состоянии движения, начать переход в состояние атаки
        if (currentState == State.Moving)
        {
            StartTransition(State.Attacking);
            return;
        }

    }

    private void StartTransition(State targetState)
    {
        isTransitioning = true;

    }

 }