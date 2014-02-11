﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Util;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a movie set or collection.</summary>
    [Table("Sets")]
    public class Set {

        /// <summary>Initializes a new instance of the <see cref="Set"/> class.</summary>
        private Set() {
            Movies = new ObservableHashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Set"/> class with specified set name.</summary>
        /// <param name="name">The name of this set.</param>
        public Set(string name) : this() {
            Name = name;
        }

        /// <summary>Gets or sets the database Id of this set.</summary>
        /// <value>The database Id of this set</value>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of this set.</summary>
        /// <value>The name of this set.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the movies in this set.</summary>
        /// <value>The movies in this set.</value>
        public ObservableHashSet<Movie> Movies { get; set; }

    }

}
