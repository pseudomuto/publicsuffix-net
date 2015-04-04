//
// WilcardRuleTest.cs
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
	public class WilcardRuleTest
	{
		[TestCase]
		public void Constructor()
		{
			var rule = Rule.Parse("*.aichi.jp");
			Assert.AreEqual("WilcardRule", rule.Type);								
			Assert.AreEqual("*.aichi.jp", rule.Definition);
			Assert.AreEqual("aichi.jp", rule.Domain);
			CollectionAssert.AreEqual(new string[] { "jp", "aichi" }, rule.Labels);
		}

		[TestCase]
		public void IsMatchWithSimpleHosts()
		{
			Assert.IsTrue(Rule.Parse("*.uk").IsMatch("example.uk"));
			Assert.IsTrue(Rule.Parse("*.uk").IsMatch("example.co.uk"));
			Assert.IsTrue(Rule.Parse("*.co.uk").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("*.go.uk").IsMatch("example.co.uk"));
		}

		[TestCase]
		public void IsMatchWithFQDN()
		{
			Assert.IsTrue(Rule.Parse("*.uk").IsMatch("uk."));
			Assert.IsTrue(Rule.Parse("*.uk").IsMatch("co.uk."));
			Assert.IsTrue(Rule.Parse("*.uk").IsMatch("example.co.uk."));
			Assert.IsTrue(Rule.Parse("*.uk").IsMatch("www.example.co.uk."));
		}

		[TestCase]
		public void PartsSplitsDomainAppropriately()
		{
			var expectations = new Dictionary<string, string[]> {
				{ "*.uk", new string[] { "uk" } },
				{ "*.co.uk", new string[] { "co", "uk" } }
			};

			foreach (var host in expectations.Keys)
			{
				CollectionAssert.AreEqual(expectations[host], Rule.Parse(host).Parts);
			}
		}

		[TestCase]
		public void LengthReturnsTheNumberOfPartsPlusOne()
		{
			Assert.AreEqual(2, Rule.Parse("*.com").Length);
			Assert.AreEqual(3, Rule.Parse("*.co.com").Length);
			Assert.AreEqual(4, Rule.Parse("*.mx.co.com").Length);
		}

		[TestCase]
		public void IsAllowedChecksToSeeIfTheHostIsValid()
		{
			var rule = Rule.Parse("*.uk");
			Assert.AreEqual("WilcardRule", rule.Type);

			Assert.IsFalse(rule.IsAllowed("uk"));
			Assert.IsFalse(rule.IsAllowed("co.uk"));
			Assert.IsTrue(rule.IsAllowed("example.co.uk"));
			Assert.IsTrue(rule.IsAllowed("www.example.co.uk"));

			Assert.IsFalse(rule.IsAllowed("uk."));
			Assert.IsFalse(rule.IsAllowed("co.uk."));
			Assert.IsTrue(rule.IsAllowed("example.co.uk."));
			Assert.IsTrue(rule.IsAllowed("www.example.co.uk."));
		}

		[TestCase]
		public void DecomposeBreaksDownHosts()
		{
			var expectations = new Dictionary<string, string[]> {
				{ "nic.uk", new string[] { string.Empty, string.Empty } },
				{ "google.co.uk", new string[] { "google", "co.uk" } },
				{ "foo.google.co.uk", new string[] { "foo.google", "co.uk" } },

				// FQDN
				{ "nic.uk.", new string[] { string.Empty, string.Empty } },
				{ "google.co.uk.", new string[] { "google", "co.uk" } },
				{ "foo.google.co.uk.", new string[] { "foo.google", "co.uk" } }
			};

			var rule = Rule.Parse("*.uk");
			Assert.AreEqual("WilcardRule", rule.Type);

			foreach (var host in expectations.Keys)
			{
				CollectionAssert.AreEqual(expectations[host], rule.Decompose(host));
			}
		}
	}
}

