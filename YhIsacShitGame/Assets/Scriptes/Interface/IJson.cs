using System.Collections;
using System.Collections.Generic;

namespace YhProj
{
    public interface IJson
    {
        void JsonToData(string _json);
        void DataToJson();
    }
}
