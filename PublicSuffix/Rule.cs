//
// Rule.cs
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
using System.Reflection;

namespace PublicSuffix
{
	public abstract class Rule
	{
		private static readonly IDictionary<string, ConstructorInfo> ClassMap;
		private static readonly ConstructorInfo DefaultRuleConstructor;

		public string Definition { get; private set; }

		public string Domain { get; private set; }

		public string[] Labels { get; private set; }

		public int Length { get { return Labels.Length; } }

		public string Type { get { return GetType().Name; } }

		static Rule()
		{
			var types = new Type[] { typeof(string) };
			ClassMap = new Dictionary<string, ConstructorInfo>();
			ClassMap.Add("!", typeof(ExceptionRule).GetConstructor(types));
			ClassMap.Add("*", typeof(WilcardRule).GetConstructor(types));

			DefaultRuleConstructor = typeof(NormalRule).GetConstructor(types);
		}

		protected internal Rule(string definition, string domain)
		{
			Definition = definition;
			Domain = domain;
			Labels = MakeLabels(domain);
		}

		public static Rule Parse(string hostname)
		{
			ConstructorInfo info = null;
			if (!ClassMap.TryGetValue(hostname.Substring(0, 1), out info))
			{
				info = DefaultRuleConstructor;
			}

			return info.Invoke(new object[] { hostname }) as Rule;
		}

		public virtual bool IsMatch(string hostname)
		{
			var hostLabels = MakeLabels(hostname);
			var index = 0;

			while (index < Labels.Length && index < hostLabels.Length && Labels[index] == hostLabels[index])
			{
				index++;
			}

			return Labels.Length - index == 0;
		}

		public override int GetHashCode()
		{
			return Definition.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Definition.Equals((obj as Rule).Definition);
		}

		public override string ToString()
		{
			return Definition;
		}

		private string[] MakeLabels(string host)
		{
			return host.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
		}
	}
}
