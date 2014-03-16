using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.DetectFeatures.Util.AspectRatio {

    public class AspectRatioComparer : IComparer, IComparer<AspectRatioInfo> {

        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero <paramref name="x"/> is less than <paramref name="y"/>. Zero <paramref name="x"/> equals <paramref name="y"/>. Greater than zero <paramref name="x"/> is greater than <paramref name="y"/>. </returns>
        /// <param name="x">The first object to compare. </param>
        /// <param name="y">The second object to compare. </param>
        /// <exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception>
        public int Compare(object x, object y) {
            if (x is AspectRatioInfo && y is float) {
                return ((AspectRatioInfo) x).CompareTo((float) y);
            }

            if(y is AspectRatioInfo && x is float) {
                return ((AspectRatioInfo) y).CompareTo((float) x);
            }

            throw new ArgumentException("Can not be used for comparing types other that AspectRatio to float or float to AspectRatio.");
        }

        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public int Compare(AspectRatioInfo x, AspectRatioInfo y) {
            return x.CompareTo(y);
        }
    }

}