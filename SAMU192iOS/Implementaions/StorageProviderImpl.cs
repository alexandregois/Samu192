using System;
using System.IO;
using System.Xml.Serialization;
using Foundation;
using SAMU192Core.Interfaces;

namespace SAMU192iOS.Implementaions
{
    public class StorageProviderImpl : IStorageProvider
    {
        public bool Salvar<T>(T obj)
        {
            NSUserDefaults.StandardUserDefaults.SetString(Object2String(obj), obj.GetType().Name);
            NSUserDefaults.StandardUserDefaults.Synchronize();
            return true;
        }

        public T Recuperar<T>(string key)
        {
            string dados = NSUserDefaults.StandardUserDefaults.StringForKey(key);
            return (!string.IsNullOrWhiteSpace(dados) ? String2Object<T>(dados) : Activator.CreateInstance<T>());
        }

        public string Object2String(object data)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                new XmlSerializer(data.GetType()).Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }

        public T String2Object<T>(string dados)
        {
            using (StringReader sr = new StringReader(dados))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(sr);
            }
        }
    }
}