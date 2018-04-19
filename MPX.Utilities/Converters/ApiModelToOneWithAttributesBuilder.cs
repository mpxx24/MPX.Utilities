using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MPX.Utilities.Converters {
    internal class ApiModelToOneWithAttributesBuilder {
        private readonly string objectAccessModifier;
        private readonly string objectName;
        private readonly string objectType;
        private readonly List<PropertyExample> properties = new List<PropertyExample>();

        public ApiModelToOneWithAttributesBuilder(string objectType, string objectName, string objectAccessModifier = "public") {
            this.objectType = objectType;
            this.objectName = objectName;
            this.objectAccessModifier = objectAccessModifier;
        }

        public ApiModelToOneWithAttributesBuilder AddProperty(string propertyType, string propertyName, string accessModifier = "public") {
            this.properties.Add(new PropertyExample(propertyName, propertyType, accessModifier));
            return this;
        }

        public string GetObjectAsString() {
            var sb = new StringBuilder();

            sb.AppendLine($"{this.objectAccessModifier} {this.objectType} {this.objectName} {{");

            for (var index = 0; index < this.properties.Count; index++) {
                var propertyExample = this.properties[index];
                sb.AppendLine($"\t[JsonProperty(\"{propertyExample.PropertyName}\")]");
                sb.AppendLine($"\t{propertyExample.PropertyAccessModifier} {propertyExample.PropertyType} {this.GetProperPropertyName(propertyExample.PropertyName)} {{get; set;}}");
                if (index != this.properties.Count - 1) {
                    sb.AppendLine();
                }
            }
            sb.Append("}");

            return sb.ToString();
        }

        private string GetProperPropertyName(string propName) {
            var nameParts = propName.Split('_');
            var sb = new StringBuilder();
            foreach (var part in nameParts) {
                sb.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(part));
            }
            return sb.ToString();
        }
    }
}