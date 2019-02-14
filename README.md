# Echo
This simple WCF example demonstrate an error that occurs in Mono on macOS. 

## Overview

If a WCF server, running on Windows, throws a `FaultException` that contains a `FaultCode` to a WCF client, running on macOS, the following `CommunicationException` is caught, not the expected `FaultException`.

```
{System.ServiceModel.CommunicationException: Received an invalid SOAP Fault message ---
> System.Xml.XmlException: Content cannot be converted to the type
System.Xml.XmlQualifiedName. Line 1, position 78. ---> System.FormatException: The ':'
character, hexadecimal value 0x3A, cannot be included in a name.
```
Note, if the same WCF server throws a `FaultException` without a `FaultCode`, the same WCF client will catch the `FaultException` as expected.

I've tested this with:

- Microsoft Visual Studio for Mac, Version 7.7.4 (build 1)
- Mono Version 5.16.0.220

Note, this worked in (much) earlier versions of Mono. I've tested against Version 4.0.2.

Also note, this error does not occur on Windows.

## Details

To demonstrate the error:

1. Clone the repository on both Windows and macOS.

2. Build a **Release** build of the solution on Windows and then run **EchoServer.exe**. Note, you might need to open TCP Port 80 for incoming traffic in the Windows Firewall.

3. Build a **Debug** build of the solution on macOS and then debug the **EchoClient** project. This is a console utility that sends a simple string to **EchoServer**. 

4. For easy debugging, add the host name or IP address of the system running **EchoServer** in the project's run arguments (*EchoClient > Option > Default > Arguments*).

## Notes

If you look at the **EchoService** service contract,  you'll see two two *Echo* methods: one that throws a `FaultException` and one that throws a `FaultException` with a `FaultCode`.

The code in **EchoClient** calls each of these functions, once with valid input and one without, so as to trigger a `FaultException`.
