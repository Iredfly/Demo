using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DiYi.Demo.Service
{
    public class RedisService
    {
        private static readonly string ConnStr = ConfigurationManager.ConnectionStrings["Redis"].ConnectionString;
        public static ConnectionMultiplexer cm = null;
        public RedisService()
        {
            if (cm == null || !cm.IsConnected)
            {
                cm = ConnectionMultiplexer.Connect(ConnStr);
            }
            db = cm.GetDatabase();
        }

        public static readonly JsonSerializerSettings jsonConfig = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };


        public IDatabase db = null;
        /// <summary>
        /// 设置缓存，没有添加，有则更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">有效期(0永不过期)</param>
        /// <param name="unit">有效期单位(1天 2时 3分 4秒 5毫秒)</param>
        public bool Set(string key, string value, int expiry = 0, int unit = 1)
        {
            if (expiry > 0)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(1);
                switch (unit)
                {
                    case 1:
                        ts = TimeSpan.FromDays(expiry); break;
                    case 2:
                        ts = TimeSpan.FromHours(expiry); break;
                    case 3:
                        ts = TimeSpan.FromMinutes(expiry); break;
                    case 4:
                        ts = TimeSpan.FromSeconds(expiry); break;
                    case 5:
                        ts = TimeSpan.FromMinutes(expiry); break;
                    default:
                        break;
                }

                return db.StringSet(key, value, ts);
            }
            else
            {
                return db.StringSet(key, value);
            }
        }

        #region  插入Redis   

        /// <summary>
        /// 插入Redis，永不过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Insert(string key, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            db.StringSet(key, jsonData);
        }

        public void Insert(string key, object data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = JsonConvert.SerializeObject(data);
            db.StringSet(key, jsonData, timeSpan);
        }

        public void Insert(string key, object data, DateTime cacheTime)
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = JsonConvert.SerializeObject(data);
            db.StringSet(key, jsonData, timeSpan);
        }

        public void Insert<T>(string key, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            db.StringSet(key, jsonData);
        }

        public void Insert<T>(string key, T data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = JsonConvert.SerializeObject(data);
            db.StringSet(key, jsonData, timeSpan);
        }

        public void Insert<T>(string key, T data, DateTime cacheTime)
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = JsonConvert.SerializeObject(data);
            db.StringSet(key, jsonData, timeSpan);
        }

        #endregion


        /// <summary>
        /// 向List中添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="mode">1:left  2right</param>
        public void ListPush<T>(string key, T data, int mode = 1)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            if (mode == 1)
            {
                db.ListLeftPush(key, jsonData);
            }
            else
            {
                db.ListRightPush(key, jsonData);
            }
        }

        public long ListDelete(string key, string value)
        {

            return db.ListRemove(key, value);
        }


        /// <summary>
        /// 获取缓存值, key不存在时返回null
        /// </summary>
        public string Get(string key)
        {
            return db.StringGet(key);
        }

        public bool Remove(string key)
        {
            return db.KeyDelete(key);
        }




        public T Get<T>(string key)
        {
            DateTime begin = DateTime.Now;
            var cacheValue = db.StringGet(key);
            DateTime endCache = DateTime.Now;
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = JsonConvert.DeserializeObject<T>(cacheValue, jsonConfig);
            }
            return value;
        }





        //public bool Set(string key, object value)
        //{
        //    return db.StringSet(key, Serialize(value));
        //}

        private T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        private byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }
    }
}
