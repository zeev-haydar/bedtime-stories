using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxController : MonoBehaviour
{
    BoxCollider attackBox;
    public GameObject attackBoxVisualizer;
    // Start is called before the first frame update
    void Start()
    {
        attackBox = GetComponent<BoxCollider>();
        attackBoxVisualizer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        attackBoxVisualizer.GetComponent<Collider>().isTrigger = true;
        attackBoxVisualizer.tag = tag; // Set tag to follow parent
        attackBoxVisualizer.transform.SetParent(transform); // Make the cube a child of the object with the BoxCollider
        attackBoxVisualizer.transform.localPosition = attackBox.center; // Set position relative to the BoxCollider
        attackBoxVisualizer.transform.localScale = attackBox.size; // Set scale to match the BoxCollider size
        attackBoxVisualizer.GetComponent<Renderer>().enabled = false; // Hide the cube renderer by default

        attackBoxVisualizer.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        attackBoxVisualizer.transform.localScale = attackBox.size; // Set scale to match the BoxCollider size
        attackBoxVisualizer.transform.localPosition = attackBox.center; // Set position relative to the BoxCollider
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    attackBoxVisualizer.GetComponent<Renderer>().enabled = !attackBoxVisualizer.GetComponent<Renderer>().enabled;
        //    print("HI!");
        //}
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow; // Set the color of the gizmo
    //    Gizmos.matrix = transform.localToWorldMatrix; // Use the GameObject's transform matrix
    //    if (attackBox != null)
    //    {
    //        // Draw the box collider using Gizmos
    //        Gizmos.DrawWireCube(attackBox.center, attackBox.size);
    //    }
    //}

}
