using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleGravity.Environment
{
    public class ColorButton : MonoBehaviour
    {
        public ColorType requiredColor;
        public ColorDatabase database;
        public UnityEvent onActivated;
        public UnityEvent onDeactivated;

        [Header("Visuals")]
        public MeshRenderer buttonRenderer;
        public Color activeColor = Color.green;
        public Color inactiveColor = Color.red;

        private List<ColorBox> boxesOnTop = new List<ColorBox>();
        private bool isActive = false;

        void Start()
        {
            UpdateVisuals(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            ColorBox box = other.GetComponent<ColorBox>();
            if (box != null && !boxesOnTop.Contains(box))
            {
                boxesOnTop.Add(box);
                CheckColor();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            ColorBox box = other.GetComponent<ColorBox>();
            if (box != null && boxesOnTop.Contains(box))
            {
                boxesOnTop.Remove(box);
                CheckColor();
            }
        }

        private void CheckColor()
        {
            if (database == null) return;

            // Collect all unique colors from boxes currently on the button
            List<ColorType> presentColors = boxesOnTop
                .Where(b => b.currentColor != null)
                .Select(b => b.currentColor.type)
                .ToList();

            // Use the database to get the resulting mixed color
            ColorType mixedColor = database.MixColors(presentColors);

            bool shouldBeActive = (mixedColor == requiredColor);

            if (shouldBeActive != isActive)
            {
                isActive = shouldBeActive;
                if (isActive)
                {
                    onActivated?.Invoke();
                }
                else
                {
                    onDeactivated?.Invoke();
                }
                UpdateVisuals(isActive);
            }
        }

        private void UpdateVisuals(bool active)
        {
            if (buttonRenderer == null) return;
            
            MaterialPropertyBlock prop = new MaterialPropertyBlock();
            Color c = active ? activeColor : inactiveColor;
            prop.SetColor("_BaseColor", c);
            prop.SetColor("_Color", c);
            buttonRenderer.SetPropertyBlock(prop);
        }
    }
}
