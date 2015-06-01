namespace HolmesMVC.Services
{
    using System.Web.Hosting;
    using System.Xml;

    public class StoryXmlRetriever : IStoryXmlRetriever
    {
        public XmlElement Retrieve(string storyCode)
        {
            var xmlDoc = new XmlDocument();
            
            var storyUrl = HostingEnvironment.MapPath("~/Services/Stories/" + storyCode + ".xml");

            if (storyUrl != null)
            {
                xmlDoc.Load(storyUrl);

                return xmlDoc.DocumentElement;
            }

            return null;
        }
    }
}
