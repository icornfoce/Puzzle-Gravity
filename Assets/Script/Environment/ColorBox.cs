using UnityEngine;

namespace PuzzleGravity.Environment
{
    public class ColorBox : MonoBehaviour
    {
        public ColorData currentColor;
        public ColorDatabase database;
        
        private MeshRenderer meshRenderer;
        private MaterialPropertyBlock propertyBlock;

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            propertyBlock = new MaterialPropertyBlock();
        }

        void Start()
        {
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            if (currentColor == null || meshRenderer == null) return;

            propertyBlock.SetColor("_BaseColor", currentColor.colorValue);
            // Some shaders use _Color instead of _BaseColor
            propertyBlock.SetColor("_Color", currentColor.colorValue);
            
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        public void SetColor(ColorType type)
        {
            if (database == null) return;
            currentColor = database.GetColorData(type);
            UpdateVisuals();
        }
    }
}
