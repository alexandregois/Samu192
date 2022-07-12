using System;
using System.IO;
using System.Security.Cryptography;

namespace SAMU192Core.Utils
{
    internal class Cryptography
    {
        private DESCryptoServiceProvider ClientDESCryptoServiceProvider = new DESCryptoServiceProvider();
        private string _SwordFish;


        public Cryptography()
        {
        }

        public string Password { set => _SwordFish = value; }

        public string EncryptString(string InputString)
        {
            byte[] Salt = new byte[8];
            MemoryStream OutputStream = new MemoryStream();
            CryptoStream encStream;
            StreamWriter Writer;

            try
            {
                // Gera SALT
                Salt = SetEncKey(_SwordFish);
                OutputStream.Write(Salt, 0, 8);
                OutputStream.Flush();
                // Criptografa
                encStream = new CryptoStream(OutputStream, ClientDESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                Writer = new StreamWriter(encStream);
                Writer.Write(InputString);
                Writer.Flush();
                encStream.FlushFinalBlock();
                OutputStream.Flush();

                return Convert.ToBase64String(OutputStream.GetBuffer(), 0, System.Convert.ToInt32(OutputStream.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Writer = null;
                Salt = null;
                OutputStream = null;
                encStream = null;
            }
        }

        public string DecryptString(string InputString)
        {
            byte[] Salt = new byte[8];
            MemoryStream OutputStream;
            CryptoStream encStream;
            StreamReader Reader;
            byte[] Buffer;

            try
            {
                // Gera SALT
                Buffer = Convert.FromBase64String(InputString);
                OutputStream = new MemoryStream(Buffer);
                OutputStream.Read(Salt, 0, 8);
                SetDecKey(Salt);
                // Criptografa
                encStream = new CryptoStream(OutputStream, ClientDESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Read);
                Reader = new StreamReader(encStream);

                return Reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Reader = null;
                Salt = null;
                OutputStream = null;
                encStream = null;
            }
        }



        // Returns the Salt to be saved into the stream
        private byte[] SetEncKey(string pwd)
        {
            Random rnd;
            byte[] byts = new byte[8]; // 8 bytes
            try
            {
                rnd = new Random();
                rnd.NextBytes(byts);
                SetKey(_SwordFish, byts);

                return byts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pwd = null;
                rnd = null;
                byts = null;
            }
        }

        private void SetDecKey(byte[] Salt)
        {
            try
            {
                SetKey(_SwordFish, Salt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Salt = null;
            }
        }

        private void SetKey(string pwd, byte[] salt)
        {
            Rfc2898DeriveBytes Deriver;
            try
            {
                Deriver = new Rfc2898DeriveBytes(pwd, salt);

                ClientDESCryptoServiceProvider.Key = Deriver.GetBytes(ClientDESCryptoServiceProvider.LegalKeySizes[0].MaxSize / 8);
                ClientDESCryptoServiceProvider.IV = Deriver.GetBytes(ClientDESCryptoServiceProvider.BlockSize / 8);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Deriver = null;
            }
        }
    }
}
