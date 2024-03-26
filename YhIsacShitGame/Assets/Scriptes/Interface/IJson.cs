using System.Collections;
namespace YhProj.Game
{
    public interface IJson
    {
        void SaveJson<T>(T _data);
        void LoadJson();
    }
}
