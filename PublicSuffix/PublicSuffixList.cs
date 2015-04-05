﻿//
// PublicSuffix.cs
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
using System.Linq;
using System.Collections.Generic;

namespace PublicSuffix
{
	public static class PublicSuffixList
	{
		public static Domain Parse(string host, List list = null)
		{
			var rule = (list ?? List.DefaultList).GetMatch(host);
			EnsureValidHostForRule(host, rule);

			var parts = rule.Decompose(host);
			var hosts = new Stack<string>(parts.First().Split('.'));

			var tld = parts.Last();
			var sld = hosts.Count == 0 ? null : hosts.Pop();
			var sub = hosts.Count == 0 ? null : string.Join(".", hosts.Reverse());

			return new Domain(tld, sld, sub);
		}

		public static bool IsValid(string host, List list = null)
		{
			var rule = (list ?? List.DefaultList).GetMatch(host);
			return rule != null && rule.IsAllowed(host);
		}

		private static void EnsureValidHostForRule(string host, Rule rule)
		{
			if (rule == null)
			{
				throw new InvalidDomainException(host);
			} else if (!rule.IsAllowed(host))
			{
				throw new BlockedDomainException(host);
			}
		}
	}
}
