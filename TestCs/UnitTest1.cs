using NUnit.Framework;
using CbStyles.Parser;
using static System.Console;

namespace TestCs
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSpan1()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var s = new Span<int>(arr);
            var a = s[1..3];
            WriteLine($"{a}");
            Assert.AreEqual(a, new[] { 2, 3 });
        }

        [Test]
        public void TestSpan2()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var s = new Span<int>(arr);
            var a = s[..3];
            WriteLine($"{a}");
            Assert.AreEqual(a, new[] { 1, 2, 3 });
        }

        [Test]
        public void TestSpan3()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var s = new Span<int>(arr);
            var a = s[2..];
            WriteLine($"{a}");
            Assert.AreEqual(a, new[] { 3, 4, 5 });
        }

        [Test]
        public void TestSpan4()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var s = new Span<int>(arr);
            var a = s[1..^1];
            WriteLine($"{a}");
            Assert.AreEqual(a, new[] { 2, 3, 4 });
        }

        [Test]
        public void TestSpan5()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var s = new Span<int>(arr);
            var a = s[^5..1];
            WriteLine($"{a}");
            Assert.AreEqual(a, new[] { 1 });
        }

        [Test]
        public void TestSpan6()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            var s = new Span<int>(arr);
            var a = s[..];
            WriteLine($"{a}");
            Assert.AreEqual(a, s);
        }

        [Test]
        public void TestSpan7()
        {
            var arr = new[] { 1, 2, 3 };
            var s = new Span<int>(arr);
            var a = s.Get(3);
            WriteLine($"{a}");
            Assert.AreEqual(a, null);
        }

        [Test]
        public void TestSpan8()
        {
            var arr = new[] { 1, 2, 3 };
            var s = new Span<int>(arr);
            var a = s.Get(2);
            WriteLine($"{a}");
            Assert.AreEqual(a, 3);
        }
    }
}