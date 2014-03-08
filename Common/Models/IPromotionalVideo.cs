namespace Frost.Common.Models {

    public interface IPromotionalVideo : IMovieEntity {
        PromotionalVideoType Type { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        string Duration { get; set; }
        string Language { get; set; }
        string SubtitleLanguage { get; set; }
        IMovie Movie { get; set; }
    }

    public interface IPromotionalVideo<TMovie> : IPromotionalVideo where TMovie : IMovie {
        new TMovie Movie { get; set; }
    }

}