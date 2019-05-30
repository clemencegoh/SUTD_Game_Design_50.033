using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePickupController : MonoBehaviour
{

    private GameObject playerObj = null;

    private float next_x = 0;
    private float next_y = 0;

    private int counter = 0;
    private bool state = true;

    public SpriteRenderer m_SpriteRenderer;
    Color m_NewColor;
    float m_Red, m_Blue, m_Green;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj == null)
             playerObj = GameObject.Find("Player");

        //Fetch the SpriteRenderer from the GameObject
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /* 
        next_x = playerObj.transform.position.x - transform.position.x;
        next_y = playerObj.transform.position.y - transform.position.y;
        
        next_x *= 0.01f;
        next_y *= 0.01f;

        transform.Translate(next_x, next_y, 0); */
        /* Debug.Log("Player Position: X = " + playerObj.transform.position.x + " --- Y = " + playerObj.transform.position.y + " --- Z = " + 
         playerObj.transform.position.z); */
        /* 
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red); */

        updateCounter();

        transform.Rotate(new Vector3(0,0,1));
    }

    void changeColor(bool normal){
        if (normal){
            m_NewColor = new Color(1, 1, 1);
        }else{
            m_NewColor = new Color(30, 0, 0);
        }
        m_SpriteRenderer.color = m_NewColor;
    }

    void updateCounter(){
        counter = counter + 1;
        if (counter > 72){
            counter = 0;
            state = !state;
            changeColor(state);
        }
    }
}
