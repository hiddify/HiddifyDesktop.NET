﻿using System.Collections.Generic;
using ClashGui.Models.Providers;

namespace ClashGui.Interfaces;

public interface IProxyProviderListViewModel
{
    List<ProxyProviderExt> ProxyProviders { get; }
}