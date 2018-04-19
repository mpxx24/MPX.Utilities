using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MPX.Utilities.Converters;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
namespace MPX.Utilities.Tests.Converters
{
    [TestFixture]
    public class ApiModelToOneWithAttributesHelperFixture {
        [SetUp]
        public void SetUp() {
        }

        [Test]
        public void ConvertType_Works() {
            var result = ApiModelToOneWithAttributesHelper.ConvertType(typeof(TestType));

            foreach (var property in typeof(TestType).GetProperties()) {
                Assert.That(result.Contains($"[JsonProperty(\"{property.Name}\")]"));
                Assert.That(result.Contains($"{this.GetProperPropertyName(property.Name)}"));
            }

            //NOTE: just paste it manualy instead of copying methods from implementation
            Assert.That(result.Contains("double"));
            Assert.That(result.Contains("List<double>"));
            Assert.That(result.Contains("List<int>"));
            Assert.That(result.Contains("List<string>"));
            Assert.That(result.Contains("double?"));
            Assert.That(result.Contains("DateTime"));
            Assert.That(result.Contains("DateTime?"));
            Assert.That(result.Contains("TestType2"));
            Assert.That(result.Contains("List<TestType2>"));
        }

        private string GetProperPropertyName(string propName) {
            var nameParts = propName.Split('_');
            var sb = new StringBuilder();
            foreach (var part in nameParts) {
                sb.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(part));
            }
            return sb.ToString();
        }

        private class TestType {
            public double somedoublevalue { get; set; }
            public double another_double_value { get; set; }
            public List<double> list_of_double_elements { get; set; }
            public List<int> list_of_int_elements { get; set; }
            public List<string> list_of_string_elements { get; set; }
            public List<double> timbre { get; set; }
            public double? nullable_double { get; set; }
            public DateTime some_datetime { get; set; }
            public DateTime? nullable_datetime { get; set; }
            public TestType2 testtype2 { get; set; }
            public List<TestType2> List_of_testtype2 { get; set; }

        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestType2 {
            public Guid id { get; set; }
        }
    }
}
