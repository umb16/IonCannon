﻿using System;
using System.Linq;
using System.Collections.Generic;
using Reflex.Injectors;
using Reflex.Scripts.Utilities;
using UnityEngine;

namespace Reflex
{
    public class Container : IDisposable
    {
        internal readonly CompositeDisposable Disposables = new CompositeDisposable();
        internal readonly Dictionary<Type, Binding> Bindings = new Dictionary<Type, Binding>(); // TContract, Binding
        internal readonly Dictionary<Type, object> Singletons = new Dictionary<Type, object>(); // TContract, Instance

        private Resolver _singletonNonLazyResolver;
        private readonly Resolver _methodResolver = new MethodResolver();
        private readonly Resolver _transientResolver = new TransientResolver();
        private readonly Resolver _singletonLazyResolver = new SingletonLazyResolver();

        public Container()
        {
            BindSingleton<Container>(this);
        }

        public T Construct<T>()
        {
            return ConstructorInjector.ConstructAndInject<T>(this);
        }
        
        public object Construct(Type concrete)
        {
            return ConstructorInjector.ConstructAndInject(concrete, this);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }

        public void Clear()
        {
            Bindings.Clear();
            Singletons.Clear();
        }
        
        public BindingContractDefinition<TContract> Bind<TContract>()
        {
            return new BindingContractDefinition<TContract>(this);
        }

        public void BindSingleton<TContract>(TContract instance)
        {
            var binding = new Binding
            {
                Contract = typeof(TContract),
                Concrete = instance.GetType(),
                Scope = BindingScope.SingletonLazy
            };
            
            Bindings.Add(typeof(TContract), binding);
            Singletons.Add(typeof(TContract), instance);
        }

        public void BindSingleton<T>(Type contract, T instance)
        {
            var binding = new Binding
            {
                Contract = contract,
                Concrete = instance.GetType(),
                Scope = BindingScope.SingletonLazy
            };
            
            Bindings.Add(contract, binding);
            Singletons.Add(contract, instance);
        }

        public BindingGenericContractDefinition BindGenericContract(Type genericContract)
        {
            return new BindingGenericContractDefinition(genericContract, this);
        }

        public TContract Resolve<TContract>()
        {
            if (typeof(TContract).Name == "ICollectableRegistry")
            {
                Debug.Log("B");
            }
            
            return (TContract) Resolve(typeof(TContract));
        }

        public object Resolve(Type contract)
        {
            var resolver = MatchResolver(contract);
            return resolver.Resolve(contract, this);
        }

        private Resolver MatchResolver(Type contract)
        {
            if (Bindings.TryGetValue(contract, out var binding))
            {
                switch (binding.Scope)
                {
                    case BindingScope.Method: return _methodResolver;
                    case BindingScope.Transient: return _transientResolver;
                    case BindingScope.SingletonLazy: return _singletonLazyResolver;
                    case BindingScope.SingletonNonLazy: return _singletonNonLazyResolver;
                    default: throw new ScopeNotHandledException(binding.Scope);
                }
            }

            throw new UnknownContractException(contract);
        }

        public TCast ResolveGenericContract<TCast>(Type genericContract, params Type[] genericConcrete)
        {
            var contract = genericContract.MakeGenericType(genericConcrete);
            return (TCast) Resolve(contract);
        }

        internal Type GetConcreteTypeFor(Type contract)
        {
            return Bindings[contract].Concrete;
        }

        internal object RegisterSingletonInstance(Type contract, object concrete)
        {
            Singletons.Add(contract, concrete);
            return concrete;
        }

        internal bool TryGetSingletonInstance(Type contract, out object instance)
        {
            return Singletons.TryGetValue(contract, out instance);
        }

        internal bool TryGetMethod(Type contract, out Func<object> method)
        {
            if (Bindings.TryGetValue(contract, out var binding))
            {
                if (binding.Scope == BindingScope.Method)
                {
                    method = binding.Method;
                    return true;
                }
            }

            method = null;
            return false;
        }
        
        internal void InstantiateNonLazySingletons()
        {
            _singletonNonLazyResolver = new SingletonLazyResolver();

            var nonLazyBindings = Bindings.Values.Where(IsSingletonNonLazy).ToArray();
            foreach (var binding in nonLazyBindings)
            {
                Resolve(binding.Contract);
            }
            _singletonNonLazyResolver = new SingletonNonLazyResolver();
        }

        private static bool IsSingletonNonLazy(Binding binding)
        {
            return binding.Scope == BindingScope.SingletonNonLazy;
        }
    }
}