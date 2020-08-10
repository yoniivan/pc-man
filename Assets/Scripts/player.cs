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
    private GameObject scoreBalls;

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
        if (other.name.Contains("scoreBalls"))
        {
            scoreBalls = other.gameObject;
            scoreBalls.SetActive(false);
            counter++;
            scoreBoard.text = counter.ToString();
        }

        
        
    }

    private void Movment()
    {
        float loc;

        if (Input.GetKey(KeyCode.LeftArrow)) // שמאל
        {
            loc = transform.position.x - (m_Speed * Time.deltaTime);
            v = new Vector3(loc, 5.5f, transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            loc = transform.position.x + (m_Speed * Time.deltaTime);
            v = new Vector3(loc, 5.5f, transform.position.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            loc = transform.localPosition.z + (m_Speed * Time.deltaTime);
            v = new Vector3(transform.position.x, 5.5f, loc);
            
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            loc = transform.localPosition.z - (m_Speed * Time.deltaTime);
            v = new Vector3(transform.position.x, 5.5f, loc);
        }

        rb.MovePosition(v);
    }
}
