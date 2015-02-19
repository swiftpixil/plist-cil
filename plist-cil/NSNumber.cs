﻿// plist-cil - An open source library to parse and generate property lists for .NET
// Copyright (C) 2015 Natalia Portillo
//
// This code is based on:
// plist - An open source library to parse and generate property lists
// Copyright (C) 2014 Daniel Dreibrodt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;
using System.Text;

namespace Claunia.PropertyList
{
    /// <summary>
    /// A number whose value is either an integer, a real number or bool.
    /// </summary>
    /// @author Daniel Dreibrodt
    public class NSNumber : NSObject, IComparable
    {
        /// <summary>
        /// Indicates that the number's value is an integer.
        /// The number is stored as a .NET <see cref="long"/>.
        /// Its original value could have been char, short, int, long or even long long.
        /// </summary>
        public const int INTEGER = 0;

        /// <summary>
        /// Indicates that the number's value is a real number.
        /// The number is stored as a .NET <see cref="double"/>.
        /// Its original value could have been float or double.
        /// </summary>
        public const int REAL = 1;

        /// <summary>
        /// Indicates that the number's value is bool.
        /// </summary>
        public const int BOOLEAN = 2;

        //Holds the current type of this number
        int type;

        long longValue;
        double doubleValue;
        bool boolValue;

        /// <summary>
        /// Parses integers and real numbers from their binary representation.
        /// <i>Note: real numbers are not yet supported.</i>
        /// </summary>
        /// <param name="bytes">The binary representation</param>
        /// <param name="type">The type of number</param>
        /// <seealso cref="INTEGER"/>
        /// <seealso cref="REAL"/>
        public NSNumber(byte[] bytes, int type) {
            switch (type) {
                // TODO: Implement BinaryPropertyListParser class
                case INTEGER: {
                        //doubleValue = longValue = BinaryPropertyListParser.parseLong(bytes);
                        break;
                    }
                // TODO: Implement BinaryPropertyListParser class
                case REAL: {
                        //doubleValue = BinaryPropertyListParser.parseDouble(bytes);
                        //longValue = Math.Round(doubleValue);
                        break;
                    }
                default: {
                        throw new ArgumentException("Type argument is not valid.");
                    }
            }
            this.type = type;
        }

        /// <summary>
        /// Creates a number from its textual representation.
        /// </summary>
        /// <param name="text">The textual representation of the number.</param>
        /// <seealso cref="bool.Parse"/>
        /// <seealso cref="long.Parse"/>
        /// <seealso cref="double.Parse"/>
        public NSNumber(string text) {
            if (text == null)
                throw new ArgumentException("The given string is null and cannot be parsed as number.");
            try {
                long l = long.Parse(text);
                doubleValue = longValue = l;
                type = INTEGER;
            } catch (Exception ex) {
                try {
                    doubleValue = double.Parse(text);
                    longValue = (long)Math.Round(doubleValue);
                    type = REAL;
                } catch (Exception ex2) {
                    try {
                        boolValue = text.ToLower().Equals("true") || text.ToLower().Equals("yes");
                        if(!boolValue && !(text.ToLower().Equals("false") || text.ToLower().Equals("no"))) {
                            throw new Exception("not a bool");
                        }
                        type = BOOLEAN;
                        doubleValue = longValue = boolValue ? 1 : 0;
                    } catch (Exception ex3) {
                        throw new ArgumentException("The given string neither represents a double, an int nor a bool value.");
                    }
                }
            }
        }

        /// <summary>
        /// Creates an integer number.
        /// </summary>
        /// <param name="i">The integer value.</param>
        public NSNumber(int i) {
            doubleValue = longValue = i;
            type = INTEGER;
        }

        /// <summary>
        /// Creates an integer number.
        /// </summary>
        /// <param name="l">The long integer value.</param>
        public NSNumber(long l) {
            doubleValue = longValue = l;
            type = INTEGER;
        }

