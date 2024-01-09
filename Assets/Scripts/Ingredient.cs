using UnityEngine;
using System.Collections.Generic;

public class Ingredient : MonoBehaviour
{
    public float cumulativeHeight = 0f;

    public float TotalHeight()
    {
        float totalHeight = cumulativeHeight;
        Ingredient currentIngredient = this;

        while (currentIngredient != null)
        {
            totalHeight += currentIngredient.GetComponent<Collider>().bounds.size.y;
            if (currentIngredient.transform.childCount > 0)
            {
                currentIngredient = currentIngredient.transform.GetChild(0).GetComponent<Ingredient>();
            }
            else
            {
                currentIngredient = null;
            }
        }

        return totalHeight;
    }
}