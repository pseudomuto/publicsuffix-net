//
// DomainTest.cs
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
	public class DomainTest
	{
		[TestCase]
		public void IsDomainReturnsFalseWhenEitherTLDOrSLDIsNull()
		{
			Assert.IsFalse(new Domain(null).IsDomain);
			Assert.IsFalse(new Domain("  ").IsDomain);
			Assert.IsFalse(new Domain("com").IsDomain);
			Assert.IsFalse(new Domain("", "google").IsDomain);
			Assert.IsFalse(new Domain("com", "    ").IsDomain);
		}

		[TestCase]
		public void IsDomainIsTrueWhenBothTLDAndSLDAreValid()
		{
			Assert.IsTrue(new Domain("com", "google").IsDomain);
			Assert.IsTrue(new Domain("com", "google", "www").IsDomain);
		}

		[TestCase]
		public void ToStringReturnsFQDN()
		{
			Assert.AreEqual("google.com", new Domain("com", "google").ToString());
			Assert.AreEqual("www.google.com", new Domain("com", "google", "www").ToString());
			Assert.AreEqual("a.b.c.google.com", new Domain("com", "google", "a.b.c").ToString());
		}
	}
}

