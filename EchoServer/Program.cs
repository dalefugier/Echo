using EchoLibrary;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace EchoServer
{
  /// <summary>
  /// Simple WCF service
  /// </summary>
  class Program
  {
    /// <summary>
    /// Program entry point
    /// </summary>
    static void Main(string[] args)
    {
      var base_address = new Uri("http://localhost:80/echo/");
      var service_host = new ServiceHost(typeof(EchoService), base_address);

      try
      {
        var binding = new BasicHttpBinding();
        service_host.AddServiceEndpoint(typeof(IEchoService), binding, "basic");

        var debug_behavior = service_host.Description.Behaviors.Find<ServiceDebugBehavior>();
        if (null == debug_behavior)
        {
          debug_behavior = new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true };
          service_host.Description.Behaviors.Add(debug_behavior);
        }
        else
        {
          debug_behavior.IncludeExceptionDetailInFaults = true;
        }

        service_host.Open();
        Console.WriteLine("The Echo service is ready.");
        Console.WriteLine("Press <ENTER> to terminate service.");
        Console.WriteLine();
        Console.ReadLine();
        service_host.Close();
      }
      catch (CommunicationException ex)
      {
        Console.WriteLine("An exception occurred: {0}", ex.Message);
        service_host.Abort();
      }
    }
  }

  /// <summary>
  /// Implements the service contract
  /// </summary>
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class EchoService : IEchoService
  {
    /// <summary>
    /// Converts a StringFault to a string
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private string StringFaultCode(StringFault code)
    {
      return ((int)code).ToString();
    }

    /// <summary>
    /// Test method that throws a fault exception without a fault code.
    /// </summary>
    public string Echo1(string message)
    {
      if (string.IsNullOrEmpty(message))
        throw new FaultException("String is null or empty.");

      var rc = string.Format("Echo1 - {0}", message);
      Console.WriteLine(rc);
      return rc;
    }

    /// <summary>
    /// Test method that throws a fault exception with a fault code.
    /// This will not work on a Mono client.
    /// </summary>
    public string Echo2(string message)
    {
      if (null == message)
      {
        var code = new FaultCode(StringFaultCode(StringFault.Null));
        throw new FaultException("String is null.", code);
      }

      if (string.IsNullOrEmpty(message))
      {
        var code = new FaultCode(StringFaultCode(StringFault.Empty));
        throw new FaultException("String is empty.", code);
      }

      if (string.IsNullOrWhiteSpace(message))
      {
        var code = new FaultCode(StringFaultCode(StringFault.WhiteSpace));
        throw new FaultException("String is whitespace.", code);
      }

      var rc = string.Format("Echo2 - {0}", message);
      Console.WriteLine(rc);
      return rc;
    }
  }

}
