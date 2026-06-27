using System;
using System.Linq;
using Xunit;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
    public int MethodWithParams(string name, int age) => age;
    private void PrivateMethod() { }
    public void Method2() { }
}
[Serializable]
public class AttributedClass { }
[Obsolete]
public class ObsoleteClass { }
public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods().ToList();

        Assert.Contains("Method", methods);
        Assert.Contains("MethodWithParams", methods);
        Assert.Contains("Method2", methods);
        Assert.DoesNotContain("PrivateMethod", methods);
    }
    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields().ToList();

        Assert.Contains("PublicField", fields);
        Assert.Contains("_privateField", fields);
    }
    [Fact]
    public void GetMethodParams_ReturnsCorrectParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var result = analyzer.GetMethodParams("MethodWithParams").ToList();

        Assert.Contains("String name", result);
        Assert.Contains("Int32 age", result);
    }
    [Fact]
    public void GetMethodParams_ForMethodWithoutParams_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var result = analyzer.GetMethodParams("Method").ToList();

        Assert.Empty(result);
    }
    [Fact]
    public void GetMethodParams_ForNonExistingMethod_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var result = analyzer.GetMethodParams("NonExistingMethod").ToList();

        Assert.Empty(result);
    }
    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties().ToList();

        Assert.Contains("Property", properties);
    }
    [Fact]
    public void HasAttribute_WhenClassHasAttribute_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }
    [Fact]
    public void HasAttribute_WhenClassDoesNotHaveAttribute_ReturnsFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
    [Fact]
    public void HasAttribute_WithObsoleteAttribute_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(ObsoleteClass));
        Assert.True(analyzer.HasAttribute<ObsoleteAttribute>());
    }
}