using System.Xml;

namespace OpmlParser
{
    public static class Parser
    {
        public static string[] ParseAtrribute(string filePath, string parseAtrribute)
        {
            XmlReader xmlReader = XmlReader.Create(filePath);

            var list = new List<string> {};
            
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.HasAttributes)
                    {
                        // Console.WriteLine(xmlReader.GetAttribute(parseAtrribute));
                        var atrribute = xmlReader.GetAttribute(parseAtrribute);
                        if (atrribute != null)
                        {
                            list.Add(atrribute);   
                        }
                    }
                }
            }

            return list.ToArray();
        }
    }
}