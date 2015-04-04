//
// ExceptionRuleTest.cs
//
// Author:
//       PseudoMuto <david.muto@gmail.com>
//
// Copyright (c) 2015 PseudoMuto
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace PublicSuffix.Test
{
	[TestFixture]
	public class ExceptionRuleTest
	{
		[TestCase]
		public void Constructor()
		{
			var rule = Rule.Parse("!british-library.uk");
			Assert.AreEqual("ExceptionRule", rule.Type);
			Assert.AreEqual("!british-library.uk", rule.Definition);
			Assert.AreEqual("british-library.uk", rule.Domain);
			CollectionAssert.AreEqual(new string[] { "uk", "british-library" }, rule.Labels);
		}

		[TestCase]
		public void IsMatchWithSimpleHosts()
		{
			Assert.IsTrue(Rule.Parse("!uk").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("!gk").IsMatch("example.co.uk"));
			Assert.IsTrue(Rule.Parse("!co.uk").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("!go.uk").IsMatch("example.co.uk"));
			Assert.IsTrue(Rule.Parse("!british-library.uk").IsMatch("british-library.uk"));
			Assert.IsFalse(Rule.Parse("!british-library.uk").IsMatch("example.co.uk"));
		}

		[TestCase]
		public void IsMatchWithFQDN()
		{
			Assert.IsTrue(Rule.Parse("!uk").IsMatch("uk."));
			Assert.IsTrue(Rule.Parse("!uk").IsMatch("co.uk."));
			Assert.IsTrue(Rule.Parse("!uk").IsMatch("example.co.uk."));
			Assert.IsTrue(Rule.Parse("!uk").IsMatch("www.example.co.uk."));
		}

		[TestCase]
		public void PartsSplitsDomainAppropriately()
		{
			var expectations = new Dictionary<string, string[]> {
				{ "!british-library.uk", new string[] { "uk" } },
				{ "!metro.tokyo.jp", new string[] { "tokyo", "jp" } }
			};

			foreach (var host in expectations.Keys)
			{
				CollectionAssert.AreEqual(expectations[host], Rule.Parse(host).Parts);
			}
		}

		[TestCase]
		public void LengthReturnsTheNumberOfParts()
		{
			Assert.AreEqual(1, Rule.Parse("!british-library.uk").Length);
			Assert.AreEqual(2, Rule.Parse("!foo.british-library.uk").Length);
		}

		[TestCase]
		public void IsAllowedChecksToSeeIfTheHostIsValid()
		{
			var rule = Rule.Parse("!british-library.uk");
			Assert.AreEqual("ExceptionRule", rule.Type);

			Assert.IsFalse(rule.IsAllowed("uk"));
			Assert.IsTrue(rule.IsAllowed("british-library.uk"));
			Assert.IsTrue(rule.IsAllowed("www.british-library.uk"));

			Assert.IsFalse(rule.IsAllowed("uk."));
			Assert.IsTrue(rule.IsAllowed("british-library.uk."));
			Assert.IsTrue(rule.IsAllowed("www.british-library.uk."));
		}

		[TestCase]
		public void DecomposeBreaksDownHosts()
		{
			var expectations = new Dictionary<string, string[]> {
				{ "uk", new string[] { string.Empty, string.Empty } },
				{ "british-library.uk", new string[] { "british-library", "uk" } },
				{ "foo.british-library.uk", new string[] { "foo.british-library", "uk" } },

				// FQDN
				{ "uk.", new string[] { string.Empty, string.Empty } },
				{ "british-library.uk.", new string[] { "british-library", "uk" } },
				{ "foo.british-library.uk.", new string[] { "foo.british-library", "uk" } }
			};

			var rule = Rule.Parse("!british-library.uk");
			Assert.AreEqual("ExceptionRule", rule.Type);

			foreach (var host in expectations.Keys)
			{
				CollectionAssert.AreEqual(expectations[host], rule.Decompose(host));
			}
		}
	}
}

