using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ClickPositionManager : MonoBehaviour

{

    //[Range(1f, 30f)
    private int shape = 0;
    private float size = 0.5f;

    private GameObject primitive;
    private float red = 0.8f, green = 0.8f, blue = 0.8f, timeToDestroy = 3f, Xcutoff;
    public Text mousePosition, redAmount, greenAmount, blueAmount, timerAmount, sizeAmount, animAmount;
    public GameObject prefab00, prefab01, prefab02, explosion;
    public Color paintedObjectColor, paintedObjectEmission; 

    [SerializeField]

    private float distance = 5f;
    private Vector3 clickPosition;



    private Vector3 lastClickPosition = Vector3.zero;
    //public Text lifeTime;
    private bool timedDestroyIsOn = true, isAnimTypeRandom, isAnimSpeedRandom; //isSpawnTypeRandom = false, isSpawTimeRandom = false;
    //[Range(-3f, 3f)]


    [SerializeField]
    // public float rotationAmount = 0f;
    [Range(0.0f, 2f)]

    private int animationState = 0;
    public Dropdown animDropDown;
    private float animationSpeed = 1f;

    //private float emissionStrength = 0.5f;
    private float opacityStrength = 0.5f;
    
   
    //public Dropdown animDropDown, shapeDropDown;
    public Clock clock;
    [SerializeField]
    [Range(0.0f, 2f)]
    private float emissionStrength = 0.5f;


    private void Update()

    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeAnimationState(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeAnimationState(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeAnimationState(2);

        if (Input.GetMouseButtonDown(0))

        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit; // dont have to assign this a the raycast will assign this procedurally


            if (Physics.Raycast(ray, out hit)) //export out the information to hit

            {
                //Destroy(hit.transform.gameObject);
                if (hit.transform.gameObject.layer == 11)
                {
                    hit.transform.parent.GetComponent<Clock>().UpdateTime(hit.transform.localEulerAngles.y);
                    Debug.Log(hit.transform.ToString());
                }
            }
        }
        Debug.Log((EventSystem.current.currentSelectedGameObject == null));

        if (Input.GetMouseButton(1) && (EventSystem.current.currentSelectedGameObject == null))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit; // dont have to assign this a the raycast will assign this procedurally

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == 12)
                {
                    Destroy(hit.transform.gameObject);
                    primitive = Instantiate(explosion, hit.transform.position, Quaternion.identity);
                    Destroy(primitive, 1f);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            lastClickPosition = Vector3.zero;
        }

        if ((Input.GetMouseButton(0)) && (EventSystem.current.currentSelectedGameObject == null) && (Input.mousePosition.x > 280f)) //left hold

        {
            //distance += distanceChang
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));
            switch (shape)
            {
                case 0:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    primitive = Instantiate(prefab00, clickPosition, Quaternion.identity);
                    break;
                case 1:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    primitive = Instantiate(prefab01, clickPosition, Quaternion.identity);
                    break;
                case 2:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    primitive = Instantiate(prefab02, clickPosition, Quaternion.identity);
                    break;
                default:
                    break;
            }
            if (lastClickPosition == Vector3.zero) primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f) * size, Random.Range(0.1f, 1f) * size, Random.Range(0.1f, 1f) * size);
            else
            {
                float x = Mathf.Clamp(Random.Range(size, size * 6f) * Mathf.Abs(lastClickPosition.x - clickPosition.x), .1f, size * 10f);
                float y = Mathf.Clamp(Random.Range(size, size * 6f) * Mathf.Abs(lastClickPosition.y - clickPosition.y), .1f, size * 10f);
                float z = (x + y) / 2f;
                primitive.transform.localScale = new Vector3(x, y, z);
            }

            //randomizing color and scales
            if (primitive.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStrength);
                primitive.GetComponent<Renderer>().material.color = paintedObjectColor;
                paintedObjectEmission = new Color(paintedObjectColor.r * emissionStrength, paintedObjectColor.g * emissionStrength, paintedObjectColor.b * emissionStrength);
                primitive.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
            }

            foreach(Transform child in primitive.transform)
            {
                if(child.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStrength);
                    child.gameObject.GetComponent<Renderer>().material.color = paintedObjectColor;
                    paintedObjectEmission = new Color(paintedObjectColor.r * emissionStrength, paintedObjectColor.g * emissionStrength, paintedObjectColor.b * emissionStrength);
                    child.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);

                }
            }

            //animation states
            if (primitive.GetComponent<Animator>() != null)
            {
                if (isAnimTypeRandom) animationState = (int)Random.Range(0f, 2.99f);
                primitive.GetComponent<Animator>().SetInteger("state", animationState);

                if (isAnimSpeedRandom) animationSpeed = Random.Range(0f, 1f);
                primitive.GetComponent<Animator>().speed = animationSpeed;
            }

           /* else { }
            primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));

            primitive.transform.position = clickPosition;

            primitive.GetComponent<Renderer>().material.color = new Vector4(Random.Range(0f, red), Random.Range(0f, green), Random.Range(0f, blue), 1f);*/
            primitive.transform.parent = this.transform;
            if (timedDestroyIsOn)
            {
                Destroy(primitive, timeToDestroy);
            }
            lastClickPosition = clickPosition;
        }
        mousePosition.text = "Mouse Position x: " + Input.mousePosition.x.ToString("F0") + ". y: " + Input.mousePosition.y.ToString("F0");
                //distance += distanceChange;


    }

    //shange animation state function
    public void ChangeAnimationState(int temp)
    {
        animationState = temp;
        animDropDown.value = animationState;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Animator>() != null)
            {
                child.gameObject.GetComponent<Animator>().SetInteger("state", animationState);
            }
        }
    }

    public void changeShape(int tempShape)
    {
        shape = tempShape;
    }
    /*
    public void changeRed(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.r = temp;
                paintedObjectColor = new Color(temp, paintedObjectColor.g, paintedObjectColor.b, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);   
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.r = temp;
                    paintedObjectColor = new Color(temp, paintedObjectColor.g, paintedObjectColor.b, paintedObjectColor.a);
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }

            }

        }

        red = temp;
        redAmount.text = (red * 100f).ToString("F0");

    }*/

    public void changeRed(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.r = temp;
                paintedObjectColor = new Color(temp,paintedObjectColor.g, paintedObjectColor.b, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.r = temp;
                    paintedObjectColor = new Color(temp, paintedObjectColor.g,  paintedObjectColor.b, paintedObjectColor.a);
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }

            }

        }

        red = temp;
        redAmount.text = (red * 100f).ToString("F0");
    }

    public void changeGreen(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor = new Color(paintedObjectColor.r,temp, paintedObjectColor.b, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor = new Color(paintedObjectColor.r, temp, paintedObjectColor.b, paintedObjectColor.a);
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }

            }

        }

        green = temp;
        greenAmount.text = (green * 100f).ToString("F0");
    }

    public void changeBlue(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor = new Color(paintedObjectColor.r, paintedObjectColor.g, temp, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor = new Color(paintedObjectColor.r, paintedObjectColor.g, temp, paintedObjectColor.a);
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }

            }

        }

        blue = temp;
        blueAmount.text = (blue * 100f).ToString("F0");
    }

    public void ChangeAnimationSpeed(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Animator>() != null)
            {
                child.gameObject.GetComponent<Animator>().speed = temp;
            }

        }
        animationSpeed = temp;
        animAmount.text = animationSpeed.ToString("f1") + "Sec";

    }

    //Randomizing animation Type
    public void ChangeAnimationTypeRandom(bool temp)
    {
        isAnimTypeRandom = temp;
    }

    //randomizing animation speed
    public void ChangeAnimationSpeedRandom(bool temp)
    {
        isAnimSpeedRandom = temp;
    }



    public void destroyObjects()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
            primitive = Instantiate(explosion, child.position, Quaternion.identity);
            Destroy(primitive, 1f);
        }

    }
  
    public void TogleTimedDestroy(bool timer)

    {
        timedDestroyIsOn = timer;
    }

    //size slider function
    public void ChangeSize(float temp)
    {
        foreach (Transform child in transform)
        {
            child.localScale = child.localScale * temp / size;
        }
        size = temp;
    }

    public void ChangeEmissionStrength(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectEmission = new Color(paintedObjectColor.r * temp, paintedObjectColor.g * temp, paintedObjectColor.b *temp);
                child.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectEmission = new Color(paintedObjectColor.r * temp, paintedObjectColor.g * temp, paintedObjectColor.b *temp);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
                }

            }

            
        }
        emissionStrength = temp;
    }

    // change opacity function
    public void ChangeOpacityStrength(float temp)
    {
        foreach(Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.a = temp;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.a = temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);

                }

            }

        }
        opacityStrength = temp;
    }
        

    public void changeTimeToDestroy(float temp)
    {
        timeToDestroy = temp;
        timerAmount.text = timeToDestroy.ToString("F0") + "Sec";
    }

}


