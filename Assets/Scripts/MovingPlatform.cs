using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 lastPos;
    private Transform player;

    private float maxHorizontalDistance; // ahora se calcula solo

    void Start()
    {
        lastPos = transform.position;

        //Calcular tamaño real de la plataforma automáticamente
        BoxCollider box = GetComponent<BoxCollider>();

        // tamaño real en mundo (por si hay escala)
        float realX = box.size.x * transform.lossyScale.x;
        float realZ = box.size.z * transform.lossyScale.z;

        // usamos la mitad del mayor de los dos
        maxHorizontalDistance = Mathf.Max(realX, realZ) * 0.5f;
    }

    void Update()
    {
        // Mover plataforma
        transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.05f)
            (pointA, pointB) = (pointB, pointA);

        // Delta de movimiento
        Vector3 delta = transform.position - lastPos;

        if (player != null)
        {
            Vector3 playerPos = player.position;
            Vector3 platPos = transform.position;

            // distancia horizontal (X,Z)
            Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
            Vector2 platXZ = new Vector2(platPos.x, platPos.z);
            float distXZ = Vector2.Distance(playerXZ, platXZ);

            bool fueraHorizontal = distXZ > maxHorizontalDistance;
            bool porDebajo = playerPos.y < platPos.y - 0.1f;

            if (fueraHorizontal || porDebajo)
            {
                player = null;
            }
            else
            {
                player.GetComponent<CharacterController>().Move(delta);
            }
        }

        lastPos = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player"))
            player = hit.collider.transform;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            player = null;
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.up, out RaycastHit hit, 3f))
            {
                if (hit.collider.CompareTag("Player"))
                    player = hit.collider.transform;
            }
        }
    }
}

