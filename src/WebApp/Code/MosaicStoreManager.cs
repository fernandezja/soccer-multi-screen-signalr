using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Ogyke.Core;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Code
{
    public  class MosaicStoreManager
    {
        private IMemoryCache _cache;

        public MosaicStoreManager(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        private  readonly string MOSAIC_STORE_KEY = "MosaicStoreKey";
        private  readonly string SWIPERELAYER_STORE_KEY = "SwipeRelayerKey";

        private  MosaicStore _mosaicStore;
        private  SwipeRelayer _swipeRelayer;

        public  MosaicStore MosaicStore
        {
            get
            {
                return CacheTryGetValueSetMosaicStore();
               
            }
        }


        public SwipeRelayer SwipeRelayer
        {
            get
            {
                var swipeRelayer = CacheTryGetValueSetSwipeRelayer();
                return swipeRelayer.SetMosaicStore(MosaicStore);
            }
        }


        private MosaicStore CacheTryGetValueSetMosaicStore()
        {
            MosaicStore cacheEntry;

           
            if (!_cache.TryGetValue(MOSAIC_STORE_KEY, out cacheEntry))
            {
                cacheEntry = new MosaicStore();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(120));
               
                _cache.Set(MOSAIC_STORE_KEY, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }

        private  SwipeRelayer CacheTryGetValueSetSwipeRelayer()
        {
            SwipeRelayer cacheEntry;


            if (!_cache.TryGetValue(SWIPERELAYER_STORE_KEY, out cacheEntry))
            {
                cacheEntry = new SwipeRelayer();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(120));

                _cache.Set(SWIPERELAYER_STORE_KEY, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}
