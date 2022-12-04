using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "SpawnedObject")
        {
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
