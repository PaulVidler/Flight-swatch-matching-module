using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Atlass.Riegl
{
    /// <summary>
    /// In RiProcess Project Files (RPP files) are node links.
    /// E.g.
    /// <link name="lasdevice" node="/*[0]/*[0]/*[2]/*[0]" kind="lasdevice"/>
    /// <link name="lasconfig" node="/*[0]/*[0]/*[1]/*[2]" kind="lasconfig"/>
    /// 
    /// Despite appearances, these are NOT XPath syntax.
    /// This class parses and processes node links for RiProcess project files.
    /// </summary>
    internal class NodeLink
    {
        private List<int> _indices;

        /// <param name="indices">List of indices</param>
        public NodeLink(IEnumerable<int> indices)
        {
            _indices = indices.ToList();
        }

        public override string ToString()
        {
            return string.Join("", _indices.Select(i => $"/*[{i}]"));
        }

        public XElement GetElement(XElement projectElement)
        {
            if (projectElement.Attribute("kind").Value != "project")
            {
                throw new Exception($"Unexpected object type for {nameof(NodeLink)}. Expected 'project', but received '{projectElement.Attribute("kind").Value}'.");
            }

            var elementToEvaluate = projectElement;

            // Skip first index, because that's always zero. I think that's possibly the project index?

            for (int i = 1; i < _indices.Count; i++)
            {
                var index = _indices[i];

                var elementObjects = elementToEvaluate.XPathSelectElement("objects");

                if (elementObjects == null)
                {
                    throw new Exception($"Could not process {nameof(NodeLink)} because the element doesn't have an 'objects' node.");
                }

                elementToEvaluate = elementObjects.Elements().ElementAtOrDefault(index);

                if (elementToEvaluate == null)
                {
                    return null;
                }
            }

            return elementToEvaluate;
        }

        public static NodeLink Parse(string node)
        {
            if (!node.StartsWith("/*[0]"))
            {
                throw new Exception($"Could not parse RPP node link. Expected it to start with '/*[0]', but received {node}");
            }

            var segments = node.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length <= 1)
            {
                throw new Exception($"Expected at least 2 segments in RPP node link, but found only {segments.Length}. Node link: {node}");
            }

            if (!segments.Any(s => Regex.IsMatch(s, "^\\*\\[\\d+\\]$")))
            {
                throw new Exception($"Not all segments in the RPP node link were of expected format /*[number-here]. Node link: {node}");
            }

            return new NodeLink(segments.Select(s => int.Parse(Regex.Match(s, "\\d+").Value)));
        }
    }
}
