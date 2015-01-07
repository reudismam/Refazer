// Guids.cs
// MUST match guids.h
using System;

namespace SPG.Refactor
{
    static class GuidList
    {
        public const string guidRefactorPkgString = "1dc4bb3f-e037-4803-af99-3ae94cff28f4";
        public const string guidRefactorCmdSetString = "c022363e-0588-43a8-83f1-075fe1000447";

        public static readonly Guid guidRefactorCmdSet = new Guid(guidRefactorCmdSetString);
    };
}