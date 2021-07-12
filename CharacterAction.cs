using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class CharacterAction : MonoBehaviour
{
    [SerializeField]
    private GameObject grabbedLogs;

    [SerializeField] private Transform HomePoint;
    
    private CharecterStates _charecterStates;
    
    public Transform _currentTree;

    private List<GameObject> trees = new List<GameObject>();

    private NavMeshAgent _navMeshAgent;

    private bool _foundTree = false;
    
    private void Awake()
    {
        _charecterStates = GetComponent<CharecterStates>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _charecterStates.SetCurrentState(States.Idle.ToString());
    }

    void Update()
    {
        if (_charecterStates.GetCurrentState() == States.Idle.ToString() && !_foundTree)
        {
            //если нужно вызывать в апдейте, а не на маус клик
            AddTreesToList();
        }
        else if (_charecterStates.GetCurrentState() == States.GoToTree.ToString())
        {
            ShouldCut();
        }
        else if (_charecterStates.GetCurrentState() == States.CutTree.ToString())
        {
            
        }
        else if (_charecterStates.GetCurrentState() == States.GoToHome.ToString())
        {
            GoHome();
        }
        
    }

    public void AddTreesToList()
    {
        var arrTrees = GameObject.FindGameObjectsWithTag("Tree");

        
        if (arrTrees.Length == 0)
        {
            _charecterStates.SetCurrentState(States.Idle.ToString());
            return;
        }
        
        
        foreach (var tree in arrTrees)
        {
            if (!trees.Contains(tree))
            {
                trees.Add(tree);
                GetClosestTree(trees);
            }

        }
    }

    private void GetClosestTree(List<GameObject> enemies)
    {
        _charecterStates.SetCurrentState(States.GoToTree.ToString());
        _navMeshAgent.isStopped = false;
        
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }

        _currentTree = tMin;

        if (_currentTree != null)
        {
            _foundTree = true;
            _navMeshAgent.SetDestination(_currentTree.position);
        }
    }

    private void ShouldCut()
    {
        float dist = Vector3.Distance(_currentTree.position, transform.position);
        if (dist < 2.5f)
        {
            _navMeshAgent.isStopped = true;
            transform.LookAt(_currentTree);
            _charecterStates.SetCurrentState(States.CutTree.ToString());
        }
    }

    public void DestroyedTree(GameObject tree)
    {
        trees.Remove(tree);
        _charecterStates.SetCurrentState(States.Idle.ToString());
    }

    public void GrabLogsAnim()
    {
        _charecterStates.SetCurrentState(States.GrabLogs.ToString());
    }
    
    public void GrabLogs()
    {
        _charecterStates.SetCurrentState(States.GoToHome.ToString());
        _currentTree.transform.parent.gameObject.SetActive(false);
        grabbedLogs.SetActive(true);
        _navMeshAgent.SetDestination(HomePoint.position);
        _navMeshAgent.isStopped = false;
    }

    private void GoHome()
    {        
        float dist = Vector3.Distance(HomePoint.position, transform.position);
        if (dist <= 0.5f)
        {
            _navMeshAgent.isStopped = true;
            _charecterStates.SetCurrentState(States.Idle.ToString());
            grabbedLogs.SetActive(false);

            if (trees.Count != 0)
            {
                _navMeshAgent.isStopped = false;
                GetClosestTree(trees);
            }
            else
            {
                _currentTree = null;
                _foundTree = false;
                
                
                //если нужно вызывать после погрузки бревен в дом    
                //AddTreesToList();
            }
        }
    }
}
