﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace BTCPayServer.Services.Rates
{
    public class CachedDefaultRateProviderFactory : IRateProviderFactory
    {
        IMemoryCache _Cache;
        ConcurrentDictionary<string, IRateProvider> _Providers = new ConcurrentDictionary<string, IRateProvider>();

        public IMemoryCache Cache
        {
            get
            {
                return _Cache;
            }
        }

        public CachedDefaultRateProviderFactory(IMemoryCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            _Cache = cache;
        }

        public IRateProvider RateProvider { get; set; }

        public TimeSpan CacheSpan { get; set; } = TimeSpan.FromMinutes(1.0);
        public IRateProvider GetRateProvider(BTCPayNetwork network)
        {
            return _Providers.GetOrAdd(network.CryptoCode, new CachedRateProvider(network.CryptoCode, RateProvider ?? network.DefaultRateProvider, _Cache) { CacheSpan = CacheSpan });
        }
    }
}
