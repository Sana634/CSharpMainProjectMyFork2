using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitBrains.Player;  // ���������� ������������ ����, ������� �������� DefaultPlayerUnitBrain


public class ThirdUnitBrain : DefaultPlayerUnitBrain
{
    private enum State
    {
        Moving,
        Attacking,
        Transitioning
    }

    private State currentState = State.Moving;  // ��������� ���������
    private bool isTransitioning = false;
    private float transitionTimer = 0f;
    private const float transitionDuration = 1f;  // ������������ ��������

    // �������� TargetUnitName
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
            isTransitioning = false; // ��������� �������
            transitionTimer = 0f;

            // ������������ ��������� ����� ���������� ��������
            currentState = currentState == State.Moving ? State.Attacking : State.Moving;
        }
    }

    public void Move()//�������� ��������� �� ���� � �������� ���� ��, �� ���������� ������ ����������� � ���� �� ��������
    {
        // ���������� ��������, ���� ���������� �������
        if (isTransitioning)
        {
           return;
        }

        // ���� � ��������� �����, ������ ������� � ��������� ��������
        if (currentState == State.Attacking)
        {
            StartTransition(State.Moving);
            return;
        }

     }

    public void Attack()
    {
        // ���������� �����, ���� ���������� �������
        if (isTransitioning)
        {
           return;
        }

        // ���� � ��������� ��������, ������ ������� � ��������� �����
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