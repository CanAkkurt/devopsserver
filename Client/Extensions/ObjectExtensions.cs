﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetQueryString(this object obj)
        {
            var result = new List<string>();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                var enumerable = value as ICollection;
                if (enumerable != null)
                {
                    result.AddRange(from object v in enumerable select string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(v.ToString())));
                }
                else
                {
                    result.Add(string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(value.ToString())));
                }
            }

            return string.Join("&", result.ToArray());

            //var properties = from p in obj.GetType().GetProperties()
            //                     where p.GetValue(obj, null) != null
            //                     select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            //    return string.Join("&", properties.ToArray());
        }
    }
}
