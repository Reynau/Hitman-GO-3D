using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picking : MonoBehaviour
{
    public Animator collectibleAnimController;

    public string collectiblePickTrigger = "isPicked";

    public void Pick()
    {
        if (collectibleAnimController != null)
        {
            collectibleAnimController.SetTrigger(collectiblePickTrigger);
        }
    }


}
