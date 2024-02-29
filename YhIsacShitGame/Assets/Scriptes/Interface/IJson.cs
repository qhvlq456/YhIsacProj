using System.Collections;
using System.Collections.Generic;

namespace YhProj
{
    public interface IJson
    {
        void SaveJson<T>(T _data);
        void LoadJson();
    }
}
