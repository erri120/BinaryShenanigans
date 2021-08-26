using System;
using System.Reflection;

namespace BinaryShenanigans.BinaryParser
{
    internal class MemberParsingSettings
    {
        public readonly MemberInfo MemberInfo;
        public readonly bool LittleEndian;

        public MemberParsingSettings(MemberInfo memberInfo, bool littleEndian)
        {
            MemberInfo = memberInfo;
            LittleEndian = littleEndian;
        }
    }
}