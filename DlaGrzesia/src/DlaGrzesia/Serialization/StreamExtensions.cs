using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DlaGrzesia.Serialization
{
    internal static class StreamExtensions
    {
        public static bool ReadBool(this Stream stream)
        {
            var @byte = stream.ReadByte();
            return @byte != 0;
        }

        public static T ReadStruct<T>(this Stream stream) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = new byte[size];

            stream.Read(buffer);

            var ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(buffer, 0, ptr, size);
                return Marshal.PtrToStructure<T>(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static int ReadInt(this Stream stream)
        {
            TryReadInt(stream, out var value);
            return value;
        }

        public static bool TryReadInt(this Stream stream, out int value)
        {
            Span<byte> buffer = stackalloc byte[4];
            var success = stream.Read(buffer) == 4;
            value = success ? BitConverter.ToInt32(buffer) : default;
            return success;
        }

        public static Type ReadType(this Stream stream)
        {
            var typeName = ReadVarchar(stream);
            return Type.GetType(typeName);
        }

        public static string ReadVarchar(this Stream stream)
        {
            TryReadVarchar(stream, out var value);
            return value;
        }

        public static bool TryReadVarchar(this Stream stream, out string value)
        {
            if (TryReadInt(stream, out var length))
            {
                Span<byte> buffer = stackalloc byte[length];
                stream.Read(buffer);
                value = Encoding.UTF8.GetString(buffer);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public static void WriteBool(this Stream stream, bool value)
        {
            stream.WriteByte(value ? 1 : 0);
        }

        public static void WriteInt(this Stream stream, int value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void WriteType(this Stream stream, Type value)
        {
            WriteVarchar(stream, value.FullName);
        }

        public static void WriteVarchar(this Stream stream, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            stream.WriteInt(bytes.Length);
            stream.Write(bytes);
        }

        public static void WriteStruct<T>(this Stream stream, T value) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(value, ptr, true);
                Marshal.Copy(ptr, buffer, 0, size);
                stream.Write(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
