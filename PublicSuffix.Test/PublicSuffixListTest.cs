//
// PublicSuffixTest.cs
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
	public class PublicSuffixListTest
	{
		[TestCase]
		public void ParseDomainWithoutSubDomain()
		{
			var domain = PublicSuffixList.Parse("example.com");
			Assert.AreEqual("com", domain.TopLevelDomain);
			Assert.AreEqual("example", domain.SecondLevelDomain);
			Assert.IsTrue(string.IsNullOrEmpty(domain.SubDomain));

			domain = PublicSuffixList.Parse("example.co.uk");
			Assert.AreEqual("co.uk", domain.TopLevelDomain);
			Assert.AreEqual("example", domain.SecondLevelDomain);
			Assert.IsTrue(string.IsNullOrEmpty(domain.SubDomain));
		}

		[TestCase]
		public void ParseDomainWithSingleSubDomain()
		{
			var domain = PublicSuffixList.Parse("alpha.example.com");
			Assert.AreEqual("com", domain.TopLevelDomain);
			Assert.AreEqual("example", domain.SecondLevelDomain);
			Assert.AreEqual("alpha", domain.SubDomain);

			domain = PublicSuffixList.Parse("alpha.example.co.uk");
			Assert.AreEqual("co.uk", domain.TopLevelDomain);
			Assert.AreEqual("example", domain.SecondLevelDomain);
			Assert.AreEqual("alpha", domain.SubDomain);
		}

		[TestCase]
		public void ParseDomainWithMultipleSubDomain()
		{
			var domain = PublicSuffixList.Parse("alpha.beta.example.com");
			Assert.AreEqual("com", domain.TopLevelDomain);
			Assert.AreEqual("example", domain.SecondLevelDomain);
			Assert.AreEqual("alpha.beta", domain.SubDomain);

			domain = PublicSuffixList.Parse("alpha.beta.example.co.uk");
			Assert.AreEqual("co.uk", domain.TopLevelDomain);
			Assert.AreEqual("example", domain.SecondLevelDomain);
			Assert.AreEqual("alpha.beta", domain.SubDomain);
		}

		[TestCase]
		public void ParseDomainThrowsInvalidDomainException()
		{
			Assert.Throws(
				typeof(InvalidDomainException),
				() => PublicSuffixList.Parse("example.qqq")
			);
		}

		[TestCase]
		public void ParseDomainThrowsBlockedDomainException()
		{
			Assert.Throws(
				typeof(BlockedDomainException),
				() => PublicSuffixList.Parse("example.ke")
			);
		}
	}
}
