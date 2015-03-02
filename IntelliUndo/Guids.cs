// Guids.cs
// MUST match guids.h
using System;

namespace SPG.IntelliUndo
{
    static class GuidList
    {
        public const string guidIntelliUndoPkgString = "19b5f9d6-18d6-445c-88a3-475878694b4f";
        public const string guidIntelliUndoCmdSetString = "8272cf95-2d24-486b-b8e7-570dbf38939d";

        public static readonly Guid guidIntelliUndoCmdSet = new Guid(guidIntelliUndoCmdSetString);
    };
}