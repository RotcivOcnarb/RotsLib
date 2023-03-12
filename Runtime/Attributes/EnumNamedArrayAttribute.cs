using UnityEngine;
namespace Rotslib.Attributes {
    public class EnumNamedArrayAttribute : PropertyAttribute {
        public string[] names;
        public EnumNamedArrayAttribute(System.Type names_enum_type) {
            names = System.Enum.GetNames(names_enum_type);
        }
    }
}