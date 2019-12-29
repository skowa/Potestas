using System;
using System.Collections.Generic;
using System.Linq;
using Potestas.Utils;

namespace Potestas.Storages
{
    public abstract class BaseStorage<T> where T : IEnergyObservation
    {
        public bool Contains(T item, IEnumerable<T> collection)
        {
            return !Validator.IsGenericTypeNull(item) && collection.Any(xmlElement => xmlElement.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex, IEnumerable<T> collection)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentException($"{nameof(arrayIndex)} is out of range");
            }

            var arrayHelper = new T[array.Length];
            int indexHelper = arrayIndex;
            foreach (var entity in collection)
            {
                if (indexHelper == array.Length)
                {
                    throw new ArgumentException($"The number of elements in the {nameof(array)} is greater than the available space from {nameof(arrayIndex)} to the end.");
                }

                arrayHelper[indexHelper] = entity;
                indexHelper++;
            }

            Array.Copy(arrayHelper, arrayIndex, array, arrayIndex, indexHelper - arrayIndex - 1);
        }

    }
}