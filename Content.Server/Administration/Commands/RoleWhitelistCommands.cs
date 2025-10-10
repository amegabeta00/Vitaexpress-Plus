// SPDX-FileCopyrightText: 2024 Brandon Hu <103440971+Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server.Database;
using Content.Server.Players.JobWhitelist;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class JobWhitelistAddCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly RoleWhitelistManager _roleWhitelist = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;
    [Dependency] private readonly IPlayerManager _players = default!;

    public override string Command => "rolewhitelistadd";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError("This command needs at least one argument.");
            shell.WriteLine(Help);
            return;
        }

        var player = args[0].Trim();

        var data = await _playerLocator.LookupIdByNameAsync(player);
        var adminData = await _playerLocator.LookupIdAsync(shell.Player?.UserId ?? new NetUserId(Guid.Empty));

        if (data != null)
        {
            var guid = data.UserId;
            var adminGuid = adminData?.UserId ?? Guid.Empty;
            var isWhitelisted = await _db.IsPlayerRoleWhitelisted(guid);
            if (isWhitelisted)
            {
                shell.WriteLine(Loc.GetString("cmd-rolewhitelistadd-already-whitelisted",
                    ("player", player)));
                return;
            }

            _roleWhitelist.AddWhitelist(guid, adminGuid);
            shell.WriteLine(Loc.GetString("cmd-rolewhitelistadd-added",
                ("player", player)));
            return;
        }

        shell.WriteError(Loc.GetString("cmd-rolewhitelist-player-doesnt-exist-error"));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                _players.Sessions.Select(s => s.Name),
                Loc.GetString("cmd-jobwhitelist-hint-player"));
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class GetJobWhitelistCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;
    [Dependency] private readonly IPlayerManager _players = default!;

    public override string Command => "rolewhitelistget";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError("This command needs at least one argument.");
            shell.WriteLine(Help);
            return;
        }

        var player = string.Join(' ', args).Trim();
        var data = await _playerLocator.LookupIdByNameAsync(player);
        if (data != null)
        {
            var guid = data.UserId;
            var whitelisted = await _db.IsPlayerRoleWhitelisted(guid);
            if (whitelisted)
            {
                shell.WriteLine(Loc.GetString("cmd-rolewhitelistget-whitelisted", ("player", player)));
                return;
            }
        }

        shell.WriteLine(Loc.GetString("cmd-rolewhitelistget-not-whitelisted",
            ("player", player)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                _players.Sessions.Select(s => s.Name),
                Loc.GetString("cmd-jobwhitelist-hint-player"));
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.RoleWhitelist)]
public sealed class RemoveJobWhitelistCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly RoleWhitelistManager _roleWhitelist = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;
    [Dependency] private readonly IPlayerManager _players = default!;

    public override string Command => "rolewhitelistremove";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError("This command needs at least one argument.");
            shell.WriteLine(Help);
            return;
        }

        var player = args[0].Trim();

        var data = await _playerLocator.LookupIdByNameAsync(player);
        var adminData = await _playerLocator.LookupIdAsync(shell.Player?.UserId ?? new NetUserId(Guid.Empty));

        if (data != null)
        {
            var guid = data.UserId;
            var adminGuid = adminData?.UserId ?? Guid.Empty;
            var isWhitelisted = await _db.IsPlayerRoleWhitelisted(guid);
            if (!isWhitelisted)
            {
                shell.WriteLine(Loc.GetString("cmd-rolewhitelistget-not-whitelisted",
                    ("player", player)));
                return;
            }

            _roleWhitelist.RemoveWhitelist(guid, adminGuid);
            shell.WriteLine(Loc.GetString("cmd-rolewhitelistremove-removed",
                ("player", player)));
            return;
        }

        shell.WriteError(Loc.GetString("cmd-rolewhitelist-player-doesnt-exist-error"));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                _players.Sessions.Select(s => s.Name),
                Loc.GetString("cmd-jobwhitelist-hint-player"));
        }

        return CompletionResult.Empty;
    }
}
