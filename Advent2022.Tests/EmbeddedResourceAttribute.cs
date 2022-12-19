using System.Reflection;
using Xunit.Sdk;

namespace Advent2022.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class EmbeddedResourceAttribute : DataAttribute
{
    public string FileName { get; }
    public string ExpectedValue { get; }

    public EmbeddedResourceAttribute(string fileName, string expectedValue)
    {
        FileName = fileName;
        ExpectedValue = expectedValue;
    }
    
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(FileName);

        if (stream == null)
        {
            throw new NullReferenceException("The stream to be used for input is null.");
        }

        return new[]
        {
            new object[] { stream, ExpectedValue }
        };
    }
}