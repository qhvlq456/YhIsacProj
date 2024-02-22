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
        else if (_type == typeof(RoadType))
        {
            return Enum.GetNames(typeof(RoadType)).ToList();
        }
        else if (_type == typeof(bool))
        {
            return new List<string> { "True", "False" };
        }
        // �߰� �ڷ����鿡 ���� ó��...
        else
        {
            return new List<string>();
        }
    }
    #endregion
}
