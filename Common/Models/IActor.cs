namespace Frost.Common.Models {

    public interface IActor : IPerson {
        string Character { get; set; }
    }
}