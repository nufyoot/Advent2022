using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace Advent2022.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class StringResourceAttribute : DataAttribute
{
    public string Input { get; }
    public string ExpectedValue { get; }

    public StringResourceAttribute(string input, string expectedValue)
    {
        Input = input;
        ExpectedValue = expectedValue;
    }
    
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        return new[]
        {
            new object[] { new MemoryStream(Encoding.UTF8.GetBytes(Input)), ExpectedValue }
        };
    }
}