        /// <summary>
        /// Creates a real number.
        /// </summary>
        /// <param name="d">The real value.</param>
        public NSNumber(double d) {
            longValue = (long) (doubleValue = d);
            type = REAL;
        }

        /// <summary>
        /// Creates a bool number.
        /// </summary>
        /// <param name="b">The bool value.</param>
        public NSNumber(bool b) {
            boolValue = b;
            doubleValue = longValue = b ? 1 : 0;
            type = BOOLEAN;
        }

        /// <summary>
        /// Gets the type of this number's value.
        /// </summary>
        /// <returns>The type flag.</returns>
        /// <seealso cref="BOOLEAN"/>
        /// <seealso cref="INTEGER"/>
        /// <seealso cref="REAL"/>
        public int GetNSNumberType() {
            return type;
        }

        /// <summary>
        /// Checks whether the value of this NSNumber is a bool.
        /// </summary>
        /// <returns>Whether the number's value is a bool.</returns>
        public bool isBoolean() {
            return type == BOOLEAN;
        }

        /// <summary>
        /// Checks whether the value of this NSNumber is an integer.
        /// </summary>
        /// <returns>Whether the number's value is an integer.</returns>
        public bool isInteger() {
            return type == INTEGER;
        }

        /// <summary>
        /// Checks whether the value of this NSNumber is a real number.
        /// </summary>
        /// <returns>Whether the number's value is a real number.</returns>
        public bool isReal() {
            return type == REAL;
        }

        /// <summary>
        /// The number's bool value.
        /// </summary>
        /// <returns><code>true</code> if the value is true or non-zero, <code>false</code> otherwise.</returns>
        public bool ToBool() {
            if (type == BOOLEAN)
                return boolValue;
            else
                return longValue != 0;
        }

        /// <summary>
        /// The number's long value.
        /// </summary>
        /// <returns>The value of the number as long</returns>
        public long ToLong() {
            return longValue;
        }

        /// <summary>
        /// The number's int value.
        /// <i>Note: Even though the number's type might be INTEGER it can be larger than a Java int.
        /// Use intValue() only if you are certain that it contains a number from the int range.
        /// Otherwise the value might be innaccurate.</i>
        /// </summary>
        /// <returns>The value of the number as int.</returns>
        public int ToInt() {
            return (int) longValue;
        }

        /// <summary>
        /// The number's double value.
        /// </summary>
        /// <returns>The value of the number as double.</returns>
        public double ToDouble() {
            return doubleValue;
        }

        /// <summary>
        /// The number's float value.
        /// WARNING: Possible loss of precision if the value is outside the float range.
        /// </summary>
        /// <returns>The value of the number as float.</returns>
        public float floatValue() {
            return (float) doubleValue;
        }

        /// <summary>
        /// Checks whether the other object is a NSNumber of the same value.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether the objects are equal in terms of numeric value and type.</returns>
        public override bool Equals(Object obj) {
            if (!(obj is NSNumber)) return false;
            NSNumber n = (NSNumber) obj;
            return type == n.type && longValue == n.longValue && doubleValue == n.doubleValue && boolValue == n.boolValue;
        }

        public override int GetHashCode() {
            int hash = type;
            hash = 37 * hash + (int) (this.longValue ^ ((uint)this.longValue >> 32));
            hash = 37 * hash + (int) (BitConverter.DoubleToInt64Bits(this.doubleValue) ^ ((uint)(BitConverter.DoubleToInt64Bits(this.doubleValue) >> 32)));
            hash = 37 * hash + (ToBool() ? 1 : 0);
            return hash;
        }

        public override string ToString() {
            switch (type) {
                case INTEGER: {
                        return ToLong().ToString();
                    }
                case REAL: {
                        return ToDouble().ToString();
                    }
                case BOOLEAN: {
                        return ToBool().ToString();
                    }
                default: {
                        return base.ToString();
                    }
            }
        }

