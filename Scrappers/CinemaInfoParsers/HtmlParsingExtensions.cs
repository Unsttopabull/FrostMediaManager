using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Frost.InfoParsers {

    public static class HtmlParsingExtensions {
        public static string InnerTextOrNull(this HtmlNode node, bool decode = true) {
            if (node != null) {
                return decode 
                    ? WebUtility.HtmlDecode(node.InnerText.Trim())
                    : node.InnerText.Trim();
            }
            return null;
        }

        public static IEnumerable<string> InnerTextOrNull(this HtmlNodeCollection nodes, bool decode = true) {
            if (nodes != null && nodes.Count > 0) {
                return nodes.Select(node => decode ? WebUtility.HtmlDecode(node.InnerText.Trim()) : node.InnerText.Trim());
            }
            return null;
        }

        public static IEnumerable<string> InnerTextSplitOrNull(this HtmlNode node, bool decode, params char[] delimiters) {
            if (node != null) {
                return node.InnerText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                           .Select(str => decode ? WebUtility.HtmlDecode(str.Trim()) : str.Trim());
            }
            return null;
        }

        public static IEnumerable<string> InnerTextSplitOrNull(this HtmlNode node, bool decode, params string[] delimiters) {
            if (node != null) {
                return node.InnerText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                           .Select(str => decode ? WebUtility.HtmlDecode(str.Trim()) : str.Trim());
            }
            return null;
        }
    }

}