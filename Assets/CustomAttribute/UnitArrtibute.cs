using UnityEngine;

namespace MyCustomAttribute
{
    //custom atrr read only (sử dụng để ẩn chỉnh sửa trên inspector)
    //cách sử dụng [ReadOnly]
    public class ReadOnlyAttribute : PropertyAttribute { }

    //custom attr lable (chỉnh sửa tiêu đề của field)
    //cách sử dụng [Label("tiêu đề")]
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string label;
        public LabelAttribute(string label)
        {
            this.label = label;
        }
    }


    //custom attr Min max (Tạo MinMaxSlider cho field)
    //cách sử dụng [MinMax(float min, float max)] field phải có kiểu là Vector2
    public class MinMaxAttribute : PropertyAttribute
    {
        public readonly float min;
        public readonly float max;

        public MinMaxAttribute() { }
        public MinMaxAttribute(float min, float max = 0)
        {
            this.min = min;
            this.max = max;
        }
    }
}