        internal override void ToXml(StringBuilder xml, int level) {
            Indent(xml, level);
            switch (type) {
                case INTEGER: {
                        xml.Append("<integer>");
                        xml.Append(ToLong());
                        xml.Append("</integer>");
                        break;
                    }
                case REAL: {
                        xml.Append("<real>");
                        xml.Append(ToDouble());
                        xml.Append("</real>");
                        break;
                    }
                case BOOLEAN: {
                        if (ToBool())
                            xml.Append("<true/>");
                        else
                            xml.Append("<false/>");
                        break;
                    }
            }
        }

        // TODO: Implement BinaryPropertyListWriter class
        /*
        @Override
        void toBinary(BinaryPropertyListWriter out) throws IOException {
            switch (type()) {
                case INTEGER: {
                        if (longValue() < 0) {
                            out.write(0x13);
                            out.writeBytes(longValue(), 8);
                        } else if (longValue() <= 0xff) {
                            out.write(0x10);
                            out.writeBytes(longValue(), 1);
                        } else if (longValue() <= 0xffff) {
                            out.write(0x11);
                            out.writeBytes(longValue(), 2);
                        } else if (longValue() <= 0xffffffffL) {
                            out.write(0x12);
                            out.writeBytes(longValue(), 4);
                        } else {
                            out.write(0x13);
                            out.writeBytes(longValue(), 8);
                        }
                        break;
                    }
                case REAL: {
                        out.write(0x23);
                        out.writeDouble(doubleValue());
                        break;
                    }
                case BOOLEAN: {
                        out.write(boolValue() ? 0x09 : 0x08);
                        break;
                    }
            }
        }*/

        internal override void ToASCII(StringBuilder ascii, int level) {
            Indent(ascii, level);
            if (type == BOOLEAN) {
                ascii.Append(boolValue ? "YES" : "NO");
            } else {
                ascii.Append(ToString());
            }
        }

        internal override void ToASCIIGnuStep(StringBuilder ascii, int level) {
            Indent(ascii, level);
            switch (type) {
                case INTEGER: {
                        ascii.Append("<*I");
                        ascii.Append(ToString());
                        ascii.Append(">");
                        break;
                    }
                case REAL: {
                        ascii.Append("<*R");
                        ascii.Append(ToString());
                        ascii.Append(">");
                        break;
                    }
                case BOOLEAN: {
                        if (boolValue) {
                            ascii.Append("<*BY>");
                        } else {
                            ascii.Append("<*BN>");
                        }
                        break;
                    }
            }
        }

        public int CompareTo(Object o) {
            double x = ToDouble();
            double y;
            if (o is NSNumber) {
                NSNumber num = (NSNumber) o;
                y = num.ToDouble();
                return (x < y) ? -1 : ((x == y) ? 0 : 1);
            } else if (IsNumber(o)) {
                y = GetDoubleFromObject(o);
                return (x < y) ? -1 : ((x == y) ? 0 : 1);
            } else {
                return -1;
            }
        }

        /// <summary>
        /// Determines if an object is a number.
        /// Substitutes Java's Number class comparison
        /// </summary>
        /// <returns><c>true</c> if it is a number.</returns>
        /// <param name="o">Object.</param>
        static bool IsNumber(Object o)
        {
            return o is sbyte
            || o is byte
            || o is short
            || o is ushort
            || o is int
            || o is uint
            || o is long
            || o is ulong
            || o is float
            || o is double
            || o is decimal;
        }

        static double GetDoubleFromObject(Object o)
        {
            if (o is sbyte)
                return (double)((sbyte)o);
            if (o is byte)
                return (double)((byte)o);
            if (o is short)
                return (double)((short)o);
            if (o is ushort)
                return (double)((ushort)o);
            if (o is int)
                return (double)((int)o);
            if (o is uint)
                return (double)((uint)o);
            if (o is long)
                return (double)((long)o);
            if (o is ulong)
                return (double)((ulong)o);
            if (o is float)
                return (double)((float)o);
            if (o is double)
                return (double)o;
            if (o is decimal)
                return (double)((decimal)o);

            return (double)0;
        }
    }
}

