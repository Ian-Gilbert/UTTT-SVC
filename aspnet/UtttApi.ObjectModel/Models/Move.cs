using System.ComponentModel.DataAnnotations;
using UtttApi.ObjectModel.Enums;

namespace UtttApi.ObjectModel.Models
{
    public class Move
    {
        [Required]
        [Range(1, 2)]
        public MarkType Mark { get; set; }

        [Required]
        [Range(0, 8)]
        public int LbIndex { get; set; }

        [Required]
        [Range(0, 8)]
        public int MarkIndex { get; set; }
    }
}