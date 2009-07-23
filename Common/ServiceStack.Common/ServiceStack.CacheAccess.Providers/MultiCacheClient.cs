using System;
using System.Collections.Generic;
using ServiceStack.Common.Extensions;
using ServiceStack.Logging;

namespace ServiceStack.CacheAccess.Providers
{
	public class MultiCacheClient : ICacheClient
	{
		private readonly List<ICacheClient> cacheClients;

		public MultiCacheClient(params ICacheClient[] cacheClients)
		{
			if (cacheClients.Length == 0)
			{
				throw new ArgumentNullException("cacheClients");
			}
			this.cacheClients = new List<ICacheClient>(cacheClients);
		}

		public void Dispose()
		{
			cacheClients.ExecAll(client => client.Dispose());
		}

		public bool Remove(string key)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Remove(key), ref firstResult);
			return firstResult;
		}

		public object Get(string key)
		{
			return cacheClients.ExecReturnFirstWithResult(client => client.Get(key));
		}

		public T Get<T>(string key)
		{
			return cacheClients.ExecReturnFirstWithResult(client => client.Get<T>(key));
		}

		public long Increment(string key, uint amount)
		{
			var firstResult = default(long);
			cacheClients.ExecAllWithFirstOut(client => client.Increment(key, amount), ref firstResult);
			return firstResult;
		}

		public long Decrement(string key, uint amount)
		{
			var firstResult = default(long);
			cacheClients.ExecAllWithFirstOut(client => client.Decrement(key, amount), ref firstResult);
			return firstResult;
		}

		public bool Add(string key, object value)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Add(key, value), ref firstResult);
			return firstResult;
		}

		public bool Set(string key, object value)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Set(key, value), ref firstResult);
			return firstResult;
		}

		public bool Replace(string key, object value)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Replace(key, value), ref firstResult);
			return firstResult;
		}

		public bool Add(string key, object value, DateTime expiresAt)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Add(key, value, expiresAt), ref firstResult);
			return firstResult;
		}

		public bool Set(string key, object value, DateTime expiresAt)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Set(key, value, expiresAt), ref firstResult);
			return firstResult;
		}

		public bool Replace(string key, object value, DateTime expiresAt)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.Replace(key, value, expiresAt), ref firstResult);
			return firstResult;
		}

		public bool CheckAndSet(string key, object value, ulong lastModifiedValue)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.CheckAndSet(key, value, lastModifiedValue), ref firstResult);
			return firstResult;
		}

		public bool CheckAndSet(string key, object value, ulong lastModifiedValue, DateTime expiresAt)
		{
			var firstResult = default(bool);
			cacheClients.ExecAllWithFirstOut(client => client.CheckAndSet(key, value, lastModifiedValue, expiresAt), ref firstResult);
			return firstResult;
		}

		public void FlushAll()
		{
			cacheClients.ExecAll(client => client.FlushAll());
		}

		public IDictionary<string, object> Get(IEnumerable<string> keys)
		{
			IDictionary<string, object> firstResult = null;
			cacheClients.ExecAllWithFirstOut(client => client.Get(keys), ref firstResult);
			return firstResult;
		}

		public IDictionary<string, object> Get(IEnumerable<string> keys, out IDictionary<string, ulong> lastModifiedValues)
		{
			foreach (var client in cacheClients)
			{
				try
				{
					var result = client.Get(keys, out lastModifiedValues);
					if (result != null)
					{
						return result;
					}
				}
				catch (Exception ex)
				{
					ExecExtensions.LogError(client.GetType(), "Get", ex);
				}
			}

			lastModifiedValues = null;
			return null;
		}
	}
}