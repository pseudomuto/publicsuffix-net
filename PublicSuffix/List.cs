//
// List.cs
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
using System.Collections.Generic;
using System.Linq;

namespace PublicSuffix
{
	public class List : IEnumerable<Rule>
	{
		private List<Rule> _rules = new List<Rule>();

		public void AddRule(Rule rule)
		{
			if (!_rules.Contains(rule))
			{
				_rules.Add(rule);
			}
		}

		public void Clear()
		{
			_rules.Clear();
		}

		public Rule GetMatch(string host)
		{
			var matches = GetMatches(host).ToList();

			if (matches.Count == 0)
			{
				return null;
			}

			return GetExceptionMatch(matches) ?? GetLongestMatch(matches);
		}

		public IEnumerable<Rule> GetMatches(string host)
		{
			if (!string.IsNullOrWhiteSpace(host))
			{
				return _rules.Where(r => r.IsMatch(host));
			}

			return Enumerable.Empty<Rule>();
		}

		#region [IEnumerable implementation]

		public IEnumerator<Rule> GetEnumerator()
		{
			return _rules.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		private static Rule GetExceptionMatch(IEnumerable<Rule> rules)
		{
			return rules.FirstOrDefault(rule => rule.Type == "ExceptionRule");
		}

		private static Rule GetLongestMatch(IEnumerable<Rule> rules)
		{
			return rules.Aggregate((match, current) => match.Length > current.Length ? match : current);
		}
	}
}

