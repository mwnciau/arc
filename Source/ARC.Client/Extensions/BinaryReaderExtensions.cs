using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Server.Network.Structure;

namespace ARC.Client.Extensions;

public static class BinaryReaderExtensions
{
    public static RestrictionDB ReadRestrictionDB(this BinaryReader reader)
    {
        var restrictionDB = new RestrictionDB();

        restrictionDB.Version = reader.ReadUInt32();
        restrictionDB.OpenStatus = Convert.ToBoolean(reader.ReadUInt32());
        restrictionDB.MonarchID = new ObjectGuid(reader.ReadUInt32());
        restrictionDB.Table = reader.ReadObjectGuidDictionary();

        return restrictionDB;
    }

    public static Dictionary<ObjectGuid,uint> ReadObjectGuidDictionary(this BinaryReader reader)
    {
        var dictionary = new Dictionary<ObjectGuid, uint>();

        int dictionaryLength = reader.ReadUInt16();

        // Contains the number of buckets, a currently hard-coded and unused variable in ACE
        reader.Skip(2);

        for (int i = 0; i < dictionaryLength; i++) {
            dictionary.Add(
                new ObjectGuid(reader.ReadUInt32()),
                reader.ReadUInt32()
            );
        }

        return dictionary;
    }

        public static uint ReadPackedDword(this BinaryReader reader)
    {
        uint dword = BitConverter.ToUInt16(reader.ReadBytes(2));

        if (dword > 32767) {
            // This was packed as 4 bytes
            uint secondPart = BitConverter.ToUInt16(reader.ReadBytes(2));

            dword = ((dword ^ 0x8000) << 16) | secondPart;
        }

        return dword;
    }

    public static uint ReadPackedDwordOfKnownType(this BinaryReader reader, uint type)
    {
        uint dword = reader.ReadPackedDword();

        return dword + type;
    }
}
