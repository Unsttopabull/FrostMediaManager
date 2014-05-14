namespace Frost.Common.Util.Collections {

    /// <summary>Represents a string dictionary key-value pair</summary>
    public interface IKeyValue {

        /// <summary>The key used to access the value.</summary>
        string Key { get; }

        /// <summary>The pair value.</summary>
        string Value { get; }
    }

}