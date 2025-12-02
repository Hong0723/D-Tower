using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public Transform pathParent;   // PathGizmo 달린 오브젝트
    public float speed = 2f;

    private Transform[] wp;
    private int index = 0;

    private EnemyDirection dirScript;

    void Start()
    {
        dirScript = GetComponent<EnemyDirection>();  // 3번에서 붙인 스크립트
        wp = pathParent.GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (index >= wp.Length - 1) return;

        Vector3 target = wp[index + 1].position;
        Vector3 delta = target - transform.position;

        // 목표 지점에 도착했으면 다음 wp로
        if (delta.magnitude < 0.03f)
        {
            index++;
            return;
        }

        // 이동
        transform.position += delta.normalized * speed * Time.deltaTime;

        // 방향 전환
        ChangeDirection(index);
    }

    void ChangeDirection(int i)
    {
        if (i >= 0 && i < 2) dirScript.LookRight();      // wp0 ~ wp2
        else if (i >= 2 && i < 3) dirScript.LookDown();  // wp2 ~ wp3
        else if (i >= 3 && i < 4) dirScript.LookLeft();  // wp3 ~ wp4
        else if (i >= 4 && i < 5) dirScript.LookDown();  // wp4 ~ wp5
        else if (i >= 5 && i < 6) dirScript.LookLeft();  // wp5 ~ wp6
        else if (i >= 6 && i < 7) dirScript.LookDown();  // wp6 ~ wp7
        else if (i >= 7 && i < 8) dirScript.LookRight(); // wp7 ~ wp8
        else if (i >= 8 && i < 9) dirScript.LookUp();    // wp8 ~ wp9
        else if (i >= 9 && i < 11) dirScript.LookRight();// wp9 ~ wp11
        else if (i >= 11 && i < 13) dirScript.LookUp();  // wp11 ~ wp13
        else if (i >= 13 && i < 14) dirScript.LookLeft();// wp13 ~ wp14
        else if (i >= 14 && i < 15) dirScript.LookUp();  // wp14 ~ wp15
        else if (i >= 15 && i < 17) dirScript.LookRight();// wp15 ~ wp17
        else if (i >= 17 && i < 19) dirScript.LookDown();// wp17 ~ wp19
        else if (i >= 19 && i < 23) dirScript.LookLeft();// wp19 ~ wp23
        else if (i >= 23 && i < 24) dirScript.LookDown();// wp23 ~ wp24
        else if (i >= 24 && i < 25) dirScript.LookRight();// wp24 ~ wp25
        else if (i >= 25 && i < 27) dirScript.LookDown();// wp25 ~ wp27
    }
}
