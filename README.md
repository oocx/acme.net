# Oocx.ACME - an ACME protocol library and simple Let's Encrypt client

[![Build status](https://ci.appveyor.com/api/projects/status/igpc0c9u9sxresij?svg=true)](https://ci.appveyor.com/project/oocx/acme-net)

This repository contains a library that can be used to develop ACME / Let's Encrypt clients.

Requesting and installing a a new SSL certificate can be as simple as this:

```
acme.exe -d www.example.com -a
```

That's all you need to do to request and install a free SSL certificate from Let's Encrypt!

**This project is work in progress. It works, but probably still has many bugs and needs more testing.**

I created this project as a training excercise and to learn about ACME and related 
technologies (certificate file formats, ASN1, ...). This is not intended to be a finished 
and ready to use product. However, I thought it might be useful or interesting for other 
people as well.

A major difference to other ACME .net clients is that this project does not have a 
dependency on OpenSSL (mainly because I wanted to figure out if I could implement this
project without OpenSSL, and because it provided an opportunity to learn more about certificate
file formats and ASN1).

If you are just looking for a Let's Encrypt client or a more mature project, then you should
take a look at these projects:

[.net ACME protocol library](https://github.com/ebekker/letsencrypt-win/).
[A simple ACME Client for Windows](https://github.com/Lone-Coder/letsencrypt-win-simple)

# Using acme.exe

You can use acme.exe with or without IIS integration. With IIS integration, acme.exe autoamtically
configures your IIS to respond to the ACME domain validation challenge, and it updates your IIS
web site with the new SSL certificate. To use IIS integration, you must run acme.exe on your IIS web 
server.

Examples:

Request a certificate for www.yourdomain.com and accept the terms of service of the ACME server (-a), 
using your@email.com as registration contact information (-m):

```
acme.exe -a www.yourdomain.com -m mailto:your@email.com
```

If you don't want to use IIS integration or can't use it / you are not using IIS, you can also 
run acme.exe without IIS support. In that case, you need to manually copy the challenge file
that is required to validate domain ownership to your server.

Request a certificate for www.yourdomain.com without IIS integration and accept the terms of service of the ACME server (-a), 
using your@email.com as registration contact information (-m):

```
acme.exe -a www.yourdomain.com -m mailto:your@email.com -c manual-http-01 -i manual
```

If something does not work, please contact me at mathias@raacke.info or submit an issue on GitHub. You
can increase the output verbosity by using the parameter -v Verbose

# Projects in this repository

## Oocx.ACME

This is an implementation of the ACME protocol. IT contains a class AcmeClient that can
be used to communicate with ACME servers.

Oocx.ACME supports .NET 4.6 and dnx46. It does not work with .NET 4.5 (see issue #2).
I have begun to work on .NET Core support. I cannot really test it yet because I first need a
version of NSubstitute that works with .NET Core, so that I can use .NET core to run my 
unit tests.

## Oocx.ACME.Console

This is a simple command line client that I use to test my ACME client. It does not yet have
any command line arguments (I use it by commenting out/in whatever I need).


# Change log

## 2015-12-06 ACME.net now also on NuGet (v.0.0.72)

The ACME.net protocol library is now also available on nuget.org. The API could still
change and is not widely used yet, therefore I have uploaded it as a prerelease package.

## 2015-11-22 IIS integration (v.0.0.56)

The console application can now configure IIS to automatically handle an http-01 challenge.
It can now also install the certificate into the certificate store and update IIS bindings 
to use the new certificate.

## 2015-11-16 basic command line client

The console application now accepts command line parameters. I also added a first prerelease
binary to the releases page. I have also set up an AppVeyor CI build.

## 2015-11-15 Initial Commit

This is just a prototype and work in progress. The code contains hard coded 
references to paths on my PC and hard coded certificate names and domain names.

However, it is complete enough that I was able to create a SSL certificate for my
web site test.startliste.info.
