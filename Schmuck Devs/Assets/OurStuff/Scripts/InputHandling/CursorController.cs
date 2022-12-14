using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture = null;
    [SerializeField] private Texture2D cursorClickedTexture = null;
    [SerializeField] private LayerMask mouseColliderMask = 0;
    [SerializeField] private float cursorChangeSpeed = .25f;
	private MouseActionMapping myMouseActionMappingScript = null;
    private Camera theCamera = null;
    private Coroutine changeCursorIEnumeratorCo = null;
	void Awake()
    {
        if(cursorTexture == null || cursorClickedTexture == null)
        {//Check if any of the mouse cursors are null:
            Debug.Log("You Don Schmucked Up... You have not setup the cursorTexture reference and/or the cursorClickedTexture reference.");
        }
        theCamera = Camera.main;
		myMouseActionMappingScript = new MouseActionMapping();
        myMouseActionMappingScript.Enable();
        myMouseActionMappingScript.MainMouseActionMapping.LeftMouseClick.performed += clicked;
        Cursor.lockState = CursorLockMode.Confined;//Meaning to confine it to the window... Locked will center the cursor.
        changeCursor(false);
    }
    private void changeCursor(bool value)
    {
        if (value)
        {//Show Clickable Cursor:
            //Vector2 hotspot = new Vector2(cursorClickedTexture.width / 2, cursorClickedTexture.height / 2); if you want to take the middle of the texture
            //No need here because the point to click is the top left which is set by defualt in unity.
			Cursor.SetCursor(cursorClickedTexture, Vector2.zero, CursorMode.Auto);
            return;
        }
		//Show Regular Cursor:
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
	}
    //Controlls:
    private void clicked(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Ray ray = theCamera.ScreenPointToRay(myMouseActionMappingScript.MainMouseActionMapping.Position.ReadValue<Vector2>());
            RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
            {
				if(hit.transform.GetComponent<Interactables>() != null)
                {//Then It Is A ClickAble
                    if(hit.transform.GetComponent<Interactables>().doSomething())
                    {
                        if(changeCursorIEnumeratorCo != null)
                        {
                            StopCoroutine(changeCursorIEnumeratorCo);
                        }
						changeCursor(true);
                        StartCoroutine(changeCursorIEnumerator(false));
                        return;
					}
				}
			}
		}
        changeCursor(false);
    }
    private IEnumerator changeCursorIEnumerator(bool value)
    {
        yield return new WaitForSeconds(cursorChangeSpeed);
        changeCursor(value);
    }
}
