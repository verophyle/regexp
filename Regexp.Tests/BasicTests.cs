// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Verophyle.Regexp.Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void Utils_Regexp_Basic_TestCat()
        {
            var re = new StringRegexp("ab");
            Assert.IsTrue(re.Matches("ab"));
            Assert.IsFalse(re.Matches("a"));
            Assert.IsFalse(re.Matches("b"));
            Assert.IsFalse(re.Matches(""));
            Assert.IsFalse(re.Matches("ba"));
        }

        [TestMethod]
        public void Utils_Regexp_Basic_TestOr()
        {
            var re = new StringRegexp("a|b");
            Assert.IsTrue(re.Matches("a"));
            Assert.IsTrue(re.Matches("b"));
            Assert.IsFalse(re.Matches(""));
            Assert.IsFalse(re.Matches("c"));
        }

        [TestMethod]
        public void Utils_Regexp_Basic_TestPlus()
        {
            var re = new StringRegexp("a+b");
            Assert.IsTrue(re.Matches("ab"));
            Assert.IsFalse(re.Matches("b"));
            Assert.IsFalse(re.Matches("a"));
            Assert.IsTrue(re.Matches("aab"));
            Assert.IsTrue(re.Matches("aaaab"));
            Assert.IsFalse(re.Matches("aaaaaaa"));
            Assert.IsFalse(re.Matches(""));
        }

        [TestMethod]
        public void Utils_Regexp_Basic_TestStar()
        {
            var re = new StringRegexp("a*b");
            Assert.IsTrue(re.Matches("ab"));
            Assert.IsTrue(re.Matches("b"));
            Assert.IsTrue(re.Matches("aaaaaab"));
            Assert.IsFalse(re.Matches(""));
            Assert.IsFalse(re.Matches("a"));
        }

        [TestMethod]
        public void Utils_Regexp_Basic_TestDot()
        {
            var re = new StringRegexp("a.b");
            Assert.IsTrue(re.Matches("azb"));
            Assert.IsFalse(re.Matches("ab"));
        }

        [TestMethod]
        public void Utils_Regexp_Basic_TestGroup()
        {
            var re = new StringRegexp("ab(cd|ef)gh");
            Assert.IsTrue(re.Matches("abcdgh"));
            Assert.IsTrue(re.Matches("abefgh"));
            Assert.IsFalse(re.Matches("abgh"));
            Assert.IsFalse(re.Matches(""));
        }
    }
}
