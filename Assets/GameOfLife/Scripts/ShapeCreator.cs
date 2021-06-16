using UnityEngine;

namespace GameOfLife.Scripts
{
    public class ShapeCreator : MonoBehaviour
    {
        public void CreateShape(Shape shape)
        {
            Debug.Log($"Creating Shape ({shape})");
        }

        public enum Shape
        {
            GosperGliderGun,
            GosperGliderEater
        }
    }
}