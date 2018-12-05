namespace HolmesMVC.Services.BaconXml
{
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable IDE1006 // Naming Styles
    using System.Xml;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class spellcheck
    {
        [XmlElement("match")]
        public string[] match { get; set; }

        [XmlAttribute]
        public string name { get; set; }
    }
#pragma warning restore CA1819 // Properties should not return arrays
#pragma warning restore IDE1006 // Naming Styles
}