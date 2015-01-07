// Guids.cs
// MUST match guids.h
using System;

namespace SPG.IntelliExtract
{
    static class GuidList
    {
        public const string guidIntelliExtractPkgString = "cdc0fc09-d7bf-4487-87dd-93066dfd065d";
        public const string guidIntelliExtractCmdSetString = "7791ad82-0bcf-4b7f-b6b0-1ebda885525a";

        public static readonly Guid guidIntelliExtractCmdSet = new Guid(guidIntelliExtractCmdSetString);
    };
}