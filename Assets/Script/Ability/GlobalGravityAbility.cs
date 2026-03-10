using UnityEngine;

public class GlobalGravityAbility : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode actionKey = KeyCode.Q;

    void Update()
    {
        if (Input.GetKeyDown(actionKey))
        {
            ChangeAllGravity();
        }
    }

    private void ChangeAllGravity()
    {
        // Find all GravityBox objects in the scene
        GravityBox[] boxes = Object.FindObjectsByType<GravityBox>(FindObjectsSortMode.None);
        
        if (boxes.Length == 0)
        {
            Debug.LogWarning("No GravityBox objects found in the scene.");
            return;
        }

        // Flip the gravity of every box found to its opposite
        foreach (GravityBox box in boxes)
        {
            box.FlipGravity();
        }

        Debug.Log($"Flipped gravity for {boxes.Length} boxes to their opposites across the map.");
    }
}
