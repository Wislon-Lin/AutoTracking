using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAIPath : MonoBehaviour {
	public NavMeshAgent AgentObj;
	public Transform Target;
	
	// Update is called once per frame
	void Update () {
		AgentObj.SetDestination (Target.position);
	}
}
