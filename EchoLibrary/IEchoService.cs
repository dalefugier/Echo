using System.ServiceModel;

namespace EchoLibrary
{
  /// <summary>
  /// Service contract shared by EchoServer and EchoClient
  /// </summary>
  [ServiceContract]
  public interface IEchoService
  {
    [OperationContract]
    string Echo1(string message);

    [OperationContract]
    string Echo2(string message);
  }

  /// <summary>
  /// Enumeration that identifies string errors
  /// </summary>
  public enum StringFault
  {
    Null,
    Empty,
    WhiteSpace
  }
}
