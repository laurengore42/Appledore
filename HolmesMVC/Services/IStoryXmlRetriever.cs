namespace HolmesMVC.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Xml;

    [ServiceContract]
    public interface IStoryXmlRetriever
    {
        [WebGet(
                    BodyStyle = WebMessageBodyStyle.WrappedRequest,
                    ResponseFormat = WebMessageFormat.Xml
               )]
        [OperationContract]
        XmlElement Retrieve(string storyCode);
    }
}
