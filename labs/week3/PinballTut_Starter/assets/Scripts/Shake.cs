
using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
     // Transform of the GameObject you want to shake
    private Transform transform;
    
    // Desired duration of the shake effect
    private float shakeDuration = 0f;
    
    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.5f;
    
    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;
    
    // The initial position of the GameObject
    Vector3 initialPosition;

    void Awake()
    {
        if (transform == null)
        {
        transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        // store initial position of camera gameobject
        initialPosition = transform.localPosition;
    }

    public void TriggerShake() {
        shakeDuration = 0.2f;

        GameObject b = GameObject.Find("ball");
        Debug.Log(b);
        b.GetComponent<Rigidbody2D>().AddForce(new Vector3(10.0f, 0.0f, 1.0f), ForceMode2D.Impulse);
    }

    void Update()
    {

        if (shakeDuration > 0){
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;            
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }

        else{
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    void FixedUpdate(){
        if (Input.GetKeyDown(KeyCode.T) == true){
            this.TriggerShake();
        }
    }
}