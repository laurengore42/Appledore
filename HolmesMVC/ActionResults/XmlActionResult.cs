using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace HolmesMVC.ActionResults
{
    public class XmlActionResult : ActionResult
    {
        public XmlActionResult(XDocument xml)
        {
            Xml = xml;
        }

        public XDocument Xml { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/xml";
            context.HttpContext.Response.HeaderEncoding = Encoding.UTF8;
            XmlTextWriter writer = new XmlTextWriter(context.HttpContext.Response.OutputStream, Encoding.UTF8); ;
            Xml.WriteTo(writer);
            writer.Close();
        }
    }
}