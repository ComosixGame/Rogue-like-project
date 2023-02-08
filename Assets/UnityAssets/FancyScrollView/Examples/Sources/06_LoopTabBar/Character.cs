using UnityEngine;

namespace FancyScrollView.Example06{
    public class Character : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;

        public int index;
        public int price;

        public void In(MovementDirection direction) => transition?.In(direction);

        public void Out(MovementDirection direction) => transition?.Out(direction);
    }
}
