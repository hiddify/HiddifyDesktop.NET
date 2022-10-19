﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ClashGui.Clash.Models.Proxies;
using ClashGui.Interfaces;
using ClashGui.Models.Proxies;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;

namespace ClashGui.ViewModels;

public class ProxyGroupViewModel : ViewModelBase, IProxyGroupViewModel
{
    private readonly ProxyGroup _proxyGroup;

    public ProxyGroupViewModel(ProxyGroup proxyGroup)
    {
        _proxyGroup = proxyGroup;
        SelectedProxy = _proxyGroup.Now == null
            ? null
            : new SelectProxy {Group = _proxyGroup.Name, Proxy = _proxyGroup.Now};

        this.WhenAnyValue(d => d.SelectedProxy)
            .WhereNotNull()
            .Skip(1)
            .Subscribe( d =>
            {
                if (Selectable)
                {
                    GlobalConfigs.ClashControllerApi.SelectProxy(d.Group, new UpdateProxyRequest() {Name = d.Proxy}).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                {
                    SelectedProxy = _proxyGroup.Now == null
                        ? null
                        : new SelectProxy {Group = _proxyGroup.Name, Proxy = _proxyGroup.Now};
                }
            });

    }

    public string Name => _proxyGroup.Name;
    public string Type => _proxyGroup.Type;

    public bool Selectable => _proxyGroup.Type == "Selector";

    public IEnumerable<SelectProxy> Proxies =>
        _proxyGroup.All.Select(p => new SelectProxy {Group = _proxyGroup.Name, Proxy = p}).ToList();

    [Reactive]
    public SelectProxy? SelectedProxy { get; set; }

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