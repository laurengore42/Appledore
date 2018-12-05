namespace HolmesMVC.Services.BaconXml
{
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable IDE1006 // Naming Styles
    using System.Xml;
    using System.Xml.Serialization;

    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        actor,
        movie
    }
#pragma warning restore CA1819 // Properties should not return arrays
#pragma warning restore IDE1006 // Naming Styles
}