using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>This table lists all countries in which the movies were shot and/or produced.</summary>
    [Table("country")]
    public class XbmcCountry : ICountry {
        private string _countryName;
        private ISO3166 _iso3166;

        /// <summary>Initializes a new instance of the <see cref="XbmcCountry"/> class.</summary>
        public XbmcCountry() {
            Movies = new HashSet<XbmcDbMovie>();
        }

        public XbmcCountry(ISOCountryCode isoCode) {
            Name = isoCode.EnglishName;
            ISO3166 = new ISO3166(isoCode.Alpha2, isoCode.Alpha3);
        }

        internal XbmcCountry(ICountry country) {
            if (country.ISO3166 != null) {
                Name = country.ISO3166.Alpha3;
            }
            ISO3166 = country.ISO3166;
        }

        /// <summary>Gets or sets the Id of the country in the database.</summary>
        /// <value>The Id of the country in the database.</value>
        [Key]
        [Column("idCountry")]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the country.</summary>
        /// <value>The name of the country.</value>
        [Column("strCountry")]
        public string Name {
            get { return _countryName; }
            set {
                _countryName = value;

                if (!string.IsNullOrEmpty(_countryName)) {
                    ISOCountryCode isoCode = ISOCountryCodes.Instance.GetByEnglishName(_countryName);
                    if (isoCode != null) {
                        _iso3166 = new ISO3166(isoCode.Alpha2, isoCode.Alpha3);
                    }
                }
            }
        }

        /// <summary>Gets or sets the movies that were shor and/or produced in this country.</summary>
        /// <value>The movies that were shor and/or produced in this country.</value>
        public HashSet<XbmcDbMovie> Movies { get; set; }

        #region ICountry

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Name":
                    case "ISO3166":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        [NotMapped]
        public ISO3166 ISO3166 {
            get { return _iso3166; }
            set { _iso3166 = value; }
        }

        #endregion

        internal class Configuration : EntityTypeConfiguration<XbmcCountry> {

            public Configuration() {
                //Join table Movie <--> Country
                HasMany(m => m.Movies)
                    .WithMany(c => c.Countries)
                    .Map(m => {
                        m.ToTable("countrylinkmovie");
                        m.MapLeftKey("idCountry");
                        m.MapRightKey("idMovie");
                    });
            }

        }

    }

}
