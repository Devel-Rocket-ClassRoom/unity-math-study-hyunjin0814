using UnityEngine;

public class BezierSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private GameObject spherePrefab;

    private bool shoot;
    void Update()
    {
        shoot = Input.GetKeyDown(KeyCode.Space);

        if (shoot)
        {
            int randomCount = Random.Range(3, 7);
            for (int i = 0; i < randomCount; i++)
            {
                GameObject go = Instantiate(spherePrefab);
                BezierMover mover = go.GetComponent<BezierMover>();
                if (mover != null)
                {
                    mover.Initialize(startPoint.position, endPoint.position);
                }
            }
        }
    }
}
