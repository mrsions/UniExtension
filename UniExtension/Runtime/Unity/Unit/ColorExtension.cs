namespace UnityEngine
{
    public static class ColorExtension
    {
        public static Color SetR(this Color _this, float r)
        {
            _this.r = r;
            return _this;
        }
        public static Color SetG(this Color _this, float g)
        {
            _this.g = g;
            return _this;
        }
        public static Color SetB(this Color _this, float b)
        {
            _this.b = b;
            return _this;
        }
        public static Color SetAlpha(this Color _this, float a)
        {
            _this.a = a;
            return _this;
        }

        public static Color Normalize(this Color _this)
        {
            _this.a = Mathf.Clamp01(_this.a);
            _this.r = Mathf.Clamp01(_this.r);
            _this.g = Mathf.Clamp01(_this.g);
            _this.b = Mathf.Clamp01(_this.b);
            return _this;
        }
    }
}