﻿// Copyright (c) 2014-15 Solal Pirelli
// This code is licensed under the MIT License (see Licence.txt for details).

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ThriftSharp.Protocols;
using ThriftSharp.Utilities;

namespace ThriftSharp.Internals
{
    /// <summary>
    /// Writes Thrift messages from a client to a server.
    /// </summary>
    internal static class ThriftClientMessageWriter
    {
        private static readonly Dictionary<ThriftMethod, Action<object[], IThriftProtocol>> _knownWriters =
            new Dictionary<ThriftMethod, Action<object[], IThriftProtocol>>();

        /// <summary>
        /// Creates a compiled writer for the specified method.
        /// </summary>
        private static Action<object[], IThriftProtocol> CreateCompiledWriterForMethod( ThriftMethod method )
        {
            var argsParam = Expression.Parameter( typeof( object[] ) );
            var protocolParam = Expression.Parameter( typeof( IThriftProtocol ) );

            var methodContents = new List<Expression>
            {
                Expression.Call(
                    protocolParam,
                    Methods.IThriftProtocol_WriteMessageHeader,
                    Expression.New(
                        Constructors.ThriftMessageHeader,
                        Expression.Constant( method.Name ),
                        Expression.Constant( method.IsOneWay ? ThriftMessageType.OneWay : ThriftMessageType.Call )
                    )
                ),

                Expression.Call(
                    protocolParam,
                    Methods.IThriftProtocol_WriteStructHeader,
                    Expression.New(
                        Constructors.ThriftStructHeader,
                        Expression.Constant( "" )
                    )
                )
            };

            for ( int n = 0; n < method.Parameters.Count; n++ )
            {
                var getParamExpr = Expression.Convert(
                    Expression.ArrayAccess( argsParam, Expression.Constant( n ) ),
                    method.Parameters[n].WireType
                );

                methodContents.Add( ThriftStructWriter.CreateWriterForField( protocolParam, method.Parameters[n], getParamExpr ) );
            }

            methodContents.Add( Expression.Call( protocolParam, Methods.IThriftProtocol_WriteFieldStop ) );
            methodContents.Add( Expression.Call( protocolParam, Methods.IThriftProtocol_WriteStructEnd ) );
            methodContents.Add( Expression.Call( protocolParam, Methods.IThriftProtocol_WriteMessageEnd ) );

            return Expression.Lambda<Action<object[], IThriftProtocol>>(
                Expression.Block( methodContents ),
                argsParam, protocolParam
            ).Compile();
        }

        /// <summary>
        /// Writes the specified ThriftMethod call on the specified protocol.
        /// </summary>
        public static void Write( ThriftMethod method, object[] args, IThriftProtocol protocol )
        {
            if ( !_knownWriters.ContainsKey( method ) )
            {
                _knownWriters.Add( method, CreateCompiledWriterForMethod( method ) );
            }

            _knownWriters[method]( args, protocol );
        }
    }
}