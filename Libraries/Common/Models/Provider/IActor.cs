namespace Frost.Common.Models.Provider {

    public interface IActor : IPerson {
        string Character { get; set; }
    }
}