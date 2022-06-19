using System.Collections.Generic;
using System.Xml;

namespace XrmEarth.Configuration.Query
{
    public class XmlBuilder
    {
        // The internal XmlDocument that holds the complete structure.
        readonly XmlDocument _xd = new XmlDocument();

        // A stack representing the hierarchy of nodes added. nodeStack.Peek() will always be the current node scope.
        readonly Stack<XmlNode> _nodeStack = new Stack<XmlNode>();

        // Whether the next node should be created in the scope of the current node.
        bool _nextNodeWithin;

        // The current node. If null, the current node is the XmlDocument itself.
        XmlNode _currentNode;

        /// <summary>
        /// Returns the string representation of the XmlDocument.
        /// </summary>
        /// <returns>A string representation of the XmlDocument.</returns>
        public string GetOuterXml()
        {
            return _xd.OuterXml;
        }

        /// <summary>
        /// Returns the XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlDocument()
        {
            return _xd;
        }

        /// <summary>
        /// Changes the scope to the current node.
        /// </summary>
        /// <returns>this</returns>
        public XmlBuilder Within()
        {
            _nextNodeWithin = true;

            return this;
        }

        /// <summary>
        /// Changes the scope to the parent node.
        /// </summary>
        /// <returns>this</returns>
        public XmlBuilder EndWithin()
        {
            if (_nextNodeWithin)
                _nextNodeWithin = false;
            else
                _nodeStack.Pop();

            return this;
        }

        /// <summary>
        /// Adds an XML declaration with the most common values.
        /// </summary>
        /// <returns>this</returns>
        public XmlBuilder XmlDeclaration() { return XmlDeclaration("1.0", "utf-8", ""); }

        /// <summary>
        /// Adds an XML declaration to the document.
        /// </summary>
        /// <param name="version">The version of the XML document.</param>
        /// <param name="encoding">The encoding of the XML document.</param>
        /// <param name="standalone">Whether the document is standalone or not. Can be yes/no/(null || "").</param>
        /// <returns>this</returns>
        public XmlBuilder XmlDeclaration(string version, string encoding, string standalone)
        {
            XmlDeclaration xdec = _xd.CreateXmlDeclaration(version, encoding, standalone);
            _xd.AppendChild(xdec);

            return this;
        }

        /// <summary>
        /// Creates a node. If no nodes have been added before, it'll be the root node, otherwise it'll be appended as a child of the current node.
        /// </summary>
        /// <param name="name">The name of the node to create.</param>
        /// <returns>this</returns>
        public XmlBuilder Node(string name)
        {
            XmlNode xn = _xd.CreateElement(name);

            // If nodeStack.Count == 0, no nodes have been added, thus the scope is the XmlDocument itself.
            if (_nodeStack.Count == 0)
            {
                _xd.AppendChild(xn);

                // Automatically change scope to the root DocumentElement.
                _nodeStack.Push(xn);
            }
            else
            {
                // If this node should be created within the scope of the current node, change scope to the current node before adding the node to the scope element.
                if (_nextNodeWithin)
                {
                    _nodeStack.Push(_currentNode);

                    _nextNodeWithin = false;
                }

                _nodeStack.Peek().AppendChild(xn);
            }

            _currentNode = xn;

            return this;
        }

        /// <summary>
        /// Sets the InnerText of the current node without using CData.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public XmlBuilder InnerText(string text)
        {
            return InnerText(text, false);
        }

        /// <summary>
        /// Sets the InnerText of the current node.
        /// </summary>
        /// <param name="text">The text to set.</param>
        /// <returns>this</returns>
        public XmlBuilder InnerText(string text, bool useCData)
        {
            if (useCData)
                _currentNode.AppendChild(_xd.CreateCDataSection(text));
            else
                _currentNode.AppendChild(_xd.CreateTextNode(text));

            return this;
        }

        public XmlBuilder AppendRawXml(string text)
        {
            _currentNode.InnerXml += text;
            return this;
        }

        /// <summary>
        /// Adds an attribute to the current node.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>this</returns>
        public XmlBuilder Attribute(string name, string value)
        {
            XmlAttribute xa = _xd.CreateAttribute(name);
            xa.Value = value;

            _currentNode.Attributes.Append(xa);

            return this;
        }

        public XmlBuilder Attribute(string name, object value)
        {
            return Attribute(name, value.ToString());
        }

        public XmlBuilder BooleanAttribute(string attributeName, bool value)
        {
            return Attribute(attributeName, value ? "true" : "false");
        }
    }
}
