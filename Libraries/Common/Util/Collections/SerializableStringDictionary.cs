using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Common.Util.Collections {

    /// <summary>Fixes <see cref="StringDictionary"/> so it is able to be serialized in user.config</summary>
    [Serializable]
    public class SerializableStringDictionary : IXmlSerializable, IEnumerable<KeyValuePair<string, string>> {
        private const string XML_ELEMENT_NAME = "KeyValuePair";
        private const string END_ELEMENTS_INDICATOR = "end";
        private const string KEY_ATTRIBUTE_NAME = "key";
        private const string VALUE_ATTRIBUTE_NAME = "value";
        private readonly Dictionary<string, string> _dictionary;

        /// <summary>Initializes a new instance of the <see cref="SerializableStringDictionary"/> class.</summary>
        public SerializableStringDictionary() {
            _dictionary = new Dictionary<string, string>();
        }

        #region Dictionary Wrappers

        /// <summary>Determines whether the <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/> contains the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>true if the <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/> contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(string key) {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>Adds the specified key and value to the dictionary.</summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public void Add(string key, string value) {
            _dictionary.Add(key, value);
        }

        /// <summary>Removes the value with the specified key from the <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/>.</summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if key is not found in the <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/>.</returns>
        public bool Remove(string key) {
            return _dictionary.Remove(key);
        }

        #endregion

        /// <summary>This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.</summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.</returns>
        public XmlSchema GetSchema() {
            return null;
        }

        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader) {
            reader.MoveToContent();

            if (reader.IsEmptyElement) {
                return;
            }

            using (XmlReader subtree = reader.ReadSubtree()) {
                while (true) {
                    subtree.ReadStartElement();

                    if (subtree.Name == END_ELEMENTS_INDICATOR) {
                        break;
                    }

                    string key = subtree.GetAttribute(KEY_ATTRIBUTE_NAME);
                    string value = subtree.GetAttribute(VALUE_ATTRIBUTE_NAME);

                    if (string.IsNullOrEmpty(key)) {
                        continue;
                    }

                    _dictionary.Add(key, value);                    
                }
            }
        }

        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer) {
            foreach (KeyValuePair<string, string> pair in _dictionary) {
                writer.WriteStartElement(XML_ELEMENT_NAME);
                writer.WriteAttributeString(KEY_ATTRIBUTE_NAME, pair.Key);
                writer.WriteAttributeString(VALUE_ATTRIBUTE_NAME, pair.Value);
                writer.WriteEndElement();
            }
            writer.WriteStartElement(END_ELEMENTS_INDICATOR);
            writer.WriteEndElement();
        }

        #region IEnumerable

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return _dictionary.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable) _dictionary).GetEnumerator();
        }

        #endregion
    }

}