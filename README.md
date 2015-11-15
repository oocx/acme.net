# Oocx.ACME - an ACME protocol library and simple Let's Encrypt client

This repository contains a library that can be used to develop ACME / Let's Encrypt clients.

**This project is work in progress, you probably won't be able to use it for anything yet**

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

# Projects in this repository

## Oocx.ACME

This is an implementation of the ACME protocol. IT contains a class AcmeClient that can
be used to communicate with ACME servers.

## Oocx.ACME.Console

This is a simple command line client that I use to test my ACME client. It does not yet have
any command line arguments (I use it by commenting out/in whatever I need).

## Oocx.Asn1PKCS

This project contains classes to generate PKCS10 certificate requests. It can also save private 
keys in a PKCS1 .pem file. I have started to work on PKCS12 (PFX) support, but this code is 
not used yet.

# Change log

## 2015-11-15 Initial Commit

This is just a prototype and work in progress. The code contains hard coded 
references to paths on my PC and hard coded certificate names and domain names.

However, it is complete enough that I was able to create a SSL certificate for my
web site test.startliste.info.