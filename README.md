# PublicSuffix

PublicSuffix is a .NET domain name parser based on the [Public Suffix List].

[![Build
Status](https://travis-ci.org/pseudomuto/publicsuffix-net.svg?branch=master)](https://travis-ci.org/pseudomuto/publicsuffix-net)

[Public Suffix List]: https://publicsuffix.org/

## What is the Public Suffix List?

The Public Suffix List is a cross-vendor initiative to provide an accurate list of domain name
suffixes.

The Public Suffix List is an initiative of the Mozilla Project, but is maintained as a community
resource. It is available for use in any software, but was originally created to meet the needs of
browser manufacturers.

A "public suffix" is one under which Internet users can directly register names. Some examples of
public suffixes are ".com", ".co.uk" and "pvt.k12.wy.us". The Public Suffix List is a list of all
known public suffixes.

Source: <http://publicsuffix.org>

## Requirements

Currently `.NET 4.5` is required. Very likely the code here would work for previous versions. If
there is any interest, I'd love to see a PR for supporting earlier versions.

## Getting Started

PublicSuffix(List) is available on NuGet.

`Install-Package PublicSuffixList`

In Xamarin Studio you can find this option under the project's context menu: `Add | Add
NuGet Packages...*.`

## Usage

### Parsing Domains

Domains can easily be broken down into their top level, second level and sub domains.
`PublicSuffixList#Parse` will respect registry policies (as defined by the list).

```csharp
// without subdomain
var domain = PublicSuffixList.Parse("google.com");
Console.WriteLine(domain.TopLevelDomain)
# com
Console.WriteLine(domain.SecondLevelDomain);
# google

// with subdomain
domain = PublicSuffixList.Parse("www.pseudomuto.co.uk");
Console.WriteLine(domain.TopLevelDomain)
# co.uk
Console.WriteLine(domain.SecondLevelDomain);
# pseudomuto
Console.WriteLine(domain.SubDomain);
# www
```

### Validating Domains

Domains that are parsed with `PublicSuffix.Parse` are implicitly valid. However, if you're not 
interested in the details, you can simply call `PublicSuffix.IsValid` and get a quick response as to 
whether or not the domain is valid.

```csharp
if (PublicSuffixList.IsValid("www.google.com")) {
  // this code will run...
}

if (PublicSuffixList.IsValid("somedomain.ke")) {
  // this will not..
}
```

### Using Your Own List

Both `Parse` and `IsValid` support overloads that take a `List` object to be used. When not
specified, `List.DefaultList` is used.

While you can pass a list, it is recommended that you grab an updated copy of [the list](http://publicsuffix.org) regularly and
set `List.DefaultDataFile` to point to it. This will make `List.DefaultList` use the specified file
rather than the version bundled with this project.

```csharp
List.DefaultDataFile = <path to your file>;
```

### Support for Private Domains

Setting `List.AllowPrivateDomains` will cause the list to be reloaded with (or without) support for
private (non-ICANN) domains. It is `true` by default.

```csharp
Console.WriteLine(PublicSuffixList.Parse("muto.blogspot.com").TopLevelDomain);
# blogspot.com

List.AllowPrivateDomains = false
Console.WriteLine(PublicSuffixList.Parse("muto.blogspot.com").TopLevelDomain);
# com
```

## License

Copyright (c) 2015 PseudoMuto. This is free software distributed under the MIT license.
