using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class DecisionRequesterCustom : MonoBehaviour
{
    [SerializeField] private int _decisionPeriod = 5;
    [SerializeField] private bool _takeActionsBetweenDecisions = true;
    
    private Agent _agent;
    
    internal void Awake()
    {
        _agent = gameObject.GetComponent<Agent>();
        Debug.Assert(_agent != null, "Agent component was not found on this gameObject and is required.");
        Academy.Instance.AgentPreStep += MakeRequests;
    }

    void OnDestroy()
    {
        if (Academy.IsInitialized)
        {
            Academy.Instance.AgentPreStep -= MakeRequests;
        }
    }

    public void EnableRequests()
    {
        Academy.Instance.AgentPreStep += MakeRequests;
    }
    
    public void DisableRequests()
    {
        Academy.Instance.AgentPreStep -= MakeRequests;
    }
    
    void MakeRequests(int academyStepCount)
    {
        var context = new DecisionRequester.DecisionRequestContext
        {
            AcademyStepCount = academyStepCount
        };

        if (ShouldRequestDecision(context))
        {
            _agent?.RequestDecision();
        }

        if (ShouldRequestAction(context))
        {
            _agent?.RequestAction();
        }
    }
    
    protected virtual bool ShouldRequestDecision(DecisionRequester.DecisionRequestContext context)
    {
        return context.AcademyStepCount % _decisionPeriod == 0;
    }
    
    protected virtual bool ShouldRequestAction(DecisionRequester.DecisionRequestContext context)
    {
        return _takeActionsBetweenDecisions;
    }
}
