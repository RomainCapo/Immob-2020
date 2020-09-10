/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private const string ANIM_OPEN = "isOpening";
    private const string ANIM_CLOSE = "isClosing";
    private Animator animator;
    private bool isOpen;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();

        //Allow to open the door in build mode
        if(!Application.isEditor)
         {
             Open();
             this.isOpen = true;
         }
        else
         {
             this.isOpen = false;
         }
    }

    public void Animate()
    {
        if (this.isOpen)
        {
            Close();
            isOpen = false;
        }
        else
        {
            Open();
            isOpen = true;
        }
    }

    public void Open()
    {
        animator.SetBool(ANIM_OPEN, true);
        animator.SetBool(ANIM_CLOSE, false);
    }

    public void Close()
    {
        animator.SetBool(ANIM_CLOSE, true);
        animator.SetBool(ANIM_OPEN, false);
    }
}
