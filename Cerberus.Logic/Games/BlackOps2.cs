using Cerberus.Logic.Crypto;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cerberus.Logic.Games
{
    public partial class BlackOps2
    {
        public static void Decompress(FastFile fastfile, BinaryReader br, BinaryWriter bw)
        {
            byte[] ivTable = new byte[16000];
            int[] ivCounter = new int[4] { 1, 1, 1, 1 };

            FillIVTable(ivTable, Encoding.UTF8.GetBytes(fastfile.GetName()));

            int sectionIndex = 0;
            Salsa20 salsa = new Salsa20
            {
                Key = fastfile.GetPlatform() switch
                {
                    Platform.PC => PCSalsaKey,
                    Platform.Playstation => PlaystationSalsaKey,
                    Platform.Xbox => XboxSalsaKey,
                    _ => throw new NotImplementedException("Unknown platform found. Can not find salsa key.")
                }
            };

            int size;
            while ((size = br.ReadInt32()) != 0)
            {
                salsa.IV = GetIV(sectionIndex % 4, ivTable, ivCounter);

                ICryptoTransform? decryptor = salsa.CreateDecryptor();

                byte[] decryptedData = decryptor.TransformFinalBlock(br.ReadBytes(size), 0, size);

                bw.Write(Utility.Deflate(decryptedData).ToArray());

                using (SHA1 sha1 = SHA1.Create())
                {
                    UpdateIVTable(sectionIndex % 4, sha1.ComputeHash(decryptedData), ivTable, ivCounter);
                }

                sectionIndex++;
            }
        }

        private static void UpdateIVTable(int index, byte[] hash, byte[] ivTable, int[] ivCounter)
        {
            for (int i = 0; i < 20; i += 5)
            {
                int value = (index + 4 * ivCounter[index]) % 800 * 5;
                for (int j = 0; j < 5; j++)
                {
                    ivTable[4 * value + j + i] ^= hash[i + j];
                }
            }

            ivCounter[index]++;
        }

        private static byte[] GetIV(int index, byte[] ivTable, int[] ivCounter)
        {
            byte[] iv = new byte[8];
            int arrayIndex = (index + 4 * (ivCounter[index] - 1)) % 800 * 20;
            Array.Copy(ivTable, arrayIndex, iv, 0, 8);
            return iv;
        }

        private static void FillIVTable(byte[] ivTable, byte[] nameKey)
        {
            int addDiv = 0;
            for (int i = 0; i < ivTable.Length; i += nameKey.Length * 4)
            {
                for (int j = 0; j < nameKey.Length * 4; j += 4)
                {
                    if (i + addDiv >= ivTable.Length || i + j >= ivTable.Length)
                    {
                        return;
                    }

                    if (j > 0)
                    {
                        addDiv = j / 4;
                    }
                    else
                    {
                        addDiv = 0;
                    }

                    for (int k = 0; k < 4; k++)
                    {
                        ivTable[i + j + k] = nameKey[addDiv];
                    }
                }
            }
        }
    }
}
