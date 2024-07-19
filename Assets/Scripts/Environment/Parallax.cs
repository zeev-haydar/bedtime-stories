using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    float singleTextureWidth;
    public float scaleTexture = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();
    }

    // Update is called once per frame
    void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit * scaleTexture;
    }

    void Scroll(){
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0f, 0f);
    }

    void CheckReset(){
        if ( (Mathf.Abs(transform.position.x) - singleTextureWidth) > 0){
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        } 
    }

    void Update(){
        Scroll();
        CheckReset(); 
    }

    void NextWave(){
        moveSpeed = moveSpeed * 10f;
    }
}
