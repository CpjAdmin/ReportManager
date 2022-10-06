using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Encriptar
    {
        public Encriptar() { }

        public string Crypto(string texto)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(texto);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    texto = Convert.ToBase64String(ms.ToArray());
                }
            }
            return texto;
        }

        public string Decrypto(string texto)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            texto = texto.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(texto);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    texto = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return texto;
        }

        public string CryptoSIC(string clave)
        {
            string vpcclaveus = clave;
            string vlcfuentes = "0123456789ABCDEFGHIJKLMN¥OPQRSTUVWXYZabcdefghijklmn¤opqrstuvwxyz";
            string vlcencript = "MNBVCXZÑLKJHGFDSAPOIUYTREWQmnbvcxzñlkjhgfdsapoiuytrewq9876543210";

            int vlclarpass = clave.Length;
            string vlclaencri = "";

            for (int i = 0; i < vlclarpass; i++)
            {
                for (int j = 0; j < vlcfuentes.Length; j++)
                {
                    if (vpcclaveus.Substring(i, 1) == vlcfuentes.Substring(j, 1))
                    {
                        vlclaencri = vlclaencri + vlcencript.Substring(j, 1);
                        break;
                    }
                }
            }
            return vlclaencri;
        }
    }
}
