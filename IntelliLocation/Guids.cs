// Guids.cs
// MUST match guids.h
using System;

namespace SPG.IntelliLocation
{
    static class GuidList
    {
        public const string guidIntelliLocationPkgString = "57b6456a-ddc0-4713-a035-a3ae3117ef83";
        public const string guidIntelliLocationCmdSetString = "a081c543-db81-497a-a25e-f3e26e3bf1e0";

        public static readonly Guid guidIntelliLocationCmdSet = new Guid(guidIntelliLocationCmdSetString);
    };
}