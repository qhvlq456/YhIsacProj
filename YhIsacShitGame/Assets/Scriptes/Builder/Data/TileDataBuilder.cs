using System;
using static YhProj.Define;

public class TileDataBuilder : BaseDataBuilder<TileData>
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

    public TileDataBuilder SetRoadType(RoadType _roadType)
    {
        data.roadType = _roadType;
        return this;
    }
    public TileDataBuilder SetRoadType(object _roadType)
    {
        if(Enum.TryParse(_roadType.ToString(), out RoadType roadType))
        {
            data.roadType = roadType;
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
