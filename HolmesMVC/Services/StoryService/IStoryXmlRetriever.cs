namespace HolmesMVC.Services.StoryService
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
