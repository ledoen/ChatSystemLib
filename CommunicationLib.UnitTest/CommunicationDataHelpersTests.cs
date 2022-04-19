using CommunicationLib.Common;
using NUnit.Framework;

namespace CommunicationLib.UnitTest
{
    public class CommunicationDataHelpersTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("a","a<EOF>")]
        [TestCase(" "," <EOF>")]
        [TestCase("","<EOF>")]
        [TestCase(null,"<EOF>")]
        public void AddTail_WhenCalled_AddTailToAString(string? input, string expectedResult)
        {
            var res = CommunictionDataHelpers.AddTail(input);
            Assert.That(res, Is.EqualTo(expectedResult));
        }
        [Test]
        [TestCase("a<EOF>", "a")]
        [TestCase("a<EOF><EOF>", "a")]
        [TestCase("a<EOF >", "a<EOF >")]
        [TestCase("a", "a")]
        [TestCase("<EOF>", "")]
        [TestCase(" <EOF>", " ")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void RemovedTail_InputStringWithATail_RemovedTail(string? input, string expectedResult)
        {
            var res = CommunictionDataHelpers.RemoveTail(input);

            Assert.That(res, Is.EqualTo(expectedResult));
        }
    }
}