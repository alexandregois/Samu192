using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using SAMU192Core.Interfaces;

namespace SAMU192Droid.Implementaitons
{
    public class StorageProviderImpl : IStorageProvider
    {
        static ISharedPreferences SharedPreferences = Application.Context.GetSharedPreferences("Prefs", FileCreationMode.Private);

        public bool Salvar<T>(T obj)
        {
            var editor = SharedPreferences.Edit();
            editor.PutString(obj.GetType().Name, Object2String(obj));
            editor.Commit();
            return true;
        }

        public T Recuperar<T>(string key)
        {
            string dados = SharedPreferences.GetString(key, String.Empty).Trim();
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