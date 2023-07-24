using Android.App;
using Com.Google.Gson;
using Com.Octo.Android.Robospice;
using Com.Octo.Android.Robospice.Persistence;
using Com.Octo.Android.Robospice.Persistence.Exception;
using Com.Octo.Android.Robospice.Persistence.Retrofit;
using Com.Octo.Android.Robospice.Request;
using Com.Octo.Android.Robospice.Request.Listener;
using Com.Octo.Android.Robospice.Request.Retrofit;
using Java.Io;
using Java.Util;
using TrainsSchedule.TrainsSchedule.API;
using Retrofit;
using Retrofit.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Base
{
    public abstract class BaseNetworkService : SpiceService
    {
        private Dictionary<Class<TWildcardTodo>, object> retrofitInterfaceToServiceMap = new HashMap<Class<TWildcardTodo>, object>();
        private RestAdapter restAdapter;
        protected IList<Class<TWildcardTodo>> retrofitInterfaceList = new List<Class<TWildcardTodo>>();
        private Converter mConverter;
        public override void OnCreate()
        {
            base.OnCreate();

            //        Ln.getConfig().setLoggingLevel(Log.ERROR);
            RestAdapter.Builder builder = CreateRetrofitBuilder();
            restAdapter = builder.Build();
            AddRetrofitInterface(typeof(YandexRaspAPI));
        }

        public override CacheManager CreateCacheManager(Application application)
        {
            CacheManager cacheManager = new CacheManager();
            cacheManager.AddPersister(new GsonRetrofitObjectPersisterFactory(application, GetConverter(), GetCacheFolder()));
            return cacheManager;
        }

        public virtual File GetCacheFolder()
        {
            return null;
        }

        protected abstract RestAdapter.Builder CreateRetrofitBuilder();
        protected virtual Converter CreateConverter()
        {
            return new GsonConverter(new Gson());
        }

        protected Converter GetConverter()
        {
            if (mConverter == null)
            {
                mConverter = CreateConverter();
            }

            return mConverter;
        }

        protected virtual T GetRetrofitService<T>(Class<T> serviceClass)
        {
            T service = (T)retrofitInterfaceToServiceMap[serviceClass];
            if (service == null)
            {
                service = restAdapter.Create(serviceClass);
                retrofitInterfaceToServiceMap.Put(serviceClass, service);
            }

            return service;
        }

        public override void AddRequest(CachedSpiceRequest<TWildcardTodo> request, HashSet<RequestListener<TWildcardTodo>> listRequestListener)
        {
            if (request.GetSpiceRequest() is RetrofitSpiceRequest)
            {
                RetrofitSpiceRequest retrofitSpiceRequest = (RetrofitSpiceRequest)request.GetSpiceRequest();
                retrofitSpiceRequest.SetService(GetRetrofitService(retrofitSpiceRequest.GetRetrofitedInterfaceClass()));
            }

            base.AddRequest(request, listRequestListener);
        }

        public IList<Class<TWildcardTodo>> GetRetrofitInterfaceList()
        {
            return retrofitInterfaceList;
        }

        protected virtual void AddRetrofitInterface(Class<TWildcardTodo> serviceClass)
        {
            retrofitInterfaceList.Add(serviceClass);
        }
    }
}