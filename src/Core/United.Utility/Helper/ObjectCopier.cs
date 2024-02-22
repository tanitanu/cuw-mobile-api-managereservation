using System;
using Newtonsoft.Json;

namespace United.Utility.Helper
{
    public static class ObjectCopier
    {
        /// <summary> 
        /// Perform a deep Copy of the object. 
        /// </summary> 
        /// <typeparam name="T">The type of object being copied.</typeparam> 
        /// <param name="source">The object instance to copy.</param> 
        /// <returns>The copied object.</returns> 
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object 
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var obj = JsonConvert.SerializeObject(source);
            return (T)JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
