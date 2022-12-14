using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    [Header("The InteractionScript Reference")]
    [SerializeField] private InteractionHandler myInteractionHandlerScriptReference;
    private void Awake()
    {
        if(myInteractionHandlerScriptReference == null)
        {//Show some Debug stuff if things are not setup correctly in the inspector window inside of Unity.
            Debug.Log("I cannot believe that I have schmucked up this bad... Assign the InteractionHandler obj to the Interactable Manager Script Component.");
        }
    }
}
