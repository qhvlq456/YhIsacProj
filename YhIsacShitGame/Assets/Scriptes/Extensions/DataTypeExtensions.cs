using System;
using System.Collections.Generic;
using System.Linq;
using static YhProj.Define;

public static class DataTypeExtensions
{
    #region Convert Enum(DataType) to String Valeus
    public static List<string> GetDropdownOptions(this Type _type)
    {
        if (_type == typeof(Direction))
        {
            return Enum.GetNames(typeof(Direction)).ToList();
        }
        else if (_type == typeof(BaseType))
        {
            return Enum.GetNames(typeof(BaseType)).ToList();
        }
        else if (_type == typeof(ElementType))
        {
            return Enum.GetNames(typeof(ElementType)).ToList();
        }
        else if (_type == typeof(bool))
        {
            return new List<string> { "True", "False" };
        }
        // 추가 자료형들에 대한 처리...
        else
        {
            return new List<string>();
        }
    }
    #endregion
}
