namespace HolmesMVC.Services.BaconXml
{
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable IDE1006 // Naming Styles
    using System.Xml;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class link
    {
        [XmlElement("actor", typeof(string))]
        [XmlElement("movie", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public string[] Items { get; set; }

        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType[] ItemsElementName { get; set; }
    }
#pragma warning restore CA1819 // Properties should not return arrays
#pragma warning restore IDE1006 // Naming Styles
}