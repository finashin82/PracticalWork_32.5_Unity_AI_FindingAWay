using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{   
    [SerializeField] private List<NavMeshAgent> _visitors;

    [SerializeField] private List<NavMeshAgent> _securitys;

    [SerializeField] private float _timeForVisitor = 5f;

    [SerializeField] private float _timeForSecurity = 2f;

    private GameObject[] targets;    

    private int randTarget, numberVisitorAtTheTarget;

    private bool isVisitorFree = false, isSecurityFree = true;

    private float currentTimeForVisitor, currentTimeForSecurity;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");        

        currentTimeForVisitor = _timeForVisitor;

        currentTimeForSecurity = _timeForSecurity;

        StartMovingTowardsTheGoal();        
    }

    private void Update()
    {
        for (int i = 0; i < _visitors.Count; i++)
        {
            if (_visitors[i].remainingDistance <= _visitors[i].stoppingDistance)
            {
                TimerVisitor();
            }

            if (isVisitorFree)
            {
                MovingTowardsTheGoal(i);
            }

            isVisitorFree = false;     
        }
        
        for (int i = 0; i < _securitys.Count; i++)
        {
            if (_securitys[i].remainingDistance <= _securitys[i].stoppingDistance)
            {
                TimerSecurity();
            }

            numberVisitorAtTheTarget = AchievingTheGoal();

            if (numberVisitorAtTheTarget >= 0 && isSecurityFree)
            {
                _securitys[i].destination = _visitors[numberVisitorAtTheTarget].transform.position;
            }

            isSecurityFree = false;
            
        }
    }

    /// <summary>
    /// Проверка достижения цели у посетителя
    /// </summary>
    private int AchievingTheGoal()
    {
        for (int i = 0; i < _visitors.Count; i++)
        {
            if (_visitors[i].remainingDistance <= _visitors[i].stoppingDistance)
            {
                return i;                
            }
        }

        return -1;
    }

    /// <summary>
    /// Движение посетителей на старте
    /// </summary>
    public void StartMovingTowardsTheGoal()
    {
        for (int i = 0; i < _visitors.Count; i++)
        {
            MovingTowardsTheGoal(i);
        }
    }

    /// <summary>
    /// Движение посетителей к случайной цели
    /// </summary>
    /// <param name="numberTarget"></param>
    public void MovingTowardsTheGoal(int numberTarget)
    {        
        randTarget = Random.Range(0, targets.Length);

        _visitors[numberTarget].destination = targets[randTarget].transform.position;
       
    }     
    
    /// <summary>
    /// Движение охраны к случайным посетителям
    /// </summary>
    /// <param name="numberVisitor"></param>
    public void SecurityMovement(int numberVisitor)
    {        
        _securitys[numberVisitor].destination = _visitors[numberVisitorAtTheTarget].transform.position;       
    }    

    /// <summary>
    /// Таймер для отсчета времени, проведенной у цели
    /// </summary>
    private void TimerVisitor()
    {
        if (currentTimeForVisitor > 0)
        {
            currentTimeForVisitor -= Time.deltaTime;
        }
        else
        {
            isVisitorFree = true;            

            currentTimeForVisitor = _timeForVisitor;
        }
    }
    
    /// <summary>
    /// Таймер для отсчета времени у охраны
    /// </summary>
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
}
