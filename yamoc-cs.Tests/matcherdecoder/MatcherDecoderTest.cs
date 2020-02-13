using NUnit.Framework;
using httpmock.requestmatcher;

namespace httpmock.matcherdecoder.Tests
{
    [TestFixture]
    public class MatcherDecoderTest
    {
        [Test]
        public void IsDecodable1() {
            string syntax = "a == \"b\"";
            IRequestMatcher expect = null;

            IRequestMatcher actual = MatcherDecoder.decode(syntax);

            Assert.That(actual, Is.EqualTo(expect));
        }
    }
}