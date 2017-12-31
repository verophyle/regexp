// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Verophyle.Regexp.Tests
{
    [TestClass]
    public class CharSetTests
    {
        [TestMethod]
        public void CharSet_Group()
        {
            var re = new StringRegexp("a[bc]d");
            Assert.IsTrue(re.Matches("abd"));
            Assert.IsTrue(re.Matches("acd"));
            Assert.IsFalse(re.Matches("ad"));
            Assert.IsFalse(re.Matches("azd"));
        }

        [TestMethod]
        public void CharSet_NegGroup()
        {
            var re = new StringRegexp("a[^bc]d");
            Assert.IsTrue(re.Matches("azd"));
            Assert.IsFalse(re.Matches("abd"));
            Assert.IsFalse(re.Matches("acd"));
            Assert.IsFalse(re.Matches("ad"));
        }

        [TestMethod]
        public void CharSet_UnicodeClass()
        {
            var re = new StringRegexp(@"\p{Ll}+");
            Assert.IsTrue(re.Matches("abc"));
            Assert.IsFalse(re.Matches("123"));
        }

        [TestMethod]
        public void CharSet_UnicodeClassGroup()
        {
            var re = new StringRegexp(@"[\p{Ll}\p{Nd}]+");
            Assert.IsTrue(re.Matches("a1b2"));
            Assert.IsFalse(re.Matches(" . ,"));
        }

        [TestMethod]
        public void CharSet_Hexadecimal()
        {
            var re = new StringRegexp(@"\x{20}");
            Assert.IsTrue(re.Matches(" "));
            Assert.IsFalse(re.Matches(""));
            Assert.IsFalse(re.Matches("20"));
        }

        [TestMethod]
        public void CharSet_WordToken()
        {
            var re = new StringRegexp(@"[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Mn}\p{Mc}\p{Me}]+('(m|re|s|t|d|ve|ll))?");
            Assert.IsTrue(re.Matches("one"));
            Assert.IsTrue(re.Matches("two'll"));
            Assert.IsFalse(re.Matches("123"));
        }

        [TestMethod]
        public void CharSet_PlusMinus()
        {
            var re = new StringRegexp(@"[+-]?([0-9]|_)+");
            Assert.IsTrue(re.Matches("-123"));
            Assert.IsTrue(re.Matches("+456"));
            Assert.IsTrue(re.Matches("789"));
            Assert.IsTrue(re.Matches("123_456"));
            Assert.IsFalse(re.Matches(""));
            Assert.IsFalse(re.Matches("asdf"));
        }

        [TestMethod]
        public void CharSet_NotNewLine()
        {
            var re = new StringRegexp(@"[^\r\n]+");
            var input = "Hello!\n";
            int i = 0, last = -1;
            do
            {
                re.ProcessInput(input[i]);
                if (re.Succeeded)
                    last = i;
            }
            while (++i < input.Length && !re.Failed);
            Assert.AreEqual(7, i);
            Assert.AreEqual(5, last);
        }
    }
}
