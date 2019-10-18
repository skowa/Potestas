﻿using System;
using System.Reflection;

namespace Potestas
{
    /* TASK. Implement method Load to load factories interfaces from assembly provided.
     * 1. Consider some classes could be private.
     * 2. Consider using special attribute to exclude some factories from creation.
     * 3. Consider refactoring of factory interfaces.
     * 4. Consider making an extension for Assembly class.
     */
    internal class FactoriesLoader<T> where T : IEnergyObservation
    {
        public (ISourceFactory<T>[], IProcessingFactory<T>[]) Load(Assembly assembly)
        {
            throw new NotImplementedException();
        }
    }
}
