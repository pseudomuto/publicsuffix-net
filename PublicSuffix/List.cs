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
using System.IO;
using System.Reflection;

namespace PublicSuffix
{
	public class List : HashSet<Rule>
	{
		private static readonly string RESOURCE_NAME = "PublicSuffix.Data.public_suffix_list.dat";

		#region DefaultList Implementation

		private static Lazy<List> defaultList = new Lazy<List>(CreateDefaultList);
		private static string defaultDataFile = string.Empty;
		private static bool allowPrivateDomains = true;

		public static List DefaultList { get { return defaultList.Value; } }

		public static string DefaultDataFile
		{
			get { return defaultDataFile; }
			set
			{
				if (defaultDataFile != value)
				{
					defaultDataFile = value;
					defaultList = new Lazy<List>(CreateDefaultList);
				}
			}
		}

		public static bool AllowPrivateDomains
		{
			get { return allowPrivateDomains; }
			set
			{ 
				if (allowPrivateDomains != value)
				{
					allowPrivateDomains = value;
					defaultList = new Lazy<List>(CreateDefaultList);
				}
			}
		}

		#endregion

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
				return this.Where(rule => rule.IsMatch(host));
			}

			return Enumerable.Empty<Rule>();
		}

		private static Rule GetExceptionMatch(IEnumerable<Rule> rules)
		{
			return rules.FirstOrDefault(rule => rule.Type == "ExceptionRule");
		}

		private static Rule GetLongestMatch(IEnumerable<Rule> rules)
		{
			return rules.Aggregate((match, current) => match.Length > current.Length ? match : current);
		}

		private static List CreateDefaultList()
		{
			return string.IsNullOrWhiteSpace(DefaultDataFile) ? 
				CreateListFromEmbeddedDataFile() : 
				CreateListFromSuppliedDataFile();
		}

		private static List CreateListFromSuppliedDataFile()
		{
			using (var stream = new FileStream(DefaultDataFile, FileMode.Open))
			{
				return new ListParser().Parse(stream, AllowPrivateDomains);
			}
		}

		private static List CreateListFromEmbeddedDataFile()
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
			{
				return new ListParser().Parse(stream, AllowPrivateDomains);
			}
		}
	}
}
	