﻿using System.Collections.Generic;
using Avalonia.Themes.Fluent;
using Clasharp.Models.Profiles;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Clasharp.Models.Settings;

public class AppSettings: ReactiveObject
{
    public AppSettings()
    {
        ThemeMode = FluentThemeMode.Light;
    }

    public SystemProxyMode SystemProxyMode { get; set; }
    public bool UseServiceMode { get; set; }
    public List<Profile> Profiles { get; set; } = new();
    
    public string? SelectedProfile { get; set; }

    public ManagedConfigs ManagedFields { get; set; } = new();

    [Reactive]
    public FluentThemeMode ThemeMode { get; set; }
    
    public bool UseSystemCore { get; set; }
}

public class ManagedConfigs: ReactiveObject
{
    public ManagedConfigValue<int> ExternalControllerPort { get; set; } = new() {Value = 19090, Hide = false, Path = "external-controller"};
    public ManagedConfigValue<int> MixedPort { get; set; } = new() {Value = 27890, Hide = false, Path = "mixed-port"};
    public ManagedConfigValue<bool> Tun { get; set; } = new() {Value = false, Hide = false, Path = "tun.enable"};
    public ManagedConfigValue<bool> AllowLan { get; set; } = new() {Value = false, Hide = false, Path = "allow-lan"};
    public ManagedConfigValue<bool> Ipv6 { get; set; } = new() {Value = false, Hide = false, Path = "ipv6"};
}

public abstract class ManagedConfig: ReactiveObject
{
    public bool Enabled { get; set; }
    public bool Hide { get; set; }
    
    public string Path { get; set; }
}

public class ManagedConfigValue<T> : ManagedConfig
{
    public T? Value { get; set; }
}