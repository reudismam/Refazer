// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Common.Utils;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal enum UpdateCommandKind
    {
        Dynamic,
        Function,
    }

    // <summary>
    // Class storing the result of compiling an instance DML command.
    // </summary>
    internal abstract class UpdateCommand : IComparable<UpdateCommand>, IEquatable<UpdateCommand>
    {
        protected UpdateCommand(UpdateTranslator translator, PropagatorResult originalValues, PropagatorResult currentValues)
        {
            OriginalValues = originalValues;
            CurrentValues = currentValues;
            Translator = translator;
        }

        // When it is not possible to order two commands based on their contents, we assign an 'ordering identifier'
        // so that one will consistently precede the other.
        private static int OrderingIdentifierCounter;
        private int _orderingIdentifier;

        // <summary>
        // Gets all identifiers (key values basically) generated by this command. For instance,
        // @@IDENTITY values.
        // </summary>
        internal abstract IEnumerable<int> OutputIdentifiers { get; }

        // <summary>
        // Gets all identifiers required by this command.
        // </summary>
        internal abstract IEnumerable<int> InputIdentifiers { get; }

        // <summary>
        // Gets table (if any) associated with the current command. FunctionUpdateCommand has no table.
        // </summary>
        internal virtual EntitySet Table
        {
            get { return null; }
        }

        // <summary>
        // Gets type of command.
        // </summary>
        internal abstract UpdateCommandKind Kind { get; }

        // <summary>
        // Gets original values of row/entity handled by this command.
        // </summary>
        internal PropagatorResult OriginalValues { get; private set; }

        // <summary>
        // Gets current values of row/entity handled by this command.
        // </summary>
        internal PropagatorResult CurrentValues { get; private set; }

        // <summary>
        // Gets the <see cref="UpdateTranslator" /> used to create this command.
        // </summary>
        protected UpdateTranslator Translator { get; private set; }

        // <summary>
        // Yields all state entries contributing to this command. Used for error reporting.
        // </summary>
        // <param name="translator"> Translator context. </param>
        // <returns> Related state entries. </returns>
        internal abstract IList<IEntityStateEntry> GetStateEntries(UpdateTranslator translator);

        // <summary>
        // Determines model level dependencies for the current command. Dependencies are based
        // on the model operations performed by the command (adding or deleting entities or relationships).
        // </summary>
        internal void GetRequiredAndProducedEntities(
            UpdateTranslator translator,
            KeyToListMap<EntityKey, UpdateCommand> addedEntities,
            KeyToListMap<EntityKey, UpdateCommand> deletedEntities,
            KeyToListMap<EntityKey, UpdateCommand> addedRelationships,
            KeyToListMap<EntityKey, UpdateCommand> deletedRelationships)
        {
            var stateEntries = GetStateEntries(translator);

            foreach (var stateEntry in stateEntries)
            {
                if (!stateEntry.IsRelationship)
                {
                    if (stateEntry.State
                        == EntityState.Added)
                    {
                        addedEntities.Add(stateEntry.EntityKey, this);
                    }
                    else if (stateEntry.State
                             == EntityState.Deleted)
                    {
                        deletedEntities.Add(stateEntry.EntityKey, this);
                    }
                }
            }

            // process foreign keys
            if (null != OriginalValues)
            {
                // if a foreign key being deleted, it 'frees' or 'produces' the referenced key
                AddReferencedEntities(translator, OriginalValues, deletedRelationships);
            }
            if (null != CurrentValues)
            {
                // if a foreign key is being added, if requires the referenced key
                AddReferencedEntities(translator, CurrentValues, addedRelationships);
            }

            // process relationships
            foreach (var stateEntry in stateEntries)
            {
                if (stateEntry.IsRelationship)
                {
                    // only worry about the relationship if it is being added or deleted
                    var isAdded = stateEntry.State == EntityState.Added;
                    if (isAdded || stateEntry.State == EntityState.Deleted)
                    {
                        var record = isAdded ? stateEntry.CurrentValues : stateEntry.OriginalValues;
                        Debug.Assert(2 == record.FieldCount, "non-binary relationship?");
                        var end1 = (EntityKey)record[0];
                        var end2 = (EntityKey)record[1];

                        // relationships require the entity when they're added and free the entity when they're deleted...
                        var affected = isAdded ? addedRelationships : deletedRelationships;

                        // both ends are being modified by the relationship
                        affected.Add(end1, this);
                        affected.Add(end2, this);
                    }
                }
            }
        }

        private void AddReferencedEntities(
            UpdateTranslator translator, PropagatorResult result, KeyToListMap<EntityKey, UpdateCommand> referencedEntities)
        {
            foreach (var property in result.GetMemberValues())
            {
                if (property.IsSimple
                    && property.Identifier != PropagatorResult.NullIdentifier
                    &&
                    (PropagatorFlags.ForeignKey == (property.PropagatorFlags & PropagatorFlags.ForeignKey)))
                {
                    foreach (var principal in translator.KeyManager.GetDirectReferences(property.Identifier))
                    {
                        PropagatorResult owner;
                        if (translator.KeyManager.TryGetIdentifierOwner(principal, out owner)
                            &&
                            null != owner.StateEntry)
                        {
                            Debug.Assert(!owner.StateEntry.IsRelationship, "owner must not be a relationship");
                            referencedEntities.Add(owner.StateEntry.EntityKey, this);
                        }
                    }
                }
            }
        }

        // <summary>
        // Executes the current update command.
        // All server-generated values are added to the generatedValues list. If those values are identifiers, they are
        // also added to the identifierValues dictionary, which associates proxy identifiers for keys in the session
        // with their actual values, permitting fix-up of identifiers across relationships.
        // </summary>
        // <param name="identifierValues"> Aggregator for identifier values (read for InputIdentifiers; write for OutputIdentifiers </param>
        // <param name="generatedValues"> Aggregator for server generated values. </param>
        // <returns> Number of rows affected by the command. </returns>
        internal abstract long Execute(
            Dictionary<int, object> identifierValues,
            List<KeyValuePair<PropagatorResult, object>> generatedValues);

#if !NET40

        // <summary>
        // An asynchronous version of Execute, which executes the current update command.
        // All server-generated values are added to the generatedValues list. If those values are identifiers, they are
        // also added to the identifierValues dictionary, which associates proxy identifiers for keys in the session
        // with their actual values, permitting fix-up of identifiers across relationships.
        // </summary>
        // <param name="identifierValues"> Aggregator for identifier values (read for InputIdentifiers; write for OutputIdentifiers </param>
        // <param name="generatedValues"> Aggregator for server generated values. </param>
        // <param name="cancellationToken"> The token to monitor for cancellation requests. </param>
        // <returns> Number of rows affected by the command. </returns>
        internal abstract Task<long> ExecuteAsync(
            Dictionary<int, object> identifierValues,
            List<KeyValuePair<PropagatorResult, object>> generatedValues, CancellationToken cancellationToken);

#endif

        // <summary>
        // Implementation of CompareTo for concrete subclass of UpdateCommand.
        // </summary>
        internal abstract int CompareToType(UpdateCommand other);

        // <summary>
        // Provides a suggested ordering between two commands. Ensuring a consistent ordering is important to avoid deadlocks
        // between two clients because it means locks are acquired in the same order where possible. The ordering criteria are as
        // follows (and are partly implemented in the CompareToType method). In some cases there are specific secondary
        // reasons for the order (e.g. operator kind), but for the most case we just care that a consistent ordering
        // is applied:
        // - The kind of command (dynamic or function). This is an arbitrary criteria.
        // - The kind of operator (insert, update, delete). See <see cref="ModificationOperator" /> for details of the ordering.
        // - The target of the modification (table for dynamic, set for function).
        // - Primary key for the modification (table key for dynamic, entity keys for function).
        // If it is not possible to differentiate between two commands (e.g., where the user is inserting entities with server-generated
        // primary keys and has not given explicit values), arbitrary ordering identifiers are assigned to the commands to
        // ensure CompareTo is well-behaved (doesn't return 0 for different commands and suggests consistent ordering).
        // </summary>
        public int CompareTo(UpdateCommand other)
        {
            // If the commands are the same (by reference), return 0 immediately. Otherwise, we try to find (and eventually
            // force) an ordering between them by returning a value that is non-zero.
            if (Equals(other))
            {
                return 0;
            }
            Debug.Assert(null != other, "comparing to null UpdateCommand");
            var result = (int)Kind - (int)other.Kind;
            if (0 != result)
            {
                return result;
            }

            // defer to specific type for other comparisons...
            result = CompareToType(other);
            if (0 != result)
            {
                return result;
            }

            // if the commands are indistinguishable, assign arbitrary identifiers to them to ensure consistent ordering
            unchecked
            {
                if (_orderingIdentifier == 0)
                {
                    _orderingIdentifier = Interlocked.Increment(ref OrderingIdentifierCounter);
                }
                if (other._orderingIdentifier == 0)
                {
                    other._orderingIdentifier = Interlocked.Increment(ref OrderingIdentifierCounter);
                }

                return _orderingIdentifier - other._orderingIdentifier;
            }
        }

        #region IEquatable: note that we use reference equality

        public bool Equals(UpdateCommand other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
