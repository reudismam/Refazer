#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

////#if (HAVE_XML_DOCUMENT || HAVE_XLINQ)

//#if HAVE_BIG_INTEGER
using System.Numerics;
//#endif
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Newtonsoft.Json.Serialization;
////#if HAVE_XLINQ
using System.Xml.Linq;
////#endif
using Newtonsoft.Json.Utilities;
////#endif

namespace Newtonsoft.Json.Converters
{


    private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
    {
        switch (propertyName)
        {
            case TextName:
                currentNode.AppendChild(document.CreateTextNode(ConvertTokenToXmlVlue(reader)));
                break;
            case CDataName:
                currentNode.AppendChild(document.CreateCDataSection(ConvertTokenToXmlVlue(reader)));
                break;
            case WhitespaceName:
                currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
                break;
            case SignificantWhitespaceName:
                currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
                break;
            default:
                // processing instructions and the xml declaration start with ?
                if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
                {
                    CreateInstruction(reader, document, currentNode, propertyName);
                }
                else if (string.Equals(propertyName, "!DOCTYPE", StringComparison.OrdinalIgnoreCase))
                {
                    CreateDocumentType(reader, document, currentNode);
                }
                else
                {
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        // handle nested arrays
                        ReadArrayElements(reader, document, propertyName, currentNode, manager);
                        return;
                    }

                    // have to wait until attributes have been parsed before creating element
                    // attributes may contain namespace info used by the element
                    ReadElement(reader, document, currentNode, propertyName, manager);
                }
                break;
        }
    }

    private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            throw JsonSerializationException.Create(reader, "XmlNodeConverter cannot convert JSON with an empty property name to XML.");
        }

        Dictionary<string, string> attributeNameValues = ReadAttributeElements(reader, manager);

        string elementPrefix = MiscellaneousUtils.GetPrefix(propertyName);

        if (propertyName.StartsWith('@'))
        {
            string attributeName = propertyName.Substring(1);
            string attributePrefix = MiscellaneousUtils.GetPrefix(attributeName);

            AddAttribute(reader, document, currentNode, propertyName, attributeName, manager, attributePrefix);
            return;
        }

        if (propertyName.StartsWith('$'))
        {
            switch (propertyName)
            {
                case JsonTypeReflector.ArrayValuesPropertyName:
                    propertyName = propertyName.Substring(1);
                    elementPrefix = manager.LookupPrefix(JsonNamespaceUri);
                    CreateElement(reader, document, currentNode, propertyName, manager, elementPrefix, attributeNameValues);
                    return;
                case JsonTypeReflector.IdPropertyName:
                case JsonTypeReflector.RefPropertyName:
                case JsonTypeReflector.TypePropertyName:
                case JsonTypeReflector.ValuePropertyName:
                    string attributeName = propertyName.Substring(1);
                    string attributePrefix = manager.LookupPrefix(JsonNamespaceUri);
                    AddAttribute(reader, document, currentNode, propertyName, attributeName, manager, attributePrefix);
                    return;
            }
        }

        CreateElement(reader, document, currentNode, propertyName, manager, elementPrefix, attributeNameValues);
    }

}