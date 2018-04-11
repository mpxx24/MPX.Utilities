using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MPX.Utilities.Converters {
    public static class ObjectToEncodedStringHelper {
        public static string ConvertToUrlEncodedString(this object parameters) {
            var result = new Dictionary<string, string>();
            if (parameters == null) {
                return string.Empty;
            }

            var parametersType = parameters.GetType();
            foreach (var prop in parametersType.GetProperties()) {
                string propValueAsString;

                if (prop.CustomAttributes.Any()) {
                    foreach (var customAttribute in prop.GetCustomAttributes(true)) {
                        if (prop.GetValue(parameters, null) is IEnumerable && prop.PropertyType.IsGenericType) {
                            propValueAsString = GetEnumerableElementsAsString((IEnumerable) prop.GetValue(parameters, null));
                        }
                        else {
                            propValueAsString = prop.GetValue(parameters, null)?.ToString();
                        }

                        if (string.IsNullOrEmpty(propValueAsString)) {
                            continue;
                        }

                        if (customAttribute is JsonPropertyAttribute jsonPropAttribute) {
                            result.Add(jsonPropAttribute.PropertyName, propValueAsString);
                        }
                        else {
                            result.Add(prop.Name, propValueAsString);
                        }
                    }
                }
                else {
                    propValueAsString = prop.GetValue(parameters, null)?.ToString();
                    result.Add(prop.Name, propValueAsString);
                }
            }

            using (var content = new FormUrlEncodedContent(result)) {
                return content.ReadAsStringAsync().Result;
            }
        }

        private static string GetEnumerableElementsAsString(IEnumerable enumerable) {
            var sb = new StringBuilder();
            foreach (var elem in enumerable) {
                sb.Append($"{elem.ToString()},");
            }

            if (sb.Length >= 1) {
                sb.Length -= 1;
            }
            return sb.ToString();
        }
    }
}