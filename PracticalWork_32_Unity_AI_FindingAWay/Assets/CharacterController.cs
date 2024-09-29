using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{   
    [SerializeField] private List<NavMeshAgent> _visitors;

    [SerializeField] private List<NavMeshAgent> _securitys;

    [SerializeField] private float _timeAtTheGoal = 5f;

    [SerializeField] private float _timeForSecurity = 2f;

    private GameObject[] targets;    

    private int randTarget, numberVisitorAtTheTarget;

    private bool isVisitorFree = false, isSecurityFree = true, isFree = true, isTimerOn = true;

    private float currentTime, currentTimeForSecurity;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");        

        currentTime = _timeAtTheGoal;

        currentTimeForSecurity = _timeForSecurity;

        StartMovingTowardsTheGoal();        
    }

    private void Update()
    {
        for (int i = 0; i < _visitors.Count; i++)
        {
            if (_visitors[i].remainingDistance < _visitors[i].stoppingDistance + 1f && isTimerOn)
            {
                Timer(_timeAtTheGoal);
            }

            if (isFree)
            {
                MovingTowardsTheGoal(i);
            }

            isFree = false;

            isTimerOn = true;            
        }
        
        for (int i = 0; i < _securitys.Count; i++)
        {
            if (_securitys[i].remainingDistance < _securitys[i].stoppingDistance + 1f)
            {
                TimerSecurity();

                numberVisitorAtTheTarget = AchievingTheGoal();

                if (numberVisitorAtTheTarget >= 0 && isSecurityFree)
                {
                    _securitys[i].destination = _visitors[numberVisitorAtTheTarget].transform.position;
                }

                isSecurityFree = false;
            }
        }
    }

    /// <summary>
    /// �������� ���������� ���� � ����������
    /// </summary>
    private int AchievingTheGoal()
    {
        for (int i = 0; i < _visitors.Count; i++)
        {
            if (_visitors[i].remainingDistance < _visitors[i].stoppingDistance + 1f)
            {
                return i;                
            }
        }

        return -1;
    }

    /// <summary>
    /// �������� ����������� �� ������
    /// </summary>
    public void StartMovingTowardsTheGoal()
    {
        for (int i = 0; i < _visitors.Count; i++)
        {
            MovingTowardsTheGoal(i);
        }
    }

    /// <summary>
    /// �������� ����������� � ��������� ����
    /// </summary>
    /// <param name="numberTarget"></param>
    public void MovingTowardsTheGoal(int numberTarget)
    {        
        randTarget = Random.Range(0, targets.Length);

        _visitors[numberTarget].destination = targets[randTarget].transform.position;
       
    }     
    
    /// <summary>
    /// �������� ������ � ��������� �����������
    /// </summary>
    /// <param name="numberVisitor"></param>
    public void SecurityMovement(int numberVisitor)
    {        
        _securitys[numberVisitor].destination = _visitors[numberVisitorAtTheTarget].transform.position;       
    }    

    /// <summary>
    /// ������ ��� ������� �������, ����������� � ����
    /// </summary>
    private void TimerVisitor()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            isVisitorFree = true;            

            currentTime = _timeAtTheGoal;
        }
    }
    
    private void TimerSecurity()
    {
        if (currentTimeForSecurity > 0)
        {
            currentTimeForSecurity -= Time.deltaTime;
        }
        else
        {
            isSecurityFree = true;            

            currentTimeForSecurity = _timeForSecurity;
        }
    }
    
    private void Timer(float timer)
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isFree = true;
            
        }
        else
        {
            isFree = false;
            isTimerOn = false;
        }
    }
}
