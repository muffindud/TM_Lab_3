using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Camera mainCamera;
    public CraftingManager craftingManager;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider)
            return;

        GameObject hitObj = rayHit.collider.gameObject;
        if (hitObj.tag == "Crafting")
        {
            craftingManager.Craft(hitObj.name);
        }
    }
}
