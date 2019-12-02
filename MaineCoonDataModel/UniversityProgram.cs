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
        /// <summary>
        /// 0 = enable, 1=DisabledByUser, 2 = Algorithm Disabled
        /// </summary>
        [BindNever]
        [Display(Name = "IsEnabled")]
        public int IsEnabled { get; set; }

        [Display(Name = "Program Introduction"),StringLength(50)]
        public string ProgramIntroduction { get; set; }

        public string UsedProcessorsIdJson { get; set; }
    }
}
