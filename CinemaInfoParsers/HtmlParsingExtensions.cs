using System;
using System.Linq;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers {

    public static class HtmlParsingExtensions {
        public static string InnerTextOrNull(this HtmlNode node) {
            if (node != null) {
                return node.InnerText.Trim();
            }
            return null;
        }

        public static string[] InnerTextOrNull(this HtmlNodeCollection nodes) {
            if (nodes != null && nodes.Count > 0) {
                return nodes.Select(node => node.InnerText.Trim()).ToArray();
            }
            return null;
        }

        public static string[] InnerTextSplitOrNull(this HtmlNode node, params char[] delimiters) {
            if (node != null) {
                return node.InnerText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(str => str.Trim())
                                     .ToArray();
            }
            return null;
        }

        public static string[] InnerTextSplitOrNull(this HtmlNode node, params string[] delimiters) {
            if (node != null) {
                return node.InnerText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(str => str.Trim())
                                     .ToArray();
            }
            return null;
        }
    }

}