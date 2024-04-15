using UnityEngine;

public class TogglePosition : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject; // The object to move

    private bool isMoved = false; // State tracking whether the object is at the right or left
    public float targetX = -695f; // Specific X position to move the object to
    private Vector3 originalPosition; // To store the original position of the object
    private Vector3 targetPosition; // Target position for moving

    public float speed = 5f; // Speed of the movement

    void Start()
    {
        // Store the original position of the object
        if (targetObject != null)
        {
            originalPosition = targetObject.transform.position;
            targetPosition = new Vector3(targetX, originalPosition.y, originalPosition.z);
        }
        else
        {
            Debug.LogError("Target object not assigned.");
        }
    }

    void Update()
    {
        // Continuously update the position if it's not already at the target or original position
        if (targetObject.transform.position != targetPosition && isMoved)
        {
            targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPosition, Time.deltaTime * speed);
        }
        else if (targetObject.transform.position != originalPosition && !isMoved)
        {
            targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, originalPosition, Time.deltaTime * speed);
        }
    }

    // Method to be called when the object needs to be toggled
    public void TogglePositionOnClick()
    {
        isMoved = !isMoved; // Toggle the state
    }
}
