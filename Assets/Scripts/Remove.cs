using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : StateMachineBehaviour
{
    public float fadetime = 0.5f;           
    private float timeElapsed = 0f;        
    private SpriteRenderer spriteRenderer; 
    private GameObject objToRemove;     
    private Color startColor;            
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        timeElapsed = 0f;                   
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;

        float newAlpha = startColor.a * (1 - timeElapsed / fadetime);

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        if (timeElapsed > fadetime)
        {
            Destroy(objToRemove);
        }
    }
}
