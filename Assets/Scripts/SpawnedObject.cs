using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;
using System;

public class SpawnedObject : MonoBehaviour
{
    [SerializeField] private string _displayName;
    [SerializeField] private string _description;

    private int _number = -1;

    public InteractionManager Manager;
    public float transparency = 1.0f;
    public bool itsTimeToDie = false;

    public int ID;

    private GameObject another;
    public bool forceMoveBool = false;
    private Rigidbody rg;
    private MeshRenderer mr;
    private Vector2 dir;
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

    private void Awake()
    {
        rg = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe += OnSwiped;
    }

    private void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe -= OnSwiped;
    }

    private void OnSwiped(Lean.Touch.LeanFinger finger)
    {
        Debug.Log("WWW MANAGER SELECTED:" + Manager.SelectedObject.GetComponent<SpawnedObject>().ID);
        Debug.Log("WWW THIS:" + this.gameObject.GetComponent<SpawnedObject>().ID);
        if (Manager.SelectedObject.GetComponent<SpawnedObject>().ID != this.gameObject.GetComponent<SpawnedObject>().ID)
            return;

        Vector2 temp = finger.ScreenPosition - finger.StartScreenPosition;
        Debug.Log("WWW ATTENTION !!!! SWIPE " + "X: " + temp.x + " Y: " + temp.y);

        try
        {
            rg.AddForce(temp.x, temp.y, 0, ForceMode.Impulse);
        }
        catch (Exception ex)
        {
            Debug.Log("WWW" + ex.ToString());
            // some code that handles the exception
            throw ex;
        }

        Debug.Log("WWW SWIPE VELOC " + rg.velocity);
        Debug.Log("WWW SWIPE VELOC mafg " + rg.velocity.magnitude*100);
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
        if (collision.gameObject.tag == "SpawnedObject" && !itsTimeToDie)
        {
            Debug.Log("WWW COLLISION SpawnedObject");
            if(rg.velocity.magnitude > 0.0001) {
                Debug.Log("WWW its TIME TO DIE");
                itsTimeToDie = true;
                collision.gameObject.GetComponent<SpawnedObject>().itsTimeToDie = true;
            }
        }
    }

    Material ChengeTransparency(Material mat, float currentTransparency)
    {
        Material newMat = new Material(mat);
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, currentTransparency);
        newMat.color = newColor;
        Debug.Log("current transparency = " + currentTransparency);
        return newMat;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(itsTimeToDie)
        {
            transparency = transparency - 0.01f;
            Debug.Log("   R: " + mr.material.color.r + "   G: " + mr.material.color.g + "  B: " + mr.material.color.b + " A :" + mr.material.color.a);
            mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, transparency);
            if (transparency < 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }
}
