using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//using Moq;

using Transverse._Common.DebugTools;


namespace Transverse._Common.DebugTools.UnitTests;



[TestClass]
public class DebugTests
{
    private readonly SimpleObject simpleObjectWithCircularReference;

    class SimpleObject
    {
        public string? Name { get; set; }

        public uint Weight { get; set; }

        public SimpleObject? Autre { get; set; }
    }

    public DebugTests()
    {
        simpleObjectWithCircularReference = new SimpleObject()
        {
            Name = "PÉCORO",
            Weight = 80,
            Autre = new SimpleObject()
            {
                Name = "PÉCOROBis",
                Weight = 160
            }
        };

        var simpleObjectBis = new SimpleObject()
        {
            Name = "PÉCOROBis",
            Weight = 160,
            Autre = simpleObjectWithCircularReference
        };

        simpleObjectWithCircularReference.Autre = simpleObjectBis;

    }


    [TestMethod]
    public void GetSerializedData_WhenCircularReference_ShouldntLoop()
    {
        var result = Debug.GetSerializedData(simpleObjectWithCircularReference);
        var expected = "{\"Name\":\"PÉCORO\",\"Weight\":80,\"Autre\":{\"Name\":\"PÉCOROBis\",\"Weight\":160}}";

        expected.Should().Be(result);
    }

    [TestMethod]
    public void GetSerializedData2_WhenCircularReference_ShouldntLoop()
    {
        var result = Debug.GetSerializedData2(simpleObjectWithCircularReference);
        var expected = "{\"Name\":\"PÉCORO\",\"Weight\":80,\"Autre\":{\"Name\":\"PÉCOROBis\",\"Weight\":160,\"Autre\":null}}";

        expected.Should().Be(result);
    }

}
