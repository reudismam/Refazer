using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using NuGet.Resources;
using NuGet.V3Interop;

namespace NuGet
{
    public static class PackageRepositoryExtensions
    {
        public static IPackage FindPackage(this IPackageRepository repository, string packageId, IVersionSpec versionSpec,
                IPackageConstraintProvider constraintProvider, bool allowPrereleaseVersions, bool allowUnlisted)
        {
            var packages = repository.FindPackages(packageId, versionSpec, allowPrereleaseVersions, allowUnlisted);

            if (constraintProvider != null)
            {
                packages = FilterPackagesByConstraints(constraintProvider, packages, packageId, allowPrereleaseVersions);
            }

            return packages.FirstOrDefault();
        }

        public static IEnumerable<IPackage> FindPackages(
            this IPackageRepository repository,
            string packageId,
            IVersionSpec versionSpec,
            bool allowPrereleaseVersions,
            bool allowUnlisted)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }

            IEnumerable<IPackage> packages = repository.FindPackagesById(packageId)
                                                       .OrderByDescending(p => p.Version);

            if (!allowUnlisted)
            {
                packages = packages.Where(PackageExtensions.IsListed);
            }

            if (versionSpec != null)
            {
                packages = packages.FindByVersion(versionSpec);
            }

            packages = FilterPackagesByConstraints(NullConstraintProvider.Instance, packages, packageId, allowPrereleaseVersions);

            return packages;
        }

        public static IPackage FindPackage(
            this IPackageRepository repository,
            string packageId,
            SemanticVersion version,
            IPackageConstraintProvider constraintProvider,
            bool allowPrereleaseVersions,
            bool allowUnlisted)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }

            // if an explicit version is specified, disregard the 'allowUnlisted' argument
            // and always allow unlisted packages.
            if (version != null)
            {
                allowUnlisted = true;
            }
            else if (!allowUnlisted && (constraintProvider == null || constraintProvider == NullConstraintProvider.Instance))
            {
                var packageLatestLookup = repository as ILatestPackageLookup;
                if (packageLatestLookup != null)
                {
                    IPackage package;
                    if (packageLatestLookup.TryFindLatestPackageById(packageId, allowPrereleaseVersions, out package))
                    {
                        return package;
                    }
                }
            }

            // If the repository implements it's own lookup then use that instead.
            // This is an optimization that we use so we don't have to enumerate packages for
            // sources that don't need to.
            var packageLookup = repository as IPackageLookup;
            if (packageLookup != null && version != null)
            {
                return packageLookup.FindPackage(packageId, version);
            }

            IEnumerable<IPackage> packages = repository.FindPackagesById(packageId);

            packages = packages.ToList()
                               .OrderByDescending(p => p.Version);

            if (!allowUnlisted)
            {
                packages = packages.Where(PackageExtensions.IsListed);
            }

            if (version != null)
            {
                packages = packages.Where(p => p.Version == version);
            }
            else if (constraintProvider != null)
            {
                packages = FilterPackagesByConstraints(constraintProvider, packages, packageId, allowPrereleaseVersions);
            }

            return packages.FirstOrDefault();
        }
    }
}