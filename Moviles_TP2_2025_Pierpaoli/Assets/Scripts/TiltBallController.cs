using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class TiltBallController : MonoBehaviour
{
    [Header("Control")]
    public float force = 14f;
    public float maxTiltDegrees = 25f;    // clamp del ángulo
    public float smooth = 0.15f;          // filtro (0..1)
    public bool invertX = false;
    public bool invertY = false;

    [Header("Calibración")]
    public bool autoCalibrateOnStart = true;
    private Vector3 calibration = Vector3.zero;

    private Rigidbody rb;
    private Vector3 filtered;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (Accelerometer.current == null)
            Debug.LogWarning("No hay acelerómetro (¿Editor?). En Android funciona.");
    }

    void Start()
    {
        if (autoCalibrateOnStart) Calibrate();
    }

    public void Calibrate()
    {
        // Toma una “posición neutra”
        var acc = ReadAccelRaw();
        calibration = acc;
    }

    void FixedUpdate()
    {
        var acc = ReadAccelRaw() - calibration;
        // Filtro exponencial
        filtered = Vector3.Lerp(filtered, acc, 1f - Mathf.Exp(-smooth * 60f * Time.fixedDeltaTime));

        // Mapear a plano XZ
        float x = (invertX ? -filtered.x : filtered.x);
        float y = (invertY ? -filtered.y : filtered.y);

        // Limitar “inclinación” máxima
        x = Mathf.Clamp(x, -Mathf.Sin(maxTiltDegrees * Mathf.Deg2Rad), Mathf.Sin(maxTiltDegrees * Mathf.Deg2Rad));
        y = Mathf.Clamp(y, -Mathf.Sin(maxTiltDegrees * Mathf.Deg2Rad), Mathf.Sin(maxTiltDegrees * Mathf.Deg2Rad));

        // Empuje en XZ (usa Y del sensor como “adelante/atrás”)
        Vector3 dir = new Vector3(x, 0f, y);
        rb.AddForce(dir * force, ForceMode.Acceleration);
    }

    private Vector3 ReadAccelRaw()
    {
        if (Accelerometer.current != null)
            return Accelerometer.current.acceleration.ReadValue(); // g-force en el marco del dispositivo

        // En editor: flechas para simular
        float simX = Input.GetAxis("Horizontal") * 0.3f;
        float simY = Input.GetAxis("Vertical") * 0.3f;
        return new Vector3(simX, simY, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Debug.Log("WIN: Llegaste a la meta.");
            // TODO: cargar pantalla de Win o volver a Menú
        }
    }
}
