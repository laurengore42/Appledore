namespace HolmesMVC.Services.StoryService
{
    using System.Web.Hosting;
    using System.Xml;

    public class StoryXmlRetriever : IStoryXmlRetriever
    {
        public XmlElement Retrieve(string storyCode)
        {
            var xmlDoc = new XmlDocument();
            
            var storyUrl = HostingEnvironment.MapPath("~/Services/StoryService/Stories/" + storyCode + ".xml");

            if (storyUrl != null)
            {
                xmlDoc.Load(storyUrl);

                return xmlDoc.DocumentElement;
            }

            return null;
        }
    }
}
