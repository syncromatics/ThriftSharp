﻿// Copyright (c) 2014-15 Solal Pirelli
// This code is licensed under the MIT License (see Licence.txt for details).

using System;
using System.Reflection;
using ThriftSharp.Internals;

namespace ThriftSharp
{
    /// <summary>
    /// Raised when a parsing error occurs.
    /// </summary>
    public sealed class ThriftParsingException : Exception
    {
        /// <summary>
        /// Prevents a default instance of the ThriftParsingException class from being created.
        /// This is meant to force the use one of the static factory methods.
        /// </summary>
        private ThriftParsingException( string message, params object[] args )
            : base( string.Format( message, args ) ) { }


        internal static ThriftParsingException EnumWithoutAttribute( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The enum type '{0}' is not part of a Thrift interface definition."
                                             + Environment.NewLine
                                             + "If this is unintentional, mark it with the ThriftEnumAttribute attribute.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException NonInt32Enum( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The enum type '{0}' has an underlying type different from Int32."
                                             + Environment.NewLine
                                             + "Only enums whose underlying type is Int32 are supported.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException NotAConcreteType( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The type '{0}' is not a concrete type."
                                             + Environment.NewLine
                                             + "Only concrete types can be used in Thrift interfaces.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException StructWithoutAttribute( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The class or struct type '{0}' is not part of a Thrift interface definition."
                                             + Environment.NewLine
                                             + "If this is unintentional, mark it with the ThriftStructAttribute attribute.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException RequiredNullableField( PropertyInfo propertyInfo )
        {
            return new ThriftParsingException( "The Thrift field '{0}' is required, but its type is nullable."
                                             + Environment.NewLine
                                             + "This is not supported. Please use non-nullable types for required value fields.",
                                               propertyInfo.Name );
        }

        internal static ThriftParsingException OptionalValueField( PropertyInfo propertyInfo )
        {
            return new ThriftParsingException( "The Thrift field '{0}' is optional, but its type is a value type."
                                             + Environment.NewLine
                                             + "This is not supported. Please use Nullable<T> for optional value fields.",
                                               propertyInfo.Name );
        }

        internal static ThriftParsingException UnknownValueType( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The type '{0}' is an user-defined value type."
                                             + Environment.NewLine
                                             + "The only available value types are Thrift primitive types and nullable types."
                                             + Environment.NewLine
                                             + "If this is unintentional, change the type to a reference type.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException UnsupportedMap( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The map type '{0}' is not supported."
                                             + Environment.NewLine
                                             + "Supported map types are IDictionary<TKey, TValue> "
                                             + "and any concrete implementation with a parameterless constructor.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException UnsupportedSet( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The set type '{0}' is not supported."
                                             + Environment.NewLine
                                             + "Supported set types are ISet<T> and any concrete implementation with a parameterless constructor.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException UnsupportedList( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The list type '{0}' is not supported."
                                             + Environment.NewLine
                                             + "Supported list types are arrays, IList<T> "
                                             + "and any concrete implementation of that interface with a parameterless constructor.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException CollectionWithOrthogonalInterfaces( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The collection type '{0}' implements more than one of IDictionary<TKey, TValue>, ISet<T> and IList<T>."
                                             + Environment.NewLine
                                             + "This is not supported. Please only use collections implementing exactly one of these interfaces, or an array.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException ParameterWithoutAttribute( ParameterInfo info )
        {
            return new ThriftParsingException( "Parameter '{0}' of method '{1}' of type '{2}' does not have a Thrift interface definition."
                                             + Environment.NewLine
                                             + "It must be decorated with the ThriftParameterAttribute attribute.",
                                               info.Name, info.Member.Name, info.Member.DeclaringType );
        }

        internal static ThriftParsingException NotAnException( TypeInfo typeInfo, MethodInfo methodInfo )
        {
            return new ThriftParsingException( "Type '{0}' was used in a ThriftThrowsClauseAttribute for method '{1}' but does not inherit from Exception."
                                             + Environment.NewLine
                                             + "If this is unintentional, mark it with the ThriftStructAttribute.",
                                               typeInfo.FullName, methodInfo.Name );
        }

        internal static ThriftParsingException NotAService( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The type '{0}' is not an interface.", typeInfo.FullName );
        }

        internal static ThriftParsingException ServiceWithoutAttribute( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The interface '{0}' does not have a Thrift interface definition."
                                             + Environment.NewLine
                                             + "If this is unintentional, mark it with the ThriftServiceAttribute attribute.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException NoMethods( TypeInfo typeInfo )
        {
            return new ThriftParsingException( "The interface '{0}' does not have any methods, and is therefore useless."
                                             + Environment.NewLine
                                             + "If this is unintentional, add some methods to it.",
                                               typeInfo.FullName );
        }

        internal static ThriftParsingException MethodWithoutAttribute( MethodInfo methodInfo )
        {
            return new ThriftParsingException( "The method '{0}' is not part of the Thrift interface."
                                             + Environment.NewLine
                                             + "All methods in a Thrift service definition must be part of the Thrift interface."
                                             + Environment.NewLine
                                             + "If this is unintentional, mark it with the ThriftMethodAttribute attribute.",
                                               methodInfo.Name );
        }

        internal static ThriftParsingException MoreThanOneCancellationToken( MethodInfo methodInfo )
        {
            return new ThriftParsingException( "The method '{0}' has more than one CancellationToken parameter."
                                             + Environment.NewLine
                                             + "A Thrift method can only have at most one such parameter.",
                                               methodInfo.Name );
        }

        internal static ThriftParsingException SynchronousMethod( MethodInfo methodInfo )
        {
            return new ThriftParsingException( "The method '{0}' but does not return a Task or Task<T>."
                                             + Environment.NewLine
                                             + "Only asynchronous calls are supported. Please wrap the return type in a Task.",
                                               methodInfo.Name );
        }

        internal static ThriftParsingException OneWayMethodWithResult( MethodInfo methodInfo )
        {
            return new ThriftParsingException( "The method '{0}' is a one-way method, but returns a value."
                                             + Environment.NewLine
                                             + "One-way methods cannot return a value. Please either make it two-way, or remove the return value (make it a Task).",
                                               methodInfo.Name );
        }

        internal static ThriftParsingException OneWayMethodWithExceptions( MethodInfo methodInfo )
        {
            return new ThriftParsingException( "The method '{0}' is a one-way method, but has declared thrown exceptions."
                                             + Environment.NewLine
                                             + "One-way methods cannot throw exceptions since they do not wait for a server response. Please either make it two-way, or remove the declared exceptions.",
                                               methodInfo.Name );
        }
    }

    /// <summary>
    /// Raised when a serialization error occurs.
    /// </summary>
    public sealed class ThriftSerializationException : Exception
    {
        /// <summary>
        /// Prevents a default instance of the ThriftSerializationException class from being created.
        /// This is meant to force customers to use one of the static factory methods.
        /// </summary>
        private ThriftSerializationException( string message, params object[] args ) : base( string.Format( message, args ) ) { }

        internal static ThriftSerializationException RequiredFieldIsNull( string fieldName )
        {
            return new ThriftSerializationException( "Field '{0}' is a required field and cannot be null when writing it.", fieldName );
        }

        internal static ThriftSerializationException NullParameter( string parameterName )
        {
            return new ThriftSerializationException( "Parameter '{0}' was null.", parameterName );
        }

        internal static ThriftSerializationException MissingRequiredField( string fieldName )
        {
            return new ThriftSerializationException( "Field '{0}' is a required field, but was not present.", fieldName );
        }

        internal static ThriftSerializationException TypeIdMismatch( ThriftTypeId expectedId, ThriftTypeId actualId )
        {
            return new ThriftSerializationException( "Deserialization error: Expected type {0}, but type {1} was read.", expectedId, actualId );
        }
    }
}