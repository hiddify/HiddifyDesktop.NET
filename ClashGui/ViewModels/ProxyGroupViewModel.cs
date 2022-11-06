﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ClashGui.Interfaces;
using ClashGui.Models.Proxies;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ClashGui.Clash.Models.Proxies;
using ReactiveUI.Fody.Helpers;

namespace ClashGui.ViewModels;

public class ProxyGroupViewModel : ViewModelBase, IProxyGroupViewModel
{
    private readonly ProxyGroup _proxyGroup;
    private Func<string, string, Task> _setProxy;

    public ProxyGroupViewModel(ProxyGroup proxyGroup, Func<string, string, Task> setProxy)
    {
        _proxyGroup = proxyGroup;
        _setProxy = setProxy;
        SelectedProxy = _proxyGroup.Now == null
            ? null
            : new SelectProxy {Group = _proxyGroup.Name, Proxy = _proxyGroup.Now};
        Enabled = _proxyGroup.Type == ProxyGroupType.Selector;

        this.WhenAnyValue(d => d.SelectedProxy)
            .WhereNotNull()
            .Skip(1)
            .Subscribe(d =>
            {
                _setProxy(d.Group, d.Proxy)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
            });
    }

    public string Name => _proxyGroup.Name;
    public ProxyGroupType Type => _proxyGroup.Type;


    public IEnumerable<SelectProxy> Proxies =>
        _proxyGroup.All.Select(p => new SelectProxy {Group = _proxyGroup.Name, Proxy = p}).ToList();

    [Reactive]
    public SelectProxy? SelectedProxy { get; set; }

    public bool Enabled { get; }

    public override bool Equals(object? obj)
    {
        if (obj is ProxyGroupViewModel other)
        {
            return _proxyGroup.All.SequenceEqual(other._proxyGroup.All)
                   && Equals(_proxyGroup.Name, other._proxyGroup.Name)
                   && Equals(_proxyGroup.Type, other._proxyGroup.Type)
                   && Equals(_proxyGroup.Now, other._proxyGroup.Now);
        }

        return false;
    }
}