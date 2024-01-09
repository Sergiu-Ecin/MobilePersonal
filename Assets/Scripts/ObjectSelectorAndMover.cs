using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectorAndMover : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private bool swipeDetected = false;
    public float checkDistance = 1.0f;
    public float moveDistance = 1.0f;
    public LayerMask ingredientLayer;
    private Stack<IngredientAction> actions = new Stack<IngredientAction>();

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                Ray ray = mainCamera.ScreenPointToRay(touchStart);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    selectedObject = hit.transform.gameObject;
                    swipeDetected = false;
                }
            }
            else if (touch.phase == TouchPhase.Ended && selectedObject != null)
            {
                touchEnd = touch.position;
                if (!swipeDetected)
                {
                    DetectSwipe();
                }
            }
        }
    }
    private void DetectSwipe()
    {
        float xDifference = touchEnd.x - touchStart.x;
        float yDifference = touchEnd.y - touchStart.y;
        float swipeThreshold = 100f;

        Vector3 direction = Vector3.zero;
        if (Mathf.Abs(xDifference) > Mathf.Abs(yDifference))
        {
            if (Mathf.Abs(xDifference) > swipeThreshold)
            {
                direction = xDifference > 0 ? Vector3.right : Vector3.left;
            }
        }
        else
        {
            if (Mathf.Abs(yDifference) > swipeThreshold)
            {
                direction = yDifference > 0 ? Vector3.forward : Vector3.back;
            }
        }

        if (direction != Vector3.zero)
        {
            if (CheckForIngredient(direction))
            {
                MoveAndRotateObject(direction);
            }
        }
    }

    private bool CheckForIngredient(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(selectedObject.transform.position, direction, out hit, checkDistance, ingredientLayer))
        {
            Ingredient ingredientBelow = hit.collider.GetComponent<Ingredient>();
            if (ingredientBelow != null)
            {
                float heightBelow = ingredientBelow.TotalHeight();

                Ingredient topIngredientOfStack = selectedObject.GetComponent<Ingredient>();
                float stackHeight = topIngredientOfStack != null ? topIngredientOfStack.TotalHeight() : 0;

                Vector3 newPosition = hit.collider.bounds.center + Vector3.up * (heightBelow + stackHeight - topIngredientOfStack.GetComponent<Collider>().bounds.size.y);
                MoveIngredient(selectedObject, newPosition, hit.transform);

                return true;
            }
        }
        return false;
    }


    private void MoveAndRotateObject(Vector3 direction)
    {
        selectedObject.transform.Rotate(0, 0, 180);

        selectedObject.transform.Translate(direction * moveDistance, Space.World);
    }

    public void FlipStack(GameObject topIngredient)
    {
        GameObject stackParent = new GameObject("IngredientStack");
        stackParent.transform.position = topIngredient.transform.position;

        while (topIngredient != null)
        {
            Ingredient ingredient = topIngredient.GetComponent<Ingredient>();
            if (ingredient == null) break;

            topIngredient.transform.SetParent(stackParent.transform);
            topIngredient = ingredient.transform.GetChild(0).gameObject;
        }

        stackParent.transform.DORotate(new Vector3(0, 0, 180), 0.5f).OnComplete(() =>
        {
            foreach (Transform child in stackParent.transform)
            {
                child.SetParent(null);
            }
            Destroy(stackParent);
        });
    }

    private void MoveIngredient(GameObject ingredient, Vector3 newPosition, Transform newParent)
    {
        actions.Push(new IngredientAction(ingredient, ingredient.transform.position, ingredient.transform.parent));

        ingredient.transform.position = newPosition;
        ingredient.transform.SetParent(newParent);
    }

    public void UndoLastAction()
    {
        if (actions.Count > 0)
        {
            IngredientAction lastAction = actions.Pop();
            lastAction.Ingredient.transform.position = lastAction.PreviousPosition;
            lastAction.Ingredient.transform.SetParent(lastAction.PreviousParent);
        }
    }

    public SandwichVictoryChecker victoryChecker;

    private void VictoryScriptChecker()
    {

        if (victoryChecker != null)
        {
            victoryChecker.CheckForVictory();
        }
        else
        {
            Debug.LogError("SandwichVictoryChecker non settato in ObjectSelectorAndMover");
        }
    }
}