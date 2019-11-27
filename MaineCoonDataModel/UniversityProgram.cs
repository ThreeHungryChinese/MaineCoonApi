using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaineCoonApi.Models {
    public class UniversityProgram {
        [BindNever]
        [Display(Name ="ProgramId")]
        [Key]
        public int Id { get; set;  }

        [Display(Name = "Program Name")]
        [Required]
        public string ProgramName { get; set; }

        [Display(Name ="UserId")]
        public int BelongsToUserId { get; set; }

        [Display(Name ="Admission Process Setting")]
        public string ProgramJson { get; set; }
        /// <summary>
        /// Abandoned
        /// </summary>
        [Display(Name = "Score Algorithm Id")]
        public int ProcesserId { get; set; }

        [BindNever]
        [Display(Name ="Applied Count")]
        public int Count { get; set; }

        [BindNever]
        [Display(Name = "Need Train?")]
        public bool IsTrainNeeded { get; set; }
        [BindNever]
        [Display(Name = "IsEnabled")]
        public bool IsEnabled { get; set; }

        [Display(Name = "Program Introduction"),StringLength(50)]
        public string ProgramIntroduction { get; set; }
    }
}
