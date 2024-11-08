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
    private const float actionDelay = 1f;          // �������� ����� ���������
    private float actionTimer = 0f;                // ������ ��� ������������ ��������
    private bool canPerformAction = true;          //  ����� �� ��������� ��������

    // �������� TargetUnitName
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
            // ��������� ����� ��� �������� ����� ���������
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
            // ����������� ������ ��������
            actionTimer += Time.deltaTime;
            if (actionTimer >= actionDelay)
            {
                // ����� �������� ��������� ���������� ��������
                canPerformAction = true;
                actionTimer = 0f; // ���������� ������
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
                isTransitioning = false; // ���������
                transitionTimer = 0f; // ��������
                canPerformAction = true; // ��������� ���������� �������� ����� ��������
            }
        }
    }

    // ����� ��� �����������
    public void Move()
    {
        // �������

        // ����� ������ �������� ��������� ����� ������� �� ����� ��������
        canPerformAction = false;
        isTransitioning = true; // ������ � ��������� ��������
        currentState = State.Attacking; // ������ ��������� �� "�������" ����� ��������
    }

    // ����� ��� �����
    public void Attack()
    {
        // ������ ������

        // ����� ����� ��������� �������� ����� ����� ���������
        canPerformAction = false;
        isTransitioning = true; // ����� ������ � ��������� ��������
        currentState = State.Moving; // ������ �������� ��������� �� "��������"
    }
}