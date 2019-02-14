using EchoLibrary;
using System;
using System.ServiceModel;

namespace EchoClient
{
  /// <summary>
  /// Simple WCF client
  /// </summary>
  class Program
  {
    /// <summary>
    /// Program entry point
    /// </summary>
    static int Main(string[] args)
    {
      if (0 == args.Length)
        return Syntax();

      var host_name = args[0];
      if (string.IsNullOrEmpty(host_name))
        return Syntax();

      var message = string.Format("Hello from {0}!", Environment.MachineName);

      IEchoService channel = null;
      try
      {
        var binding = new BasicHttpBinding();
        var uri = string.Format("http://{0}:80/echo/basic", host_name);
        var endpoint = new EndpointAddress(uri);
        var factory = new ChannelFactory<IEchoService>(binding, endpoint);
        channel = factory.CreateChannel();

        // This works on Windows and Mac
        var rc = channel.Echo1(message);
        Console.WriteLine(rc);

        // This works on Windows and Mac
        rc = channel.Echo2(message);
        Console.WriteLine(rc);

        // This works on Windows and Mac
        //rc = channel.Echo1("");
        //Console.WriteLine(rc);

        // This works on Windows
        // This does not work on Mac
        rc = channel.Echo2("");
        Console.WriteLine(rc);
      }
      catch (FaultException ex)
      {
        Console.WriteLine(FaultExceptionMessage(ex));
        //Console.WriteLine(ex);
      }
      catch (CommunicationException ex)
      {
        Console.WriteLine(nameof(ex));
        Console.WriteLine(ex);
      }
      catch (TimeoutException ex)
      {
        Console.WriteLine(nameof(ex));
        Console.WriteLine(ex);
      }
      catch (Exception ex)
      {
        Console.WriteLine(nameof(ex));
        Console.WriteLine(ex);
      }

      if (null != channel)
        (channel as ICommunicationObject).Close();

      Console.WriteLine();
      Console.WriteLine("Press <ENTER> to continue.");
      Console.WriteLine();
      Console.ReadLine();

      return 0;
    }

    /// <summary>
    /// Function returns a string from a fault exception
    /// </summary>
    private static string FaultExceptionMessage(FaultException ex)
    {
      if (null == ex)
        return null;

      string rc = null;
      if (!string.IsNullOrEmpty(ex.Code.Name))
      {
        if (Int32.TryParse(ex.Code.Name, out var code))
        {
          switch ((StringFault)code)
          {
            case StringFault.Null:
              rc = "String is null.";
              break;
            case StringFault.Empty:
              rc = "String is empty.";
              break;
            case StringFault.WhiteSpace:
              rc = "String is whitespace.";
              break;
          }
        }
      }

      if (string.IsNullOrEmpty(rc))
        rc = ex.Message;

      return rc;
    }

    /// <summary>
    /// Friendly console message
    /// </summary>
    private static int Syntax()
    {
      System.Console.WriteLine("Usage: EchoClient <hostname or ipaddress> <message to echo>");
      return 1;
    }
  }

}
