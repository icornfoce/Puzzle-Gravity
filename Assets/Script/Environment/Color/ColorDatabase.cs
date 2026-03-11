using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleGravity.Environment
{
    [CreateAssetMenu(fileName = "Color Database", menuName = "Puzzle Gravity/Color Database")]
    public class ColorDatabase : ScriptableObject
    {
        public List<ColorData> colorLibrary;

        public ColorData GetColorData(ColorType type)
        {
            return colorLibrary.FirstOrDefault(c => c.type == type);
        }

        public ColorType MixColors(List<ColorType> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0) return ColorType.Black;

            // Remove duplicates and Black
            var distinctIngredients = ingredients.Distinct().Where(c => c != ColorType.Black).ToList();

            if (distinctIngredients.Count == 1) return distinctIngredients[0];

            if (distinctIngredients.Count == 2)
            {
                if (distinctIngredients.Contains(ColorType.Red) && distinctIngredients.Contains(ColorType.Blue)) return ColorType.Purple;
                if (distinctIngredients.Contains(ColorType.Blue) && distinctIngredients.Contains(ColorType.Yellow)) return ColorType.Green;
                if (distinctIngredients.Contains(ColorType.Red) && distinctIngredients.Contains(ColorType.Yellow)) return ColorType.Orange;
            }

            if (distinctIngredients.Count == 3)
            {
                if (distinctIngredients.Contains(ColorType.Red) && 
                    distinctIngredients.Contains(ColorType.Blue) && 
                    distinctIngredients.Contains(ColorType.Yellow)) 
                    return ColorType.White;
            }

            return ColorType.Black;
        }
    }
}
