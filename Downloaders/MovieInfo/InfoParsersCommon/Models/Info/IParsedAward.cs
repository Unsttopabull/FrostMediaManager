namespace Frost.InfoParsers.Models.Info {

    public interface IParsedAward {
        string Organization { get; set; }
        bool IsNomination { get; set; }
        string Award { get; set; }
    }

}