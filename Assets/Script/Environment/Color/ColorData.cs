using UnityEngine;

namespace PuzzleGravity.Environment
{
    [CreateAssetMenu(fileName = "New Color Data", menuName = "Puzzle Gravity/Color Data")]
    public class ColorData : ScriptableObject
    {
        public ColorType type;
        public string displayName;
        public string description;
        public Color colorValue = Color.white;
    }
}
