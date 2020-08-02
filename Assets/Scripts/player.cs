using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] float m_Speed = 5;

    public TextMesh scoreBoard;
    Rigidbody rb;

    private Vertex sphere;
    private string initCrossName;

    private Vector3 v;

    private int counter = 0; //MAX 93

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        v = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Movment();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Sphere"))
        {
            if(other.GetComponent<food>() != null)
            {
                other.GetComponent<food>().MoveFoodDown();
                counter++;
                scoreBoard.text = counter.ToString();
            }
        }

        if (other.name.Contains("Player"))
        {
            Debug.Log("Loose");
        }
    }

    private void Movment()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) // שמאל
        {
            float loc = transform.position.x - (m_Speed * Time.deltaTime);
            v = new Vector3(loc, 5.5f, transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            float loc = transform.position.x + (m_Speed * Time.deltaTime);
            v = new Vector3(loc, 5.5f, transform.position.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float loc = transform.localPosition.z + (m_Speed * Time.deltaTime);
            v = new Vector3(transform.position.x, 5.5f, loc);
            
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float loc = transform.localPosition.z - (m_Speed * Time.deltaTime);
            v = new Vector3(transform.position.x, 5.5f, loc);
        }

        rb.MovePosition(v);
    }
}
