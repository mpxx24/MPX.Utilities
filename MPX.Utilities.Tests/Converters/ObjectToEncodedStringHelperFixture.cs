using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using MPX.Utilities.Converters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MPX.Utilities.Tests.Converters {
    [TestFixture]
    public class ObjectToEncodedStringHelperFixture {
        private ObjectToEncodedStringHelperModel testObject;

        [SetUp]
        public void SetUp() {
            this.testObject = new ObjectToEncodedStringHelperModel {
                SomeBooleanValue = false,
                SomeIntValue = 24,
                SomeStringValue = "let's test",
                SomeEnumerableOfValues = new List<int> {24, 54, 372, 1, 1251, 2},
                SomeEnumerableOfStrings = new List<string> {"some key", "another key", "oh, whats this!? yet another key"}
            };
        }

        [Test]
        public void ConvertToUrlEncodedString_Works() {
            var testResult = this.testObject.ConvertToUrlEncodedString();

            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.Not.Empty);

            foreach (var customAttribute in this.testObject.GetType().GetCustomAttributes(true)) {
                if (customAttribute is JsonPropertyAttribute jsonPropAttribute) {
                    Assert.That(testResult.Contains(jsonPropAttribute.PropertyName));
                }
            }

            Assert.That(testResult.Contains($"={this.testObject.SomeBooleanValue}"));
            Assert.That(testResult.Contains($"={this.testObject.SomeIntValue}"));

            this.AssertThatValueIsInTheResultString(this.testObject.SomeStringValue, testResult);

            this.AssertThatValuesFromEnumerableAreInTheResultString(this.testObject.SomeEnumerableOfStrings, testResult);
            this.AssertThatValuesFromEnumerableAreInTheResultString(this.testObject.SomeEnumerableOfValues, testResult);
        }

        private void AssertThatValueIsInTheResultString(object valueToCheck, string resultString) {
            var enumerableValuesToTest = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>(this.GetPropertynameInJsonPropertyAttribute(valueToCheck), valueToCheck.ToString())
            };

            AssertResultContainsString(resultString, enumerableValuesToTest);
        }

        private void AssertThatValuesFromEnumerableAreInTheResultString(IEnumerable enumerable, string resultString) {
            var enumerableValuesToTest = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>(this.GetPropertynameInJsonPropertyAttribute(enumerable), GetEnumerableElementsAsString(enumerable))
            };

            AssertResultContainsString(resultString, enumerableValuesToTest);
        }

        private static void AssertResultContainsString(string resultString, List<KeyValuePair<string, string>> enumerableValuesToTest) {
            using (var content = new FormUrlEncodedContent(enumerableValuesToTest)) {
                var encodedString = content.ReadAsStringAsync().Result;
                Assert.That(resultString.Contains(encodedString));
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

        private string GetPropertynameInJsonPropertyAttribute(object o) {
            return (o.GetType().GetCustomAttributes(true).First() as JsonPropertyAttribute)?.PropertyName;
        }

        private class ObjectToEncodedStringHelperModel {
            [JsonProperty("some_int_value")]
            public int SomeIntValue { get; set; }

            [JsonProperty("some_string_value")]
            public string SomeStringValue { get; set; }

            [JsonProperty("some_boolean_value")]
            public bool SomeBooleanValue { get; set; }

            [JsonProperty("some_enumerable_of_values")]
            public IEnumerable<int> SomeEnumerableOfValues { get; set; }

            [JsonProperty("some_enumerable_of_string")]
            public IEnumerable<string> SomeEnumerableOfStrings { get; set; }
        }
    }
}