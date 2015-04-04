//
// NormalRuleTest.cs
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
	public class NormalRuleTest
	{
		[TestCase]
		public void Constructor()
		{
			var rule = Rule.Parse("pseudomuto.com");
			Assert.AreEqual("NormalRule", rule.Type);
			Assert.AreEqual("pseudomuto.com", rule.Definition);
			Assert.AreEqual("pseudomuto.com", rule.Domain);
			CollectionAssert.AreEqual(new string[] { "com", "pseudomuto" }, rule.Labels);
		}

		[TestCase]
		public void IsMatchWithSimpleHosts()
		{
			Assert.IsTrue(Rule.Parse("uk").IsMatch("example.uk"));		
			Assert.IsFalse(Rule.Parse("gk").IsMatch("example.uk"));
			Assert.IsFalse(Rule.Parse("example").IsMatch("example.uk"));

			Assert.IsTrue(Rule.Parse("uk").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("gk").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("co").IsMatch("example.co.uk"));

			Assert.IsTrue(Rule.Parse("co.uk").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("uk.co").IsMatch("example.co.uk"));
			Assert.IsFalse(Rule.Parse("go.uk").IsMatch("example.co.uk"));
		}

		[TestCase]
		public void IsMatchWithFQDN()
		{
			Assert.IsTrue(Rule.Parse("com").IsMatch("com."));
			Assert.IsTrue(Rule.Parse("com").IsMatch("example.com."));
			Assert.IsTrue(Rule.Parse("com").IsMatch("www.example.com."));
		}

		[TestCase]
		public void PartsSplitsDomain()
		{
			var hosts = new string[] { "com", "co.com", "mx.co.com" };

			foreach (var host in hosts)
			{
				CollectionAssert.AreEqual(host.Split('.'), Rule.Parse(host).Parts);
			}
		}

		[TestCase]
		public void LengthReturnsTheNumberOfParts()
		{
			Assert.AreEqual(1, Rule.Parse("com").Length);
			Assert.AreEqual(2, Rule.Parse("co.com").Length);
			Assert.AreEqual(3, Rule.Parse("mx.co.com").Length);
		}

		[TestCase]
		public void IsAllowedChecksToSeeIfTheHostIsValid()
		{
			var rule = Rule.Parse("com");
			Assert.AreEqual("NormalRule", rule.Type);

			Assert.IsFalse(rule.IsAllowed("com"));
			Assert.IsTrue(rule.IsAllowed("example.com"));
			Assert.IsTrue(rule.IsAllowed("www.example.com"));

			Assert.IsFalse(rule.IsAllowed("com."));
			Assert.IsTrue(rule.IsAllowed("example.com."));
			Assert.IsTrue(rule.IsAllowed("www.example.com."));
		}

		[TestCase]
		public void DecomposeBreaksDownHosts()
		{
			var expectations = new Dictionary<string, string[]> {
				{ "com", new string[] { string.Empty, string.Empty } },
				{ "example.com", new string[] { "example", "com" } },
				{ "foo.example.com", new string[] { "foo.example", "com" } },

				// FQDN
				{ "com.", new string[] { string.Empty, string.Empty } },
				{ "example.com.", new string[] { "example", "com" } },
				{ "foo.example.com.", new string[] { "foo.example", "com" } }
			};

			var rule = Rule.Parse("com");
			Assert.AreEqual("NormalRule", rule.Type);

			foreach (var host in expectations.Keys)
			{
				CollectionAssert.AreEqual(expectations[host], rule.Decompose(host));
			}
		}
	}
}

