using NUnit.Framework;
using httpmock.stringEvaluator.oparator;
using httpmock.stringEvaluator.value;

namespace httpmock.stringEvaluator.decoder.Tests
{
    [TestFixture]
    public class MatcherDecoderTest
    {
        [Test]
        public void IsDecodable1() {
            string syntax = "a == \"b\"";
            FuzzyEqualEvaluator expect = new FuzzyEqualEvaluator();
            var left = new LiteralValueEvaluator();
            left.Value = "a";
            expect.Left = left;
            var right = new LiteralValueEvaluator();
            right.Value = "b";
            expect.Right = right;

            IStringEvaluator actual = EvaluatorDecoder.decode(syntax);

            Assert.AreEqual(actual, expect);
        }
    }
}