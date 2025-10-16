// SPDX-FileCopyrightText: 2024 Brandon Hu <103440971+Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Server.Players.RoleWhitelist;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Network;

namespace Content.Server.Administration.Commands;

public abstract class BaseRoleWhitelistCommand : LocalizedCommands
{
    [Dependency] protected readonly IServerDbManager Db = default!;
    [Dependency] protected readonly IPlayerLocator PlayerLocator = default!;
    [Dependency] protected readonly IPlayerManager Players = default!;

    protected const string PlayerNotFoundError = "cmd-rolewhitelist-player-doesnt-exist-error";

    protected async Task<LocatedPlayerData?> FindPlayerAsync(string playerName)
    {
        return await PlayerLocator.LookupIdByNameAsync(playerName.Trim());
    }

    protected NetUserId GetAdminUserId(IConsoleShell shell)
    {
        return shell.Player?.UserId ?? new NetUserId(Guid.Empty);
    }

    protected CompletionResult GetPlayerCompletion(string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                Players.Sessions.Select(s => s.Name),
                Loc.GetString("cmd-jobwhitelist-hint-player"));
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class JobWhitelistAddCommand : BaseRoleWhitelistCommand
{
    [Dependency] private readonly RoleWhitelistManager _roleWhitelist = default!;

    public override string Command => "rw:add";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError(Loc.GetString("cmd-need-at-least-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        var playerName = args[0].Trim();
        var playerData = await FindPlayerAsync(playerName);

        if (playerData == null)
        {
            shell.WriteError(Loc.GetString(PlayerNotFoundError));
            return;
        }

        var adminData = await PlayerLocator.LookupIdAsync(GetAdminUserId(shell));
        var playerGuid = playerData.UserId;
        var adminGuid = adminData?.UserId ?? Guid.Empty;

        if (await Db.IsPlayerRoleWhitelisted(playerGuid))
        {
            shell.WriteLine(Loc.GetString("cmd-rolewhitelistadd-already-whitelisted",
                ("player", playerName)));
            return;
        }

        _roleWhitelist.AddWhitelist(playerGuid, adminGuid);
        shell.WriteLine(Loc.GetString("cmd-rolewhitelistadd-added",
            ("player", playerName)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return GetPlayerCompletion(args);
    }
}

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class GetJobWhitelistCommand : BaseRoleWhitelistCommand
{
    public override string Command => "rw:check";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError(Loc.GetString("cmd-need-at-least-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        var playerName = args[0].Trim();
        var playerData = await FindPlayerAsync(playerName);

        if (playerData == null)
        {
            shell.WriteLine(Loc.GetString("cmd-rolewhitelistget-not-whitelisted",
                ("player", playerName)));
            return;
        }

        var isWhitelisted = await Db.IsPlayerRoleWhitelisted(playerData.UserId);
        var messageKey = isWhitelisted
            ? "cmd-rolewhitelistget-whitelisted"
            : "cmd-rolewhitelistget-not-whitelisted";

        shell.WriteLine(Loc.GetString(messageKey, ("player", playerName)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return GetPlayerCompletion(args);
    }
}

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class RemoveJobWhitelistCommand : BaseRoleWhitelistCommand
{
    [Dependency] private readonly RoleWhitelistManager _roleWhitelist = default!;

    public override string Command => "rw:remove";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError(Loc.GetString("cmd-need-at-least-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        var playerName = args[0].Trim();
        var playerData = await FindPlayerAsync(playerName);

        if (playerData == null)
        {
            shell.WriteError(Loc.GetString(PlayerNotFoundError));
            return;
        }

        var adminData = await PlayerLocator.LookupIdAsync(GetAdminUserId(shell));
        var playerGuid = playerData.UserId;
        var adminGuid = adminData?.UserId ?? Guid.Empty;

        if (!await Db.IsPlayerRoleWhitelisted(playerGuid))
        {
            shell.WriteLine(Loc.GetString("cmd-rolewhitelistget-not-whitelisted",
                ("player", playerName)));
            return;
        }

        _roleWhitelist.RemoveWhitelist(playerGuid, adminGuid);
        shell.WriteLine(Loc.GetString("cmd-rolewhitelistremove-removed",
            ("player", playerName)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return GetPlayerCompletion(args);
    }
}

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class GetAllJobWhitelistCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly IWhitelistDataService _whitelistService = default!;

    public override string Command => "rw:all";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        try
        {
            var whitelists = await _db.GetAllRoleWhitelists();
            var activeWhitelists = whitelists.Where(wl => wl.InWhitelist).ToList();

            if (!activeWhitelists.Any())
            {
                shell.WriteLine(Loc.GetString("cmd-rolewhitelistgetall-empty-list-error"));
                return;
            }

            var playerCache = await _whitelistService.BuildPlayerCacheAsync(activeWhitelists);
            var output = BuildWhitelistOutput(activeWhitelists, playerCache);

            shell.WriteLine(Loc.GetString("cmd-rolewhitelistgetall-entry-count",
                ("count", activeWhitelists.Count)));
            shell.WriteLine(output);
        }
        catch (Exception ex)
        {
            shell.WriteError(Loc.GetString("cmd-rolewhitelistgetall-error",
                ("error", ex.Message)));
        }
    }

    private string BuildWhitelistOutput(List<RoleWhitelist> whitelists, Dictionary<Guid, LocatedPlayerData> playerCache)
    {
        var outputBuilder = new StringBuilder();
        var validEntries = 0;

        for (var i = 0; i < whitelists.Count; i++)
        {
            var whitelist = whitelists[i];

            if (!playerCache.TryGetValue(whitelist.PlayerId, out var playerData))
                continue;

            var playerEntry = _whitelistService.FormatWhitelistEntry(whitelist, playerData, ++validEntries, playerCache);
            outputBuilder.Append(playerEntry);
        }

        return outputBuilder.ToString();
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return CompletionResult.Empty;
    }
}

public interface IWhitelistDataService
{
    Task<Dictionary<Guid, LocatedPlayerData>> BuildPlayerCacheAsync(IEnumerable<RoleWhitelist> whitelists);
    string FormatWhitelistEntry(RoleWhitelist whitelist, LocatedPlayerData playerData, int position, Dictionary<Guid, LocatedPlayerData> playerCache);
}

public sealed class WhitelistDataService : IWhitelistDataService
{
    private readonly IPlayerLocator _playerLocator;
    private readonly TimeZoneInfo _moscowTime;

    public WhitelistDataService(IPlayerLocator playerLocator)
    {
        _playerLocator = playerLocator;
        _moscowTime = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
    }

    public async Task<Dictionary<Guid, LocatedPlayerData>> BuildPlayerCacheAsync(IEnumerable<RoleWhitelist> whitelists)
    {
        var cache = new Dictionary<Guid, LocatedPlayerData>();
        var uniquePlayerIds = CollectUniquePlayerIds(whitelists);

        var lookupTasks = uniquePlayerIds.Select(async playerId =>
        {
            var playerData = await _playerLocator.LookupIdAsync(new NetUserId(playerId));
            return (playerId, playerData);
        });

        var results = await Task.WhenAll(lookupTasks);

        foreach (var (playerId, playerData) in results)
        {
            if (playerData != null)
            {
                cache[playerId] = playerData;
            }
        }

        return cache;
    }

    public string FormatWhitelistEntry(RoleWhitelist whitelist, LocatedPlayerData playerData, int position, Dictionary<Guid, LocatedPlayerData> playerCache)
    {
        var entryBuilder = new StringBuilder();

        entryBuilder.AppendLine(Loc.GetString("cmd-rolewhitelistgetall-entry-one",
            ("pos", position),
            ("usr", playerData.Username)));

        entryBuilder.AppendLine(Loc.GetString("cmd-rolewhitelistgetall-entry-two",
            ("admin", GetAdminNameFromCache(whitelist.FirstTimeAddedBy, playerCache))));

        entryBuilder.AppendLine(Loc.GetString("cmd-rolewhitelistgetall-entry-three",
            ("admin", GetAdminNameFromCache(whitelist.LastTimeAddedBy, playerCache))));

        entryBuilder.AppendLine(Loc.GetString("cmd-rolewhitelistgetall-entry-four",
            ("admin", GetAdminNameFromCache(whitelist.LastTimeRemovedBy, playerCache))));

        entryBuilder.AppendLine(Loc.GetString("cmd-rolewhitelistgetall-entry-five",
            ("times", whitelist.HowManyTimesAdded),
            ("firstadded", ConvertToMoscowTime(whitelist.FirstTimeAdded))));

        entryBuilder.AppendLine(Loc.GetString("cmd-rolewhitelistgetall-entry-six",
            ("lastadded", whitelist.HowManyTimesAdded),
            ("lastremove", GetLastRemovedTime(whitelist.LastTimeRemoved))));

        entryBuilder.AppendLine();
        return entryBuilder.ToString();
    }

    private static HashSet<Guid> CollectUniquePlayerIds(IEnumerable<RoleWhitelist> whitelists)
    {
        var uniquePlayerIds = new HashSet<Guid>();

        foreach (var whitelist in whitelists)
        {
            uniquePlayerIds.Add(whitelist.PlayerId);
            AddAdminIdIfValid(uniquePlayerIds, whitelist.FirstTimeAddedBy);
            AddAdminIdIfValid(uniquePlayerIds, whitelist.LastTimeAddedBy);
            AddAdminIdIfValid(uniquePlayerIds, whitelist.LastTimeRemovedBy);
        }

        return uniquePlayerIds;
    }

    private static void AddAdminIdIfValid(HashSet<Guid> set, Guid? adminId)
    {
        if (adminId.HasValue && adminId.Value != Guid.Empty)
        {
            set.Add(adminId.Value);
        }
    }

    private string GetAdminNameFromCache(Guid? adminId, Dictionary<Guid, LocatedPlayerData> playerCache)
    {
        if (adminId == null || adminId.Value == Guid.Empty)
            return "Неизвестно";

        return playerCache.TryGetValue(adminId.Value, out var adminData)
            ? adminData.Username
            : "Неизвестно";
    }

    private string GetLastRemovedTime(DateTime? lastRemoved)
    {
        return lastRemoved.HasValue
            ? ConvertToMoscowTime(lastRemoved.Value)
            : "Порядочный";
    }

    private string ConvertToMoscowTime(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime(dateTime, _moscowTime).ToString("yyyy-M-d H:mm:ss");
    }
}
