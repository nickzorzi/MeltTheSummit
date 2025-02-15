using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public Transform target;
    public float speed = 20;
    private EnemyController eC;

    Vector2[] path;
    int targetIndex;

    Coroutine followPathCoroutine;

    void Start() {
        eC = GetComponent<EnemyController>();
        StartCoroutine(RefreshPath());
    }

    void Update() {
        if (eC.isKnockback) {
            if (followPathCoroutine != null) {
                StopCoroutine(followPathCoroutine);
                followPathCoroutine = null;
            }
        } else {
            if (followPathCoroutine == null) {
                followPathCoroutine = StartCoroutine(FollowPath());
            }
        }
    }

    IEnumerator RefreshPath() {
        Vector2 targetPositionOld = (Vector2)target.position + Vector2.up; // ensure != to target.position initially
        
        while (true) {
            if (targetPositionOld != (Vector2)target.position) {
                targetPositionOld = (Vector2)target.position;

                path = Pathfinder.RequestPath(transform.position, target.position);
                if (followPathCoroutine != null) {
                    StopCoroutine(followPathCoroutine);
                }
                followPathCoroutine = StartCoroutine(FollowPath());
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator FollowPath() {
        if (path.Length > 0) {
            targetIndex = 0;
            Vector2 currentWaypoint = path[0];

            while (targetIndex < path.Length) {
                if ((Vector2)transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex < path.Length) {
                        currentWaypoint = path[targetIndex];
                    }
                }

                if (!eC.isKnockback) {
                    transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                }

                yield return null;
            }
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
