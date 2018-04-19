namespace MPX.Utilities.Converters {
    internal class PropertyExample {
        public PropertyExample(string propertyName, string propertyType, string propertyAccessModifier) {
            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
            this.PropertyAccessModifier = propertyAccessModifier;
        }

        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string PropertyAccessModifier { get; set; }
    }
}