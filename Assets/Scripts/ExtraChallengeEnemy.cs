using UnityEngine;

public class ExtraChallengeEnemy : MonoBehaviour
{
    public Stats enemyStats;

    [Tooltip("The transform to which the enemy will pace back and forth to.")]
    public Transform[] patrolPoints;

    private int currentPatrolPoint = 0; // Initialize with 0, since arrays are zero-based

    /// <summary>
    /// Contains tunable parameters to tweak the enemy's movement.
    /// </summary>
    [System.Serializable]
    public struct Stats
    {
        [Header("Enemy Settings")]

        [Tooltip("How fast the enemy moves.")]
        public float speed;

        [Tooltip("Whether the enemy should move or not")]
        public bool move;
    }

    void Update()
    {
        //if the enemy is allowed to move
        if (enemyStats.move == true) // Use '==' for comparison, not '=' which is for assignment
        {
            Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
            transform.position = Vector3.MoveTowards(transform.position, moveToPoint, enemyStats.speed * Time.deltaTime);

            // Check if the enemy is close enough to the current patrol point
            if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
            {
                currentPatrolPoint++;

                if (currentPatrolPoint >= patrolPoints.Length) // Use '>=' instead of '>'
                {
                    currentPatrolPoint = 0; // Use '=' instead of '==' for assignment
                }
            }
        }
    }
}
