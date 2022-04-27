// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
//
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Adapted from floating point vectors to three dimensional intergers.
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Stride.Core.Mathematics
{
    /// <summary>
    /// Represents an axis-aligned bounding box in three dimensional space.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct BoundingBoxInt3 : IEquatable<BoundingBoxInt3>, IFormattable
    {
        /// <summary>
        /// A <see cref="BoundingBoxInt3"/> which represents an empty space.
        /// </summary>
        public static readonly BoundingBoxInt3 Empty = new BoundingBoxInt3(new Int3(int.MaxValue), new Int3(int.MinValue));

        /// <summary>
        /// The minimum point of the box.
        /// </summary>
        public Int3 Minimum;

        /// <summary>
        /// The maximum point of the box.
        /// </summary>
        public Int3 Maximum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> struct.
        /// </summary>
        /// <param name="minimum">The minimum vertex of the bounding box.</param>
        /// <param name="maximum">The maximum vertex of the bounding box.</param>
        public BoundingBoxInt3(Int3 minimum, Int3 maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        /// <summary>
        /// Gets the center approximate center of this bounding box.
        /// </summary>
        public Int3 Center
        {
            get { return (Minimum + Maximum) / 2; }
        }

        /// <summary>
        /// Gets the approximate extent of this bounding box.
        /// </summary>
        public Int3 Extent
        {
            get { return (Maximum - Minimum) / 2; }
        }

        /// <summary>
        /// Retrieves the eight corners of the bounding box.
        /// </summary>
        /// <returns>An array of points representing the eight corners of the bounding box.</returns>
        public Int3[] GetCorners()
        {
            Int3[] results = new Int3[8];
            results[0] = new Int3(Minimum.X, Maximum.Y, Maximum.Z);
            results[1] = new Int3(Maximum.X, Maximum.Y, Maximum.Z);
            results[2] = new Int3(Maximum.X, Minimum.Y, Maximum.Z);
            results[3] = new Int3(Minimum.X, Minimum.Y, Maximum.Z);
            results[4] = new Int3(Minimum.X, Maximum.Y, Minimum.Z);
            results[5] = new Int3(Maximum.X, Maximum.Y, Minimum.Z);
            results[6] = new Int3(Maximum.X, Minimum.Y, Minimum.Z);
            results[7] = new Int3(Minimum.X, Minimum.Y, Minimum.Z);
            return results;
        }

        /* This implentation is wrong
        /// <summary>
        /// Determines if there is an intersection between the current object and a triangle.
        /// </summary>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
        {
            return Collision.BoxIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
        }
        */

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/>.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref BoundingBoxInt3 box)
        {
            return CollisionHelper.BoxIntersectsBox(ref this, ref box);
        }


        /// <summary>
        /// Determines whether the current objects contains a point.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public ContainmentType Contains(ref Int3 point)
        {
            return CollisionHelper.BoxContainsPoint(ref this, ref point);
        }

        /* This implentation is wrong
        /// <summary>
        /// Determines whether the current objects contains a triangle.
        /// </summary>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public ContainmentType Contains(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
        {
            return Collision.BoxContainsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
        }
        */

        /// <summary>
        /// Determines whether the current objects contains a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/>.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public ContainmentType Contains(ref BoundingBoxInt3 box)
        {
            return CollisionHelper.BoxContainsBox(ref this, ref box);
        }

        /// <summary>
        /// Constructs a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> that fully contains the given points.
        /// </summary>
        /// <param name="points">The points that will be contained by the box.</param>
        /// <param name="result">When the method completes, contains the newly constructed bounding box.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="points"/> is <c>null</c>.</exception>
        public static void FromPoints(Int3[] points, out BoundingBoxInt3 result)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            Int3 min = new Int3(int.MaxValue);
            Int3 max = new Int3(int.MinValue);

            for (int i = 0; i < points.Length; ++i)
            {
                Int3.Min(ref min, ref points[i], out min);
                Int3.Max(ref max, ref points[i], out max);
            }

            result = new BoundingBoxInt3(min, max);
        }

        /// <summary>
        /// Constructs a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> that fully contains the given points.
        /// </summary>
        /// <param name="points">The points that will be contained by the box.</param>
        /// <returns>The newly constructed bounding box.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="points"/> is <c>null</c>.</exception>
        public static BoundingBoxInt3 FromPoints(Int3[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            Int3 min = new Int3(int.MaxValue);
            Int3 max = new Int3(int.MinValue);

            for (int i = 0; i < points.Length; ++i)
            {
                Int3.Min(ref min, ref points[i], out min);
                Int3.Max(ref max, ref points[i], out max);
            }

            return new BoundingBoxInt3(min, max);
        }

        /// <summary>
        /// Constructs a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> that is as large enough to contains the bounding box and the given point.
        /// </summary>
        /// <param name="value1">The box to merge.</param>
        /// <param name="value2">The point to merge.</param>
        /// <param name="result">When the method completes, contains the newly constructed bounding box.</param>
        public static void Merge(ref BoundingBoxInt3 value1, ref Int3 value2, out BoundingBoxInt3 result)
        {
            Int3.Min(ref value1.Minimum, ref value2, out result.Minimum);
            Int3.Max(ref value1.Maximum, ref value2, out result.Maximum);
        }
        
        /// <summary>
        /// Constructs a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> that is as large as the total combined area of the two specified boxes.
        /// </summary>
        /// <param name="value1">The first box to merge.</param>
        /// <param name="value2">The second box to merge.</param>
        /// <param name="result">When the method completes, contains the newly constructed bounding box.</param>
        public static void Merge(ref BoundingBoxInt3 value1, ref BoundingBoxInt3 value2, out BoundingBoxInt3 result)
        {
            Int3.Min(ref value1.Minimum, ref value2.Minimum, out result.Minimum);
            Int3.Max(ref value1.Maximum, ref value2.Maximum, out result.Maximum);
        }

        /// <summary>
        /// Constructs a <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> that is as large as the total combined area of the two specified boxes.
        /// </summary>
        /// <param name="value1">The first box to merge.</param>
        /// <param name="value2">The second box to merge.</param>
        /// <returns>The newly constructed bounding box.</returns>
        public static BoundingBoxInt3 Merge(BoundingBoxInt3 value1, BoundingBoxInt3 value2)
        {
            BoundingBoxInt3 box;
            Int3.Min(ref value1.Minimum, ref value2.Minimum, out box.Minimum);
            Int3.Max(ref value1.Maximum, ref value2.Maximum, out box.Maximum);
            return box;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(BoundingBoxInt3 left, BoundingBoxInt3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(BoundingBoxInt3 left, BoundingBoxInt3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Minimum:{0} Maximum:{1}", Minimum.ToString(), Maximum.ToString());
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "Minimum:{0} Maximum:{1}", Minimum.ToString(format, CultureInfo.CurrentCulture),
                Maximum.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "Minimum:{0} Maximum:{1}", Minimum.ToString(), Maximum.ToString());
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                return ToString(formatProvider);

            return string.Format(formatProvider, "Minimum:{0} Maximum:{1}", Minimum.ToString(format, formatProvider),
                Maximum.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Minimum.GetHashCode() + Maximum.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Stride.Core.Mathematics.BoundingBoxInt3"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(BoundingBoxInt3 value)
        {
            return Minimum == value.Minimum && Maximum == value.Maximum;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            if (value.GetType() != GetType())
                return false;

            return Equals((BoundingBoxInt3)value);
        }
    }
}
