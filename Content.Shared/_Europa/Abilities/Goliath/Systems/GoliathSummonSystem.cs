using Content.Shared.Maps;
using Content.Shared.Physics;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Random;

namespace Content.Shared.Abilities.Goliath;

public sealed class GoliathSpawnSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly TurfSystem _turf = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<GoliathSummonBabyAction>(OnSummonAction);
    }

    private void OnSummonAction(GoliathSummonBabyAction args)
    {
        if (args.Handled)
            return;

        var coords = args.Target;
        List<EntityCoordinates> spawnPos = new();
        spawnPos.Add(coords);

        for (var i = 0; i < 1; i++)
        {
            spawnPos.Add(coords);
        }

        foreach (var pos in spawnPos)
        {
            if (_transform.GetGrid(pos) is not { } grid ||
                !TryComp<MapGridComponent>(grid, out var gridComp) ||
                !_map.TryGetTileRef(grid, gridComp, pos, out var tileRef) ||
                tileRef.IsSpace() ||
                _turf.IsTileBlocked(tileRef, CollisionGroup.Impassable))
            {
                continue;
            }

            if (_net.IsServer)
                Spawn(args.EntityId, pos);
        }

        args.Handled = true;
    }
}
