using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a given position, but fail if the player gets too close during its execution.
    /// </summary>
    [Action("Custom/Check/WaryMoveTo")]
    [Help("Moves the game object to a position by using a NavMeshAgent but fails if the target gets too close")]
    public class WaryMoveTo : GOAction
    {
        ///<value>Input target game object towards this game object will be moved Parameter.</value>
        [InParam("Target")]
        [Help("Target GameObject being tracked")]
        public GameObject target;

        [InParam("TriggerDistance")]
        [Help("How close the target can be before it triggers failure")]
        public float triggerDist;

        private UnityEngine.AI.NavMeshAgent navAgent;
        private Vector3 targetPos;

        public override void OnStart()
        {
            if (target == null)
            {
                Debug.LogError("The target of this action is null", gameObject);
                return;
            }

            targetPos = gameObject.transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            navAgent.SetDestination(targetPos);
        }

        public override TaskStatus OnUpdate()
        {
            if (Vector3.Distance(target.transform.position, targetPos) < triggerDist)
            {
                navAgent.SetDestination(gameObject.transform.position);
                return TaskStatus.FAILED;
            }
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                return TaskStatus.COMPLETED;
            else if (navAgent.destination != targetPos)
                navAgent.SetDestination(targetPos);
            return TaskStatus.RUNNING;
        }
    }
}
