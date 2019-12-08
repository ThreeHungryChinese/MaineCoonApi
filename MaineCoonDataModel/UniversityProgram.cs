using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int belongsToUserID { get; set; }

        [Display(Name = "Program Introduction")]
        public string ProgramIntroduction { get; set; }
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

        [Display(Name = "Admission Process Setting"),StringLength(50)]
        public string _programJson {
            get {
                return JsonConvert.SerializeObject(ProgramJson);
            }
            set {
                var t = JsonConvert.DeserializeObject<JArray>(value);
                if (t != null) ProgramJson = t;
            }
        }
        public string _usedProcessorsIdJson {
            get {
                return JsonConvert.SerializeObject(UsedProcessorsIdJson);
            }
            set {
                var t = JsonConvert.DeserializeObject<JArray>(value);
                if (t != null) UsedProcessorsIdJson = t;
            }
        }
        public string _programParameterJson {
            get {
                return JsonConvert.SerializeObject(ProgramParameterJson);
            }
            set {
                var t = JsonConvert.DeserializeObject<JArray>(value);
                if (t != null) ProgramParameterJson = t;
            }
        }

        [Required, NotMapped]
        public JArray ProgramJson { get; set; } = new JArray();

        [Required, NotMapped]
        public JArray UsedProcessorsIdJson { get; set; } = new JArray();

        [Required, NotMapped]
        public JArray ProgramParameterJson { get; set; } = new JArray();
    }
}
