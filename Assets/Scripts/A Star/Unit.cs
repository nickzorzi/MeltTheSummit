using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Unit : MonoBehaviour {

    public Transform target;
    public float speed = 2f;
    private EnemyController eC;
    private BossController bC;

    Vector2[] path;
    int targetIndex;

    Coroutine followPathCoroutine;

    public bool startChase = false;

    public Animator _animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    void Start() {
        eC = GetComponent<EnemyController>();
        bC = GetComponent<BossController>();
        _animator = GetComponent<Animator>();
        //StartCoroutine(RefreshPath());
    }

    void FixedUpdate() {
        if ((eC != null && eC.hasLineOfSight && !startChase) || (bC != null && bC.hasLineOfSight && !startChase))
        {
            StartCoroutine(RefreshPath());
            startChase = true;
        }

        if ((eC != null && eC.isKnockback) || (bC != null && bC.isKnockback))
        {
            if (followPathCoroutine != null)
            {
                StopCoroutine(followPathCoroutine);
                followPathCoroutine = null;
            }
        }
        else
        {
            if (followPathCoroutine == null)
            {
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
        if (path != null && path.Length > 0) {
            targetIndex = 0;
            Vector2 currentWaypoint = path[0];

            while (targetIndex < path.Length) {
                if ((Vector2)transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex < path.Length) {
                        currentWaypoint = path[targetIndex];
                    }
                }

                if (eC != null && !eC.isKnockback || bC != null && !bC.isKnockback) {
                    transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                    SetMovementAnimation(currentWaypoint);
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

    private void SetMovementAnimation(Vector2 targetWaypoint)
    {
        Vector2 movementDirection = targetWaypoint - (Vector2)transform.position;

        if (movementDirection != Vector2.zero)
        {
            _animator.SetFloat(_horizontal, movementDirection.x);
            _animator.SetFloat(_vertical, movementDirection.y);

            _animator.SetFloat(_lastHorizontal, movementDirection.x);
            _animator.SetFloat(_lastVertical, movementDirection.y);
        }
    }
}
