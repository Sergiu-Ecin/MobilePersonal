using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichVictoryChecker : MonoBehaviour
{
    public GameObject topOfStack;

    public void CheckForVictory()
    {
        if (topOfStack == null)
        {
            Debug.LogError("Top of stack non settato in SandwichVictoryChecker");
            return;
        }

        Ingredient topIngredient = topOfStack.GetComponent<Ingredient>();
        Ingredient bottomIngredient = FindBottomIngredient(topOfStack);

        if (topIngredient != null && bottomIngredient != null &&
            IsBread(topIngredient) && IsBread(bottomIngredient))
        {
            ShowVictoryScreen();
        }
    }

    private Ingredient FindBottomIngredient(GameObject ingredient)
    {
        Ingredient currentIngredient = ingredient.GetComponent<Ingredient>();
        while (currentIngredient.transform.parent != null)
        {
            Ingredient parentIngredient = currentIngredient.transform.parent.GetComponent<Ingredient>();
            if (parentIngredient != null)
            {
                currentIngredient = parentIngredient;
            }
            else
            {
                break;
            }
        }
        return currentIngredient;
    }

    private bool IsBread(Ingredient ingredient)
    {
        return ingredient.tag == "Bread";
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("Vittoria! Magnate er panino!");
    }
}