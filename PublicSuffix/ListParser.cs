﻿//
// ListParser.cs
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
using System.IO;
using System.Text.RegularExpressions;

namespace PublicSuffix
{
	public class ListParser
	{
		private static readonly Regex COMMENT_PATTERN = new Regex(@"^\/\/");

		public List Parse(Stream dataStream)
		{
			using (var reader = new StreamReader(dataStream))
			{
				var line = string.Empty;
				var list = new List();

				while ((line = reader.ReadLine()) != null)
				{
					if (!IsBlankOrComment(line))
					{
						list.Add(Rule.Parse(line.Trim()));
					}
				}

				return list;
			}
		}

		private static bool IsBlankOrComment(string line)
		{
			return string.IsNullOrWhiteSpace(line) || COMMENT_PATTERN.IsMatch(line.TrimStart());
		}
	}
}

