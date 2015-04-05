# PublicSuffix

PublicSuffix is a .NET domain name parser based on the [Public Suffix List].

[![Build
Status](https://travis-ci.org/pseudomuto/publicsuffix-net.svg?branch=master)](https://travis-ci.org/pseudomuto/publicsuffix-net)

[Public Suffix List]: https://publicsuffix.org/


## FAKE Commands

This project is configured to use [Fake] for builds. Because the package is installed under
_packages_, there is a simple wrapper script aptly named `fake`, that will ensure the package is
installed and forward arguments to the _Fake.exe_ file.

* Clean - `./fake Clean` cleans the build, test and deploy folders
* Build - `./fake`
* Build Tests - `./fake BuildTest`
* Run Tests - `./fake Test` (will trigger BuildTest)

[Fake]: http://fsharp.github.io/FAKE/
