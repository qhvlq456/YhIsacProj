using System.Collections.Generic;

namespace YhProj
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
