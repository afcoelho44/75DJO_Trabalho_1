using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector3 GetMousePosition() {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    public void Ativar() { 
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);   
    }
}
