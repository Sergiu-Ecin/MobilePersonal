using System.Collections.Generic;
using UnityEngine;

public class IngredientAction
{
    public GameObject Ingredient;
    public Vector3 PreviousPosition;
    public Transform PreviousParent;

    public IngredientAction(GameObject ingredient, Vector3 prevPos, Transform prevParent)
    {
        Ingredient = ingredient;
        PreviousPosition = prevPos;
        PreviousParent = prevParent;
    }
}