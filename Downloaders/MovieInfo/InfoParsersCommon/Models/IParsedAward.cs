namespace Frost.InfoParsers.Models {

    public interface IParsedAward {
        string Organization { get; set; }
        bool IsNomination { get; set; }
        string Award { get; set; }
    }

}