using System;
using static YhProj.Define;

namespace YhProj.Game.Map
{
    public class TileDataBuilder : GameDataBuilder<TileData>
    {
        public TileDataBuilder SetDirection(Direction _direction)
        {
            data.direction = _direction;
            return this;
        }
        public TileDataBuilder SetDirection(object _direction)
        {
            if (Enum.TryParse(_direction.ToString(), out Direction direction))
            {
                data.direction = direction;
            }

            return this;
        }

        public TileDataBuilder SetRoadType(ElementType _elementType)
        {
            data.elementType = _elementType;
            return this;
        }
        public TileDataBuilder SetRoadType(object _elementObj)
        {
            if (Enum.TryParse(_elementObj.ToString(), out ElementType _elementType))
            {
                data.elementType = _elementType;
            }

            return this;
        }
        public TileDataBuilder SetBatchIdx(int _batchIdx)
        {
            data.batchIdx = _batchIdx;
            return this;
        }

        public override TileData Build()
        {
            return data;
        }
    }
}
