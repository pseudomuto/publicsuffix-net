﻿//
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
	}
}
