using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

public class SpawnedObject : MonoBehaviour
{
    [SerializeField] private string _displayName;
    [SerializeField] private string _description;

    private int _number = -1;

    public float transparency = 1.0f;
    public bool itsTimeToDie = false;
    public string Name
    {
        get
        {
            if (_number >= 0)
            {
                return _displayName + " " + _number.ToString();
            }
            else
            {
                return _displayName;
            }
        }
    }
    
    public string Description
    {
        get
        {
            return _description;
        }
    }

    public void GiveNumber(int number)
    {
        _number = number;
    }


    public void ForceMove(Vector2 dir)
    {
        Rigidbody rg = this.GetComponent<Rigidbody>();
        this.GetComponent<Rigidbody>().AddForce(dir.x, dir.y, 0 , ForceMode.VelocityChange);
        this.GetComponent<Rigidbody>().AddForce(10, 10, 10 ,ForceMode.Force);
        this.GetComponent<Rigidbody>().AddForce(10, 10, 10, ForceMode.Acceleration);
        Debug.Log("WWW SWIPE " + "X: "+ dir.x +" Y: "+ dir.y);
        Debug.Log("WWW SWIPE VELOC " +rg.velocity);
        

    }


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("WWW Triger");
        if (other.gameObject.tag == "SpawnedObject")
        {
            Debug.Log("WWW Triger SpawnedObject");
            Debug.Log("WWW VELO =  " +other.GetComponent<Rigidbody>().velocity.magnitude);

            if (other.GetComponent<Collision>().relativeVelocity.magnitude > 2)
            {
                Debug.Log("WWW Triger VELOCITY");
                itsTimeToDie = true;
                other.gameObject.GetComponent<SpawnedObject>().itsTimeToDie = true;
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("WWW COLLISION");
        if (collision.gameObject.tag == "SpawnedObject")
        {
            Debug.Log("WWW COLLISION SpawnedObject");
            if (collision.relativeVelocity.magnitude > 2)
            {
                itsTimeToDie = true;
                collision.gameObject.GetComponent<SpawnedObject>().itsTimeToDie = true;
            }
        }
    }

    private void ChengeTransparency(Material mat, float currentTransparency)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, currentTransparency);
        mat.SetColor("_Color", newColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(itsTimeToDie)
        {
            transparency -= 0.01f;
            ChengeTransparency(this.GetComponent<Renderer>().material, transparency);

            if (transparency < 0.02f)
            {
                Destroy(this);
                transparency = 1.0f;
            }
        }
    }
}
