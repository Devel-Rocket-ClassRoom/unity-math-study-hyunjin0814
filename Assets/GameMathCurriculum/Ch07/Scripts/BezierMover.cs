using UnityEngine;

public class BezierMover : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 rand1;
    private Vector3 rand2;

    private float elapsedTime;
    public float duration = 5f;
    private bool isInitialized = false;

    public void Initialize(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;

        Vector3 randBase1 = Vector3.Lerp(startPoint, endPoint, Random.Range(0.1f, 0.4f));
        Vector3 randBase2 = Vector3.Lerp(startPoint, endPoint, Random.Range(0.6f, 0.9f));

        rand1 = randBase1 + Random.insideUnitSphere * 3f;
        rand2 = randBase2 + Random.insideUnitSphere * 3f;

        elapsedTime = 0f;
        duration = Random.Range(0.5f, 2.5f);

        Renderer sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
            return;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        t = Mathf.Clamp01(t);

        transform.position = CubicBezier(startPoint, rand1, rand2, endPoint, t);

        if (t >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(d, e, t);
    }
}
