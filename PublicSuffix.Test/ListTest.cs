//
// ListTest.cs
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
using System.Linq;

namespace PublicSuffix.Test
{
	[TestFixture]
	public class ListTest
	{
		private readonly List _subject = new List();

		[TearDown]
		public void TearDown()
		{
			_subject.Clear();
		}

		[TestCase]
		public void DefaultListIsLoadedAutomatically()
		{
			Assert.Greater(List.DefaultList.Count, 0);
		}

		[TestCase]
		public void AddRuleInsertsRuleIntoTheList()
		{		
			AddRules("google.com");
			Assert.AreEqual(1, _subject.Count());
		}

		[TestCase]
		public void AddRuleIgnoresDuplicates()
		{
			AddRules("google.com", "google.com");
			Assert.AreEqual(1, _subject.Count());
		}

		[TestCase]
		public void GetMatchReturnsARuleWhenFound()
		{
			AddRules("google.com");
			Assert.IsNotNull(_subject.GetMatch("test.google.com"));
		}

		[TestCase]
		public void GetMatchUsesBestMatch()
		{
			var expected = Rule.Parse("test.british-library.uk");
			AddRules("british-library.uk", "*.british-library.uk", "test.british-library.uk");
			Assert.AreEqual(expected, _subject.GetMatch("test.british-library.uk"));
		}

		[TestCase]
		public void GetMatchPrioritizesExceptionRules()
		{
			var expected = Rule.Parse("!british-library.uk");

			AddRules("*.uk", "!british-library.uk", "test.british-library.uk");
			Assert.AreEqual(expected, _subject.GetMatch("british-library.uk"));
			Assert.AreEqual(expected, _subject.GetMatch("test.british-library.uk"));
		}

		[TestCase]
		public void GetMatchIsNullWhenNoRulesAreDefined()
		{
			AddRules("google.com");
			Assert.IsNull(_subject.GetMatch("test.twitter.com"));
		}

		[TestCase]
		public void GetMatchesReturnsAllMatchesForTheGivenHost()
		{
			AddRules("google.com", "*.google.com", "*.twitter.com");
			Assert.AreEqual(2, _subject.GetMatches("test.google.com").Count());
		}

		[TestCase]
		public void GetMatchesWhenNoRulesAreDefined()
		{
			CollectionAssert.IsEmpty(_subject.GetMatches("www.google.com"));
		}

		[TestCase]
		public void GetMatchesWithInvalidHosts()
		{
			AddRules("*.google.com");
			CollectionAssert.IsEmpty(_subject.GetMatches(null));
			CollectionAssert.IsEmpty(_subject.GetMatches(""));
			CollectionAssert.IsEmpty(_subject.GetMatches("   "));
			CollectionAssert.IsEmpty(_subject.GetMatches("https://www.google.com/"));
		}

		private void AddRules(params string[] definitions)
		{
			foreach (string def in definitions)
			{
				_subject.Add(Rule.Parse(def));
			}
		}
	}
}

