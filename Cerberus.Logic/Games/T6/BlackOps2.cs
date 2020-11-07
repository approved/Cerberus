using Cerberus.Logic.Crypto;
using Cerberus.Logic.Extensions;
using Cerberus.Logic.Games.T6.Assets;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cerberus.Logic.Games.T6
{
    public static partial class BlackOps2
    {
        public static XAssetList ReadFastFile(FastFile ff, BinaryReader br)
        {
            int size = br.ReadInt32();

            XFile header = new XFile()
            {
                Size = size,
                ExternalSize = br.ReadInt32(),
                BlockSizes = new long[(int)XFileBlocks.T6Count]
            };

            for (int i = 0; i < (int)XFileBlocks.T6Count; i++)
            {
                header.BlockSizes[i] = br.ReadInt32();
            }

            if (br.BaseStream.Position != br.BaseStream.Length - size)
            {
                throw new InvalidDataException("Idk how we got here");
            }

            XAssetList assetList = new XAssetList
            {
                StringCount = br.ReadInt32(),
                StringPointer = br.ReadInt32(),
                DependsCount = br.ReadInt32(),
                DependsPointer = br.ReadInt32(),
                AssetCount = br.ReadInt32(),
                AssetPointer = br.ReadInt32()
            };

            LoadStringList(assetList, br);

            LoadDependencies(assetList, br);

            LoadAssets(assetList, br);

            return assetList;
        }

        private static void LoadStringList(XAssetList assetList, BinaryReader br)
        {
            if (assetList.StringCount > 0)
            {
                assetList.StringPointerArray = new int[assetList.StringCount];
                for (int i = 0; i < assetList.StringCount; i++)
                {
                    assetList.StringPointerArray[i] = br.ReadInt32();
                }

                foreach (int strPtr in assetList.StringPointerArray)
                {
                    if (strPtr != -1)
                    {
                        continue;
                    }
                    assetList.StringList.Add(br.ReadNativeString());
                }
            }
        }

        private static void LoadDependencies(XAssetList assetList, BinaryReader br)
        {
            if (assetList.DependsCount > 0)
            {
                assetList.DependsPointerArray = new int[assetList.DependsCount];
                for (int i = 0; i < assetList.DependsCount; i++)
                {
                    assetList.DependsPointerArray[i] = br.ReadInt32();
                }

                foreach (int depPtr in assetList.DependsPointerArray)
                {
                    if (depPtr != -1)
                    {
                        continue;
                    }
                    assetList.DependsList.Add(br.ReadNativeString());
                }
            }
        }

        private static void LoadAssets(XAssetList assetList, BinaryReader br)
        {
            if (assetList.AssetCount > 0)
            {
                for (int i = 0; i < assetList.AssetCount; i++)
                {
                    T6XAsset asset = new T6XAsset((T6XAssetType)br.ReadInt32(), br.ReadInt32());
                    assetList.AssetList.Add(asset);
                }

                for (int i = 0; i < assetList.AssetCount; i++)
                {
                    T6XAsset asset = (T6XAsset)assetList.AssetList[i];
                    if (asset.GetHeaderPtr() != -1)
                    {
                        Console.WriteLine($"Asset at index {i} is a pointer asset");
                        return;
                    }

                    switch ((T6XAssetType)asset.GetAssetType())
                    {
                        case T6XAssetType.XModelPieces:
                            break;
                        case T6XAssetType.PhysPreset:
                            break;
                        case T6XAssetType.PhysConstraints:
                            break;
                        case T6XAssetType.DestructibleDef:
                            break;
                        case T6XAssetType.XAnimeParts:
                            break;
                        case T6XAssetType.XModel:
                            break;
                        case T6XAssetType.Material:
                            assetList.Entries.Add(T6Material.LoadMaterial(br));
                            break;
                        case T6XAssetType.TechniqueSet:
                            assetList.Entries.Add(T6MaterialTechniqueSet.LoadSet(br));
                            break;
                        case T6XAssetType.Image:
                            break;
                        case T6XAssetType.Sound:
                            break;
                        case T6XAssetType.SoundPatch:
                            break;
                        case T6XAssetType.ClipMap:
                        case T6XAssetType.ClipMapPVS:
                            break;
                        case T6XAssetType.ComWorld:
                            break;
                        case T6XAssetType.GameWorldSP:
                            break;
                        case T6XAssetType.GameWorldMP:
                            break;
                        case T6XAssetType.MapEnts:
                            break;
                        case T6XAssetType.GFXWorld:
                            break;
                        case T6XAssetType.LightDefe:
                            break;
                        case T6XAssetType.UIMap:
                            break;
                        case T6XAssetType.Font:
                            break;
                        case T6XAssetType.FontIcon:
                            break;
                        case T6XAssetType.MenuList:
                            break;
                        case T6XAssetType.Menu:
                            break;
                        case T6XAssetType.LocalizeEntry:
                            break;
                        case T6XAssetType.Weapon:
                            break;
                        case T6XAssetType.WeaponDef:
                            break;
                        case T6XAssetType.WeaponVariant:
                            break;
                        case T6XAssetType.WeaponFull:
                            break;
                        case T6XAssetType.Attachment:
                            break;
                        case T6XAssetType.AttachmentUnique:
                            break;
                        case T6XAssetType.WeaponCamo:
                            break;
                        case T6XAssetType.SoundDriverGlobals:
                            break;
                        case T6XAssetType.Effects:
                            break;
                        case T6XAssetType.ImpactFX:
                            break;
                        case T6XAssetType.AIType:
                            break;
                        case T6XAssetType.MPType:
                            break;
                        case T6XAssetType.MPBody:
                            break;
                        case T6XAssetType.MPHead:
                            break;
                        case T6XAssetType.Character:
                            break;
                        case T6XAssetType.XModelAlias:
                            break;
                        case T6XAssetType.RawFile:
                            assetList.Entries.Add(new T6RawFile(br));
                            break;
                        case T6XAssetType.StringTable:
                            break;
                        case T6XAssetType.Leaderboard:
                            break;
                        case T6XAssetType.XGlobals:
                            break;
                        case T6XAssetType.DDL:
                            break;
                        case T6XAssetType.Glasses:
                            break;
                        case T6XAssetType.EmblemSet:
                            break;
                        case T6XAssetType.ScriptParseTree:
                            break;
                        case T6XAssetType.KeyValuePairs:
                            break;
                        case T6XAssetType.VehicleDef:
                            break;
                        case T6XAssetType.MemoryBlock:
                            break;
                        case T6XAssetType.AddonMapEnts:
                            break;
                        case T6XAssetType.Trace:
                            break;
                        case T6XAssetType.SkinnedVerts:
                            break;
                        case T6XAssetType.QDB:
                            break;
                        case T6XAssetType.Slug:
                            break;
                        case T6XAssetType.FootstepTable:
                            break;
                        case T6XAssetType.FootstepEffectsTable:
                            break;
                        case T6XAssetType.ZBarrier:
                            break;
                        case T6XAssetType.Count:
                            break;
                        case T6XAssetType.String:
                            break;
                        case T6XAssetType.AssetList:
                            break;
                        case T6XAssetType.Report:
                            break;
                        case T6XAssetType.Depend:
                            break;
                        case T6XAssetType.FullCount:
                            break;
                    }
                }
            }
        }

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
