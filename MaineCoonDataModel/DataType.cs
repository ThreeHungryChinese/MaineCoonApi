using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaineCoonApi.Models {
    static public class CataType {
        public enum Type{
            number = TypeCode.Double,
            text = TypeCode.String,
            file = TypeCode.Object
        }
    }
}
