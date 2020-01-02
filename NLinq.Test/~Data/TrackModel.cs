using Dawnx.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLinq.Test
{
    public class TrackModel : IEntity<TrackModel>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [TrackCreationTime]
        public DateTime CreationTime { get; set; }

        [TrackLastWriteTime]
        public DateTime LastWriteTime { get; set; }

        [TrackTrim]
        public string ForTrim { get; set; }

        [TrackUpper]
        public string ForUpper { get; set; }

        [TrackLower]
        public string ForLower { get; set; }

        [TrackCondensed]
        public string ForCondensed { get; set; }

        [Track(typeof(RegexUtility), nameof(RegexUtility.IPRange) + "(" + nameof(ForTrim) + ")")]
        public string Automatic { get; set; }
    }
}
