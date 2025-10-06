using UnityEngine;
using System.Collections;
public class Blob : MonoBehaviour
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private int referencePointsCount = 12;
    [SerializeField] private float referencePointRadius = 0.25f;
    [SerializeField] private float mappingDetail = 10;
    [SerializeField] private float springDampingRatio = 0;
    [SerializeField] private float springFrequency = 2;
    [SerializeField] private PhysicsMaterial2D surfaceMaterial;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform pointParent;
    private Rigidbody2D[] allReferencePoints;
    private GameObject[] referencePoints;
    private int vertexCount;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;
    private Vector3[,] offsets;
    private float[,] weights;
    private float currentScale = 1f;
    private float startScale = 2.5f;
    private Vector3[] initialLocalPositions;
    private Vector3[] virtualReferencePoints;
    private void Start()
    {
        currentScale = 2.5f;
        CreateReferencePoints();
        CreateMesh();
        MapVerticesToReferencePoints();
        virtualReferencePoints = new Vector3[referencePointsCount];
        for (int i = 0; i < referencePointsCount; i++)
            virtualReferencePoints[i] = referencePoints[i].transform.localPosition;
    }
    private void Update()
    {
        UpdateVertexPositions();
    }
    /*private class PropagateCollisions : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Sticky")
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.tag == "DetachOne")
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            if (collision.transform.tag == "DetachAll")
                gameObject.GetComponent<Blob>().TrigThis();
        }
    }*/
    void CreateReferencePoints()
    {
        initialLocalPositions = new Vector3[referencePointsCount];
        referencePoints = new GameObject[referencePointsCount];
        allReferencePoints = new Rigidbody2D[referencePointsCount];
        Vector3 offsetFromCenter = ((0.5f - referencePointRadius) * Vector3.up);
        float angle = 360.0f / referencePointsCount;
        for (int i = 0; i < referencePointsCount; i++)
        {
            referencePoints[i] = new GameObject();
            referencePoints[i].tag = gameObject.tag;
            referencePoints[i].layer = gameObject.layer;
            //referencePoints[i].AddComponent<PropagateCollisions>();
            //referencePoints[i].transform.parent = transform;
            referencePoints[i].transform.parent = pointParent;
            Quaternion rotation = Quaternion.AngleAxis(angle * (i - 1), Vector3.back);

            Vector3 localPos = rotation * offsetFromCenter;
            referencePoints[i].transform.localPosition = localPos;
            initialLocalPositions[i] = localPos; // 保存初始位置

            referencePoints[i].transform.localPosition = rotation * offsetFromCenter;
            Rigidbody2D body = referencePoints[i].AddComponent<Rigidbody2D>();
            if (tag == "bullet")
                body.gravityScale = 0;
            body.constraints = RigidbodyConstraints2D.None;
            body.mass = 0.5f;
            body.interpolation = rb.interpolation;
            body.collisionDetectionMode = rb.collisionDetectionMode;
            allReferencePoints[i] = body;
            CircleCollider2D collider = referencePoints[i].AddComponent<CircleCollider2D>();
            collider.radius = referencePointRadius * (transform.localScale.x / 2);
            if (surfaceMaterial != null)
                collider.sharedMaterial = surfaceMaterial;
            AttachWithSpringJoint(referencePoints[i], gameObject);
            if (i > 0)
                AttachWithSpringJoint(referencePoints[i],referencePoints[i - 1]);
        }
        AttachWithSpringJoint(referencePoints[0],referencePoints[referencePointsCount - 1]);
        IgnoreCollisionsBetweenReferencePoints();
    }
    private void AttachWithSpringJoint(GameObject referencePoint,GameObject connected)
    {
        SpringJoint2D springJoint = referencePoint.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint) - LocalPosition(connected);
        springJoint.distance = 0;
        springJoint.dampingRatio = springDampingRatio;
        springJoint.frequency = springFrequency;
    }
    private void IgnoreCollisionsBetweenReferencePoints()
    {
        int i;
        int j;
        CircleCollider2D a;
        CircleCollider2D b;
        for (i = 0; i < referencePointsCount; i++)
        {
            for (j = i; j < referencePointsCount; j++)
            {
                a = referencePoints[i].GetComponent<CircleCollider2D>();
                b = referencePoints[j].GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(a, b, true);
            }
        }
    }
    private void CreateMesh()
    {
        vertexCount = (width + 1) * (height + 1);
        int trianglesCount = width * height * 6;
        vertices = new Vector3[vertexCount];
        triangles = new int[trianglesCount];
        uv = new Vector2[vertexCount];
        int t;
        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                int v = (width + 1) * y + x;
                vertices[v] = new Vector3(x / (float)width - 0.5f, y / (float)height - 0.5f, 0);
                uv[v] = new Vector2(x / (float)width, y / (float)height);
                if (x < width && y < height)
                {
                    t = 3 * (2 * width * y + 2 * x);
                    triangles[t] = v;
                    triangles[++t] = v + width + 1;
                    triangles[++t] = v + width + 2;
                    triangles[++t] = v;
                    triangles[++t] = v + width + 2;
                    triangles[++t] = v + 1;
                }
            }
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    private void MapVerticesToReferencePoints()
    {
        offsets = new Vector3[vertexCount, referencePointsCount];
        weights = new float[vertexCount, referencePointsCount];
        for (int i = 0; i < vertexCount; i++)
        {
            float totalWeight = 0;
            for (int j = 0; j < referencePointsCount; j++)
            {
                offsets[i, j] = vertices[i] - LocalPosition(referencePoints[j]);
                weights[i, j] = 1 / Mathf.Pow(offsets[i, j].magnitude, mappingDetail);
                totalWeight += weights[i, j];
            }
            for (int j = 0; j < referencePointsCount; j++)
                weights[i, j] /= totalWeight;
        }
    }
    private void UpdateVertexPositions()
    {
        Vector3[] vertices = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            vertices[i] = Vector3.zero;
            for (int j = 0; j < referencePointsCount; j++)
                vertices[i] += weights[i, j] * (LocalPosition(referencePoints[j]) + offsets[i, j]);
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
    private Vector3 LocalPosition(GameObject obj)
    {
        return transform.InverseTransformPoint(obj.transform.position);
    }
    public void TrigThis()
    {
        int z = 0;
        foreach (Rigidbody2D child in allReferencePoints)
        {
            allReferencePoints[z].constraints = RigidbodyConstraints2D.None;
            z++;
        }
    }
    public void resize(float newScale)
    {
        if (Mathf.Approximately(newScale, currentScale)) return;
        float scaleFactor = newScale / startScale;
        rb.simulated = false;
        foreach (var r in allReferencePoints)
        {
            r.simulated = false;
            var col = r.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
        transform.localScale = new Vector3(newScale, newScale, 1);
        for (int i = 0; i < referencePointsCount; i++)
            virtualReferencePoints[i] = initialLocalPositions[i] * scaleFactor;
        UpdateVertexPositionsWithVirtualPoints();
        StartCoroutine(SyncRealPointsToVirtualSmooth());
        currentScale = newScale;
    }
    private IEnumerator SyncRealPointsToVirtualSmooth()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3[] startPositions = new Vector3[referencePointsCount];
        for (int i = 0; i < referencePointsCount; i++)
            startPositions[i] = referencePoints[i].transform.localPosition;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            for (int i = 0; i < referencePointsCount; i++)
                referencePoints[i].transform.localPosition = Vector3.Lerp(startPositions[i], virtualReferencePoints[i], t);
            yield return null;
        }
        for (int i = 0; i < referencePointsCount; i++)
            referencePoints[i].transform.localPosition = virtualReferencePoints[i];
        foreach (var rp in referencePoints)
        {
            foreach (var joint in rp.GetComponents<SpringJoint2D>())
            {
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = LocalPosition(rp) - LocalPosition(joint.connectedBody.gameObject);
                joint.distance = 0f; 
                joint.enabled = true;
            }
        }
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.Sleep();
        foreach (var r in allReferencePoints)
        {
            r.simulated = true;
            r.velocity = Vector2.zero;
            r.angularVelocity = 0;
            r.Sleep();
            var col = r.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = true;
        }
    }
    private void UpdateVertexPositionsWithVirtualPoints()
    {
        Vector3[] vertices = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            vertices[i] = Vector3.zero;
            for (int j = 0; j < referencePointsCount; j++)
                vertices[i] += weights[i, j] * (virtualReferencePoints[j] + offsets[i, j]);
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}