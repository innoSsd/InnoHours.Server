using System;

namespace InnoHours.Server.Extensions
{
    public static class ObjectExtensions
    {
        public static TOut Let<TOut, TIn>(this TIn obj, Func<TIn, TOut> function)
        {
            return function(obj);
        }
    }
}