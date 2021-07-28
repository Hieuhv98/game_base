/* Original license and copyright from file copied from https://github.com/871041532/zstring #ddf3d8
 * Copyright (c) sebas. All rights reserved.
 * Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
*/

/*
 Introduction:
    C# 0GC string supplement program. Combining the characteristics of gstring and CString (a tribute to the authors of these two programs), there is only one file, and the performance and ease of use are higher than the two.

 Report address:
    https://coh5.cn/p/1ace6338.html

 How to use:
    1. The Unity engine can use the zstring.cs file in the plugins directory (not in the plugins directory, IOS packaging or IL2CPP packaging and other FULLAOT methods can not be compiled), or directly put the structure definition into the zstring class; other C# programs Put zstring.cs directly into the project and use it.

    2. (Best performance) When update refreshes the label display every frame, or a lot of UI floating characters, or the string is used for a short time, use the following method:
        using (zstring.Block())
        {
            uiText1.text=(zstring)"hello world"+" you";
            uiText2.text=zstring.format("{0},{1}","hello","world");
        }
        The string value set in this way is in the shallow copy cache and may change for a certain period of time. The correctness is not guaranteed after going out of scope.

     3. The resource path, which needs to be resident, needs to be intern and used outside the scope

         using (zstring.Block())
        {
            zstring a="Assets/";
            zstring b=a+"prefabs/"+"/solider.prefab";
            prefabPath1=b.Intern();

            prefabPath2=zstring.format("{0},{1}","hello","world").Intern();
        }
        The string value set in this way is located in the deep copy cache and will not change during the game running and can be used outside the scope.

    4. You cannot use zstring as a member variable of a class. It is not recommended to write a for loop in the using scope, but use within a for loop.

    5. The class will be initialized at the first call, and various spaces will be allocated. It is recommended to call using(zstring.Block()){} once when the game is started.

    6.0GC. In terms of time consumption, short string processing, zstring takes 20%~30% less time than gstring, and is slower than native. For large string processing, zstring takes 70%~80% less time than gstring, which is close to the speed of native string.

    7. For the pursuit of extreme performance, the core function can use the memcpy memory copy function in C++Dll, and the performance is increased by 10% to 20%. Generally, this is not necessary.

    8. Test Open the zstringTest project, check the profile performance under bigStringTest with or without bigStringTest on the Test script. (At the same time compare zstring, gstring, CString, and string in the Kingdom Period)

    9. According to enthusiastic users, IL2CPP 2017.4 has byte alignment problems on Android, and it will be impossible to replace it with 2018. So there are three solutions at this time: 1. Replace IL2CPP with the version above 2018. The memcpy function on line 2.719 is replaced by a loop copying one byte at a time. 3. If you are not afraid of trouble, call the memory copy function dll of C language here, that is, memcpy in C language <string.h>, so the performance is higher.

 */

using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace Lance.Common
{
    internal struct Byte8192
    {
        private Byte4096 _a1;
        private Byte4096 _a2;
        public Byte8192(Byte4096 a1, Byte4096 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte4096
    {
        private Byte2048 _a1;
        private Byte2048 _a2;
        public Byte4096(Byte2048 a1, Byte2048 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte2048
    {
        private Byte1024 _a1;
        private Byte1024 _a2;
        public Byte2048(Byte1024 a1, Byte1024 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte1024
    {
        private Byte512 _a1;
        private Byte512 _a2;
        public Byte1024(Byte512 a1, Byte512 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte512
    {
        private Byte256 _a1;
        private Byte256 _a2;
        public Byte512(Byte256 a1, Byte256 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte256
    {
        private Byte128 _a1;
        private Byte128 _a2;
        public Byte256(Byte128 a1, Byte128 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte128
    {
        private Byte64 _a1;
        private Byte64 _a2;
        public Byte128(Byte64 a1, Byte64 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte64
    {
        private Byte32 _a1;
        private Byte32 _a2;
        public Byte64(Byte32 a1, Byte32 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte32
    {
        private Byte16 _a1;
        private Byte16 _a2;
        public Byte32(Byte16 a1, Byte16 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte16
    {
        private Byte8 _a1;
        private Byte8 _a2;
        public Byte16(Byte8 a1, Byte8 a2)
        {
            _a1 = a1;
            _a2 = a2;
        }
    }

    internal struct Byte8
    {
        private long _a1;
        public Byte8(long a1) { _a1 = a1; }
    }

    internal struct Byte4
    {
        private int _a1;
        public Byte4(int a1) { _a1 = a1; }
    }

    internal struct Byte2
    {
        private short _a;
        public Byte2(short a) { _a = a; }
    }

    internal struct Byte1
    {
        private byte _a;
        public Byte1(byte a) { _a = a; }
    }

    public class zstring
    {
        private static Queue<zstring>[] gCache; // idx specific string length, deep copy core cache
        private static Dictionary<int, Queue<zstring>> gSecCache; // key specific string length value string stack, deep copy secondary cache
        private static Stack<zstring> gShallowCache; // Shallow copy cache

        private static Stack<ZstringBlock> gBlocks; // zstring_block cache stack
        private static Stack<ZstringBlock> gOpenBlocks; // zstring has opened cache stack   
        private static Dictionary<int, string> gInternTable; // String intern table
        public static ZstringBlock gCurrentBlock; // The block where zstring is located
        private static readonly List<int> GFinds; // The string replace function records the substring position
        private static readonly zstring[] GFormatArgs; // Store formatted string values

        private const int INITIAL_BLOCK_CAPACITY = 32; // Number of gblock blocks
        private const int INITIAL_CACHE_CAPACITY = 128; // Cache dictionary capacity 128*4Byte more than 500 Byte
        private const int INITIAL_STACK_CAPACITY = 48; // The default nstring capacity of each stack of the cache dictionary
        private const int INITIAL_INTERN_CAPACITY = 256; // Intern capacity
        private const int INITIAL_OPEN_CAPACITY = 5; // The default number of open layers is 5
        private const int INITIAL_SHALLOW_CAPACITY = 100; // Default 100 shallow copies
        private const char NEW_ALLOC_CHAR = 'X'; // Fill char
        private bool _isShallow = false; // Shallow copy
        [NonSerialized] private string _value; // value
        [NonSerialized] private bool _disposed; // Destruction mark

        // Does not support construction
        private zstring() { throw new NotSupportedException(); }

        // Construct with default length
        private zstring(int length) { _value = new string(NEW_ALLOC_CHAR, length); }

        // Shallow copy special structure
        private zstring(string value, bool shallow)
        {
            if (!shallow)
            {
                throw new NotSupportedException();
            }

            _value = value;
            _isShallow = true;
        }

        static zstring()
        {
            Initialize(INITIAL_CACHE_CAPACITY,
                INITIAL_STACK_CAPACITY,
                INITIAL_BLOCK_CAPACITY,
                INITIAL_INTERN_CAPACITY,
                INITIAL_OPEN_CAPACITY,
                INITIAL_SHALLOW_CAPACITY);

            GFinds = new List<int>(10);
            GFormatArgs = new zstring[10];
        }

        // Dispose
        private void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(this);

            if (_isShallow) // Dark and shallow copies go to different caches
            {
                gShallowCache.Push(this);
            }
            else
            {
                Queue<zstring> stack;
                if (gCache.Length > Length)
                {
                    stack = gCache[Length]; // Take out the stack of valuelength length and push itself in
                }
                else
                {
                    stack = gSecCache[Length];
                }

                stack.Enqueue(this);
            }

            //memcpy(_value, NEW_ALLOC_CHAR);// Copy memory to value
            _disposed = true;
        }

        //Get the same content zstring from string, deep copy
        private static zstring GET(string value)
        {
            if (value == null)
                return null;
#if DBG
            if (log != null)
                log("Getting: " + value);
#endif
            var result = GET(value.Length);
            Memcpy(dst: result, src: value); // Memory copy
            return result;
        }

        // Shallow copy from string into zstring
        private static zstring GETShallow(string value)
        {
            if (gCurrentBlock == null)
            {
                throw new InvalidOperationException("The zstring operation must be in an zstring_block block.");
            }

            zstring result;
            if (gShallowCache.Count == 0)
            {
                result = new zstring(value, true);
            }
            else
            {
                result = gShallowCache.Pop();
                result._value = value;
            }

            result._disposed = false;
            gCurrentBlock.Push(result); // zstring pushes the stack where the block is located
            return result;
        }

        // Add string to the intern table
        private static string __intern(string value)
        {
            int hash = value.GetHashCode();
            if (gInternTable.ContainsKey(hash))
            {
                return gInternTable[hash];
            }
            else
            {
                string interned = new string(NEW_ALLOC_CHAR, value.Length);
                Memcpy(interned, value);
                gInternTable.Add(hash, interned);
                return interned;
            }
        }

        // Add method manually
        private static void GetStackInCache(int index, out Queue<zstring> outStack)
        {
            int length = gCache.Length;
            if (length > index) // Fetch from core cache
            {
                outStack = gCache[index];
            }
            else // Fetch from secondary cache
            {
                if (!gSecCache.TryGetValue(index, out outStack))
                {
                    outStack = new Queue<zstring>(INITIAL_STACK_CAPACITY);
                    gSecCache[index] = outStack;
                }
            }
        }

        // Get a specific length zstring
        private static zstring GET(int length)
        {
            if (gCurrentBlock == null || length <= 0) throw new InvalidOperationException("The zstring operation must be in a zstring_block block.");

            zstring result;
            Queue<zstring> stack;
            GetStackInCache(length, out stack);
            // Get Stack from cache
            if (stack.Count == 0)
            {
                result = new zstring(length);
            }
            else
            {
                result = stack.Dequeue();
            }

            result._disposed = false;
            gCurrentBlock.Push(result); // zstring pushes the stack where the block is located
            return result;
        }

        // value is a power of 10
        private static int get_digit_count(long value)
        {
            int cnt;
            for (cnt = 1; (value /= 10) > 0; cnt++) ;
            return cnt;
        }

        // value is a power of 10
        private static uint get_digit_count(uint value)
        {
            uint cnt;
            for (cnt = 1; (value /= 10) > 0; cnt++) ;
            return cnt;
        }

        // value is a power of 10
        private static int get_digit_count(int value)
        {
            int cnt;
            for (cnt = 1; (value /= 10) > 0; cnt++) ;
            return cnt;
        }

        // Get the subscript of char in the input from start
        private static int internal_index_of(string input, char value, int start) { return internal_index_of(input, value, start, input.Length - start); }

        // Get the subscript of string starting at 0 in input
        private static int internal_index_of(string input, string value) { return internal_index_of(input, value, 0, input.Length); }

        // Get the subscript of string starting from 0 in input
        private static int internal_index_of(string input, string value, int start) { return internal_index_of(input, value, start, input.Length - start); }

        // Get the format string
        private static unsafe zstring internal_format(string input, int numArgs)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // New string length
            int newLen = input.Length;
            for (int i = -3;;)
            {
                i = internal_index_of(input, '{', i + 3);
                if (i == -1)
                {
                    break;
                }

                newLen -= 3;
                int argIdx = input[i + 1] - '0';
                zstring arg = GFormatArgs[argIdx];
                newLen += arg.Length;
            }

            zstring result = GET(newLen);
            string resValue = result._value;

            int nextOutputIdx = 0;
            int nextInputIdx = 0;
            int braceIdx = -3;
            for (int i = 0, j = 0, x = 0;; x++) // x < num_args
            {
                braceIdx = internal_index_of(input, '{', braceIdx + 3);
                if (braceIdx == -1)
                {
                    break;
                }

                nextInputIdx = braceIdx;
                int argIdx = input[braceIdx + 1] - '0';
                string arg = GFormatArgs[argIdx]._value;
                if (braceIdx == -1) throw new InvalidOperationException("No braces { for argument" + arg);
                if (braceIdx + 2 >= input.Length || input[braceIdx + 2] != '}') throw new InvalidOperationException("No braces were found } for argument " + arg);

                fixed (char* ptrInput = input)
                {
                    fixed (char* ptrResult = resValue)
                    {
                        for (int k = 0; i < newLen;)
                        {
                            if (j < braceIdx)
                            {
                                ptrResult[i++] = ptrInput[j++];
                                ++nextOutputIdx;
                            }
                            else
                            {
                                ptrResult[i++] = arg[k++];
                                ++nextOutputIdx;
                                if (k == arg.Length)
                                {
                                    j += 3;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            nextInputIdx += 3;
            for (int i = nextOutputIdx, j = 0; i < newLen; i++, j++)
            {
                fixed (char* ptrInput = input)
                {
                    fixed (char* ptrResult = resValue)
                    {
                        ptrResult[i] = ptrInput[nextInputIdx + j];
                    }
                }
            }

            return result;
        }

        // Get the start subscript of char in the string
        private static unsafe int internal_index_of(string input, char value, int start, int count)
        {
            if (start < 0 || start >= input.Length)
                // throw new ArgumentOutOfRangeException("start");
                return -1;

            if (start + count > input.Length)
                return -1;
            // throw new ArgumentOutOfRangeException("count=" + count + " start+count=" + start + count);

            fixed (char* ptrThis = input)
            {
                int end = start + count;
                for (int i = start; i < end; i++)
                    if (ptrThis[i] == value)
                        return i;
                return -1;
            }
        }

        // Get the starting subscript of value in input from start
        private static unsafe int internal_index_of(string input, string value, int start, int count)
        {
            int inputLen = input.Length;

            if (start < 0 || start >= inputLen)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (count < 0 || start + count > inputLen)
                throw new ArgumentOutOfRangeException("count=" + count + " start+count=" + (start + count));

            if (count == 0)
                return -1;

            fixed (char* ptrInput = input)
            {
                fixed (char* ptrValue = value)
                {
                    int found = 0;
                    int end = start + count;
                    for (int i = start; i < end; i++)
                    {
                        for (int j = 0; j < value.Length && i + j < inputLen; j++)
                        {
                            if (ptrInput[i + j] == ptrValue[j])
                            {
                                found++;
                                if (found == value.Length)
                                    return i;
                                continue;
                            }

                            if (found > 0)
                                break;
                        }
                    }

                    return -1;
                }
            }
        }

        // Remove the count length substring from start in string
        private static unsafe zstring internal_remove(string input, int start, int count)
        {
            if (start < 0 || start >= input.Length)
                throw new ArgumentOutOfRangeException("start=" + start + " Length=" + input.Length);

            if (count < 0 || start + count > input.Length)
                throw new ArgumentOutOfRangeException("count=" + count + " start+count=" + (start + count) + " Length=" + input.Length);

            if (count == 0)
                return input;

            zstring result = GET(input.Length - count);
            internal_remove(result, input, start, count);
            return result;
        }

        // Copy the count length substring from start in src into dst
        private static unsafe void internal_remove(string dst, string src, int start, int count)
        {
            fixed (char* srcPtr = src)
            {
                fixed (char* dstPtr = dst)
                {
                    for (int i = 0, j = 0; i < dst.Length; i++)
                    {
                        if (i >= start && i < start + count) // within removal range
                            continue;
                        dstPtr[j++] = srcPtr[i];
                    }
                }
            }
        }

        // String replace, original string, substring to be replaced, new substring to be replaced
        private static unsafe zstring internal_replace(string value, string oldValue, string newValue)
        {
            // "Hello, World. There World" | World->Jon =
            // "000000000000000000000" (len = orig - 2 * (world-jon) = orig - 4
            // "Hello, 00000000000000"
            // "Hello, Jon00000000000"
            // "Hello, Jon. There 000"
            // "Hello, Jon. There Jon"

            // "Hello, World. There World" | World->Alexander =
            // "000000000000000000000000000000000" (len = orig + 2 * (alexander-world) = orig + 8
            // "Hello, 00000000000000000000000000"
            // "Hello, Alexander00000000000000000"
            // "Hello, Alexander. There 000000000"
            // "Hello, Alexander. There Alexander"

            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));

            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

            int idx = internal_index_of(value, oldValue);
            if (idx == -1)
                return value;

            GFinds.Clear();
            GFinds.Add(idx);

            // Record all idx points that need to be replaced
            while (idx + oldValue.Length < value.Length)
            {
                idx = internal_index_of(value, oldValue, idx + oldValue.Length);
                if (idx == -1)
                    break;
                GFinds.Add(idx);
            }

            // calc the right new total length
            int newLen;
            int dif = oldValue.Length - newValue.Length;
            if (dif > 0)
                newLen = value.Length - (GFinds.Count * dif);
            else
                newLen = value.Length + (GFinds.Count * -dif);

            zstring result = GET(newLen);
            fixed (char* ptrThis = value)
            {
                fixed (char* ptrResult = result._value)
                {
                    for (int i = 0, x = 0, j = 0; i < newLen;)
                    {
                        if (x == GFinds.Count || GFinds[x] != j)
                        {
                            ptrResult[i++] = ptrThis[j++];
                        }
                        else
                        {
                            for (int n = 0; n < newValue.Length; n++)
                                ptrResult[i + n] = newValue[n];

                            x++;
                            i += newValue.Length;
                            j += oldValue.Length;
                        }
                    }
                }
            }

            return result;
        }

        // Insert a to_insertChar of count length from the start position into the string value
        private static unsafe zstring internal_insert(string value, char toInsert, int start, int count)
        {
            // "HelloWorld" (to_insert=x, start=5, count=3) -> "HelloxxxWorld"

            if (start < 0 || start >= value.Length)
                throw new ArgumentOutOfRangeException("start=" + start + " Length=" + value.Length);

            if (count < 0)
                throw new ArgumentOutOfRangeException("count=" + count);

            if (count == 0)
                return GET(value);

            int newLen = value.Length + count;
            zstring result = GET(newLen);
            fixed (char* ptrValue = value)
            {
                fixed (char* ptrResult = result._value)
                {
                    for (int i = 0, j = 0; i < newLen; i++)
                    {
                        if (i >= start && i < start + count)
                            ptrResult[i] = toInsert;
                        else
                            ptrResult[i] = ptrValue[j++];
                    }
                }
            }

            return result;
        }

        // Insert the to_insert string into the input string, the position is start
        private static unsafe zstring internal_insert(string input, string toInsert, int start)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (toInsert == null)
                throw new ArgumentNullException(nameof(toInsert));

            if (start < 0 || start >= input.Length)
                throw new ArgumentOutOfRangeException("start=" + start + " Length=" + input.Length);

            if (toInsert.Length == 0)
                return GET(input);

            int newLen = input.Length + toInsert.Length;
            zstring result = GET(newLen);
            internal_insert(result, input, toInsert, start);
            return result;
        }

        // String splicing
        private static unsafe zstring internal_concat(string s1, string s2)
        {
            int totalLength = s1.Length + s2.Length;
            zstring result = GET(totalLength);
            fixed (char* ptrResult = result._value)
            {
                fixed (char* ptrS1 = s1)
                {
                    fixed (char* ptrS2 = s2)
                    {
                        Memcpy(dst: ptrResult, src: ptrS1, length: s1.Length, srcOffset: 0);
                        Memcpy(dst: ptrResult, src: ptrS2, length: s2.Length, srcOffset: s1.Length);
                    }
                }
            }

            return result;
        }

        // Insert the to_insert string into the start position of src, and write the content to dst
        private static unsafe void internal_insert(string dst, string src, string toInsert, int start)
        {
            fixed (char* ptrSrc = src)
            {
                fixed (char* ptrDst = dst)
                {
                    fixed (char* ptrToInsert = toInsert)
                    {
                        for (int i = 0, j = 0, k = 0; i < dst.Length; i++)
                        {
                            if (i >= start && i < start + toInsert.Length)
                                ptrDst[i] = ptrToInsert[k++];
                            else
                                ptrDst[i] = ptrSrc[j++];
                        }
                    }
                }
            }
        }

        // Insert the number of length count into dst, the starting position is start, and the length of dst must be greater than start + count
        private static unsafe void Longcpy(char* dst, long value, int start, int count)
        {
            int end = start + count;
            for (int i = end - 1; i >= start; i--, value /= 10)
                *(dst + i) = (char) (value % 10 + 48);
        }

        // Insert the number of length count into dst, the starting position is start, and the length of dst must be greater than start + count
        private static unsafe void Intcpy(char* dst, int value, int start, int count)
        {
            int end = start + count;
            for (int i = end - 1; i >= start; i--, value /= 10)
                *(dst + i) = (char) (value % 10 + 48);
        }

        private static unsafe void _memcpy4(byte* dest, byte* src, int size)
        {
            /*while (size >= 32) {
                // using long is better than int and slower than double
                // FIXME: enable this only on correct alignment or on platforms
                // that can tolerate unaligned reads/writes of doubles
                ((double*)dest) [0] = ((double*)src) [0];
                ((double*)dest) [1] = ((double*)src) [1];
                ((double*)dest) [2] = ((double*)src) [2];
                ((double*)dest) [3] = ((double*)src) [3];
                dest += 32;
                src += 32;
                size -= 32;
            }*/
            while (size >= 16)
            {
                ((int*) dest)[0] = ((int*) src)[0];
                ((int*) dest)[1] = ((int*) src)[1];
                ((int*) dest)[2] = ((int*) src)[2];
                ((int*) dest)[3] = ((int*) src)[3];
                dest += 16;
                src += 16;
                size -= 16;
            }

            while (size >= 4)
            {
                ((int*) dest)[0] = ((int*) src)[0];
                dest += 4;
                src += 4;
                size -= 4;
            }

            while (size > 0)
            {
                ((byte*) dest)[0] = ((byte*) src)[0];
                dest += 1;
                src += 1;
                --size;
            }
        }

        private static unsafe void _memcpy2(byte* dest, byte* src, int size)
        {
            while (size >= 8)
            {
                ((short*) dest)[0] = ((short*) src)[0];
                ((short*) dest)[1] = ((short*) src)[1];
                ((short*) dest)[2] = ((short*) src)[2];
                ((short*) dest)[3] = ((short*) src)[3];
                dest += 8;
                src += 8;
                size -= 8;
            }

            while (size >= 2)
            {
                ((short*) dest)[0] = ((short*) src)[0];
                dest += 2;
                src += 2;
                size -= 2;
            }

            if (size > 0)
            {
                ((byte*) dest)[0] = ((byte*) src)[0];
            }
        }

        // Copy the count length string src from src, 0 to dst
        //private unsafe static void memcpy(char* dest, char* src, int count)
        //{
        //    // Same rules as for memcpy, but with the premise that 
        //    // chars can only be aligned to even addresses if their
        //    // enclosing types are correctly aligned

        //    superMemcpy(dest, src, count);
        //    //if ((((int)(byte*)dest | (int)(byte*)src) & 3) != 0)//Convert to byte pointer
        //    //{
        //    //    if (((int)(byte*)dest & 2) != 0 && ((int)(byte*)src & 2) != 0 && count > 0)
        //    //    {
        //    //        ((short*)dest)[0] = ((short*)src)[0];
        //    //        dest++;
        //    //        src++;
        //    //        count--;
        //    //    }
        //    //    if ((((int)(byte*)dest | (int)(byte*)src) & 2) != 0)
        //    //    {
        //    //        _memcpy2((byte*)dest, (byte*)src, count * 2);//Convert to short* pointer to copy two bytes at a time
        //    //        return;
        //    //    }
        //    //}
        //    //_memcpy4((byte*)dest, (byte*)src, count * 2);//Convert to int* pointer to copy four bytes at a time
        //}
        //--------------------------------------Hand knock memcpy-------------------------------------//
        private const int M_CHAR_LEN = sizeof(char);

        private static unsafe void Memcpy(char* dest, char* src, int count)
        {
            UnsafeUtility.MemCpy(dest, src, count * M_CHAR_LEN);
            //ByteCopy((byte*) dest, (byte*) src, count * M_CHAR_LEN);
        }

        private static unsafe void ByteCopy(byte* dest, byte* src, int byteCount)
        {
            if (byteCount < 128)
            {
                goto g64;
            }
            else if (byteCount < 2048)
            {
                goto g1024;
            }

            while (byteCount >= 8192)
            {
                ((Byte8192*) dest)[0] = ((Byte8192*) src)[0];
                dest += 8192;
                src += 8192;
                byteCount -= 8192;
            }

            if (byteCount >= 4096)
            {
                ((Byte4096*) dest)[0] = ((Byte4096*) src)[0];
                dest += 4096;
                src += 4096;
                byteCount -= 4096;
            }

            if (byteCount >= 2048)
            {
                ((Byte2048*) dest)[0] = ((Byte2048*) src)[0];
                dest += 2048;
                src += 2048;
                byteCount -= 2048;
            }

            g1024:
            if (byteCount >= 1024)
            {
                ((Byte1024*) dest)[0] = ((Byte1024*) src)[0];
                dest += 1024;
                src += 1024;
                byteCount -= 1024;
            }

            if (byteCount >= 512)
            {
                ((Byte512*) dest)[0] = ((Byte512*) src)[0];
                dest += 512;
                src += 512;
                byteCount -= 512;
            }

            if (byteCount >= 256)
            {
                ((Byte256*) dest)[0] = ((Byte256*) src)[0];
                dest += 256;
                src += 256;
                byteCount -= 256;
            }

            if (byteCount >= 128)
            {
                ((Byte128*) dest)[0] = ((Byte128*) src)[0];
                dest += 128;
                src += 128;
                byteCount -= 128;
            }

            g64:
            if (byteCount >= 64)
            {
                ((Byte64*) dest)[0] = ((Byte64*) src)[0];
                dest += 64;
                src += 64;
                byteCount -= 64;
            }

            if (byteCount >= 32)
            {
                ((Byte32*) dest)[0] = ((Byte32*) src)[0];
                dest += 32;
                src += 32;
                byteCount -= 32;
            }

            if (byteCount >= 16)
            {
                ((Byte16*) dest)[0] = ((Byte16*) src)[0];
                dest += 16;
                src += 16;
                byteCount -= 16;
            }

            if (byteCount >= 8)
            {
                ((Byte8*) dest)[0] = ((Byte8*) src)[0];
                dest += 8;
                src += 8;
                byteCount -= 8;
            }

            if (byteCount >= 4)
            {
                ((Byte4*) dest)[0] = ((Byte4*) src)[0];
                dest += 4;
                src += 4;
                byteCount -= 4;
            }

            if (byteCount >= 2)
            {
                ((Byte2*) dest)[0] = ((Byte2*) src)[0];
                dest += 2;
                src += 2;
                byteCount -= 2;
            }

            if (byteCount >= 1)
            {
                ((Byte1*) dest)[0] = ((Byte1*) src)[0];
                dest += 1;
                src += 1;
                byteCount -= 1;
            }
        }
        //-----------------------------------------------------------------------------------------//

        // Fill the string dst with characters src
        private static unsafe void Memcpy(string dst, char src)
        {
            fixed (char* ptrDst = dst)
            {
                int len = dst.Length;
                for (int i = 0; i < len; i++)
                    ptrDst[i] = src;
            }
        }

        // Copy characters to the specified index position of dst
        private static unsafe void Memcpy(string dst, char src, int index)
        {
            fixed (char* ptr = dst)
                ptr[index] = src;
        }

        // Copy the src content of the same length into dst
        private static unsafe void Memcpy(string dst, string src)
        {
            if (dst.Length != src.Length)
                throw new InvalidOperationException("The length of the two string parameters is inconsistent.");
            fixed (char* dstPtr = dst)
            {
                fixed (char* srcPtr = src)
                {
                    Memcpy(dstPtr, srcPtr, dst.Length);
                }
            }
        }

        // Copy the specified length of src into dst, and offset the dst subscript src_offset
        private static unsafe void Memcpy(char* dst, char* src, int length, int srcOffset) { Memcpy(dst + srcOffset, src, length); }

        private static unsafe void Memcpy(string dst, string src, int length, int srcOffset)
        {
            fixed (char* ptrDst = dst)
            {
                fixed (char* ptrSrc = src)
                {
                    Memcpy(ptrDst + srcOffset, ptrSrc, length);
                }
            }
        }

        public class ZstringBlock : IDisposable
        {
            private readonly Stack<zstring> _stack;

            internal ZstringBlock(int capacity) { _stack = new Stack<zstring>(capacity); }

            internal void Push(zstring str) { _stack.Push(str); }

            internal IDisposable Begin() // Constructor
            {
#if DBG
                if (log != null)
                    log("Began block");
#endif
                return this;
            }

            void IDisposable.Dispose() // Destructor
            {
#if DBG
                if (log != null)
                    log("Disposing block");
#endif
                while (_stack.Count > 0)
                {
                    var str = _stack.Pop();
                    str.Dispose(); // The Dispose method of zstring in the loop call stack
                }

                zstring.gBlocks.Push(this); // Push itself into the cache stack

                // Assign currentBlock
                gOpenBlocks.Pop();
                if (gOpenBlocks.Count > 0)
                {
                    zstring.gCurrentBlock = gOpenBlocks.Peek();
                }
                else
                {
                    zstring.gCurrentBlock = null;
                }
            }
        }

        // Public API

        #region

        public static Action<string> log = null;

        public static uint decimalAccuracy = 3; // Digits of precision after the decimal point

        // Get string length
        public int Length { get { return _value.Length; } }

        // Class structure: cache_capacity cache stack dictionary capacity, stack_capacity cache string stack capacity,
        // block_capacity cache stack capacity,
        // intern_capacity cache,
        // open_capacity default open layer number
        public static void Initialize(int cacheCapacity, int stackCapacity, int blockCapacity, int internCapacity, int openCapacity, int shallowCacheCapacity)
        {
            gCache = new Queue<zstring>[cacheCapacity];
            gSecCache = new Dictionary<int, Queue<zstring>>(cacheCapacity);
            gBlocks = new Stack<ZstringBlock>(blockCapacity);
            gInternTable = new Dictionary<int, string>(internCapacity);
            gOpenBlocks = new Stack<ZstringBlock>(openCapacity);
            gShallowCache = new Stack<zstring>(shallowCacheCapacity);
            for (int c = 0; c < cacheCapacity; c++)
            {
                var stack = new Queue<zstring>(stackCapacity);
                for (int j = 0; j < stackCapacity; j++)
                    stack.Enqueue(new zstring(c));
                gCache[c] = stack;
            }

            for (int i = 0; i < blockCapacity; i++)
            {
                var block = new ZstringBlock(blockCapacity * 2);
                gBlocks.Push(block);
            }

            for (int i = 0; i < shallowCacheCapacity; i++)
            {
                gShallowCache.Push(new zstring(null, true));
            }
        }

        // Used by using syntax. Take a block from the zstring_block stack and set it as the current g_current_block.
        // The newly generated zstring in the code block {} will be pushed into the internal stack of the block.
        // When leaving the scope of the block, call the Dispose function of the block to fill all zstrings in the inner stack
        // with initial values and put them into the zstring cache stack.
        // At the same time put itself into the block cache stack. (There is a problem here: using Stack to cache the block,
        // when the block is put into the Stack by dispose, g_current_block still points to this block,
        // and the block before this block cannot be recorded, which makes zstring.Block() unable to be nested)
        public static IDisposable Block()
        {
            if (gBlocks.Count == 0)
                gCurrentBlock = new ZstringBlock(INITIAL_BLOCK_CAPACITY * 2);
            else
                gCurrentBlock = gBlocks.Pop();

            gOpenBlocks.Push(gCurrentBlock); // Add code to push this stuff into the open stack
            return gCurrentBlock.Begin();
        }

        // Put the zstring value into the intern cache table for external use
        public string Intern()
        {
            //string interned = new string(NEW_ALLOC_CHAR, _value.Length);
            //memcpy(interned, _value);
            //return interned;
            return __intern(_value);
        }

        // Put the string into the zstring intern cache table for external use
        public static string Intern(string value) { return __intern(value); }

        public static void Intern(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                __intern(values[i]);
        }

        // Subscript value function
        public char this[int i] { get { return _value[i]; } set { Memcpy(this, value, i); } }

        // Get hashcode
        public override int GetHashCode() { return _value.GetHashCode(); }

        // Literal comparison
        public override bool Equals(object obj)
        {
            if (obj == null)
                return ReferenceEquals(this, null);

            var gstr = obj as zstring;
            if (gstr != null)
                return gstr._value == this._value;

            if (obj is string str)
                return str == this._value;

            return false;
        }

        // Converted to string
        public override string ToString() { return _value; }

        // bool -> zstring conversion
        public static implicit operator zstring(bool value) { return GET(value ? "True" : "False"); }

        // long -> zstring conversion
        public static unsafe implicit operator zstring(long value)
        {
            // e.g. 125
            // first pass: count the number of digits
            // then: get a zstring with length = num digits
            // finally: iterate again, get the char of each digit, memcpy char to result
            bool negative = value < 0;
            value = Math.Abs(value);
            int numDigits = get_digit_count(value);
            zstring result;
            if (negative)
            {
                result = GET(numDigits + 1);
                fixed (char* ptr = result._value)
                {
                    *ptr = '-';
                    Longcpy(ptr, value, 1, numDigits);
                }
            }
            else
            {
                result = GET(numDigits);
                fixed (char* ptr = result._value)
                    Longcpy(ptr, value, 0, numDigits);
            }

            return result;
        }

        // int -> zstring conversion
        public static unsafe implicit operator zstring(int value)
        {
            // e.g. 125
            // first pass: count the number of digits
            // then: get a zstring with length = num digits
            // finally: iterate again, get the char of each digit, memcpy char to result
            bool negative = value < 0;
            value = Math.Abs(value);
            int numDigits = get_digit_count(value);
            zstring result;
            if (negative)
            {
                result = GET(numDigits + 1);
                fixed (char* ptr = result._value)
                {
                    *ptr = '-';
                    Intcpy(ptr, value, 1, numDigits);
                }
            }
            else
            {
                result = GET(numDigits);
                fixed (char* ptr = result._value)
                    Intcpy(ptr, value, 0, numDigits);
            }

            return result;
        }

        // float -> zstring conversion
        public static unsafe implicit operator zstring(float value)
        {
            // e.g. 3.148
            bool negative = value < 0;
            if (negative) value = -value;
            long mul = (long) Math.Pow(10, decimalAccuracy);
            long number = (long) (value * mul); // gets the number as a whole, e.g. 3148
            int leftNum = (int) (number / mul); // left part of the decimal point, e.g. 3
            int rightNum = (int) (number % mul); // right part of the decimal pnt, e.g. 148
            int leftDigitCount = get_digit_count(leftNum); // e.g. 1
            int rightDigitCount = get_digit_count(rightNum); // e.g. 3
            //int total = left_digit_count + right_digit_count + 1; // +1 for '.'
            int total = leftDigitCount + (int) decimalAccuracy + 1; // +1 for '.'

            zstring result;
            if (negative)
            {
                result = GET(total + 1); // +1 for '-'
                fixed (char* ptr = result._value)
                {
                    *ptr = '-';
                    Intcpy(ptr, leftNum, 1, leftDigitCount);
                    *(ptr + leftDigitCount + 1) = '.';
                    int offest = (int) decimalAccuracy - rightDigitCount;
                    for (int i = 0; i < offest; i++)
                        *(ptr + leftDigitCount + i + 1) = '0';
                    Intcpy(ptr, rightNum, leftDigitCount + 2 + offest, rightDigitCount);
                }
            }
            else
            {
                result = GET(total);
                fixed (char* ptr = result._value)
                {
                    Intcpy(ptr, leftNum, 0, leftDigitCount);
                    *(ptr + leftDigitCount) = '.';
                    int offest = (int) decimalAccuracy - rightDigitCount;
                    for (int i = 0; i < offest; i++)
                        *(ptr + leftDigitCount + i + 1) = '0';
                    Intcpy(ptr, rightNum, leftDigitCount + 1 + offest, rightDigitCount);
                }
            }

            return result;
        }

        // string -> zstring conversion
        public static implicit operator zstring(string value)
        {
            //return get(value);
            return GETShallow(value);
        }

        // string -> zstring conversion
        public static zstring Shallow(string value) { return GETShallow(value); }

        // zstring -> string conversion
        public static implicit operator string(zstring value) { return value._value; }

        // + overload
        public static zstring operator +(zstring left, zstring right) { return internal_concat(left, right); }

        // == overload
        public static bool operator ==(zstring left, zstring right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            if (ReferenceEquals(right, null))
                return false;
            return left._value == right._value;
        }

        // != overload
        public static bool operator !=(zstring left, zstring right) { return !(left._value == right._value); }

        // Convert to upper case
        public unsafe zstring ToUpper()
        {
            var result = GET(Length);
            fixed (char* ptrThis = this._value)
            {
                fixed (char* ptrResult = result._value)
                {
                    for (int i = 0; i < _value.Length; i++)
                    {
                        var ch = ptrThis[i];
                        if (char.IsLower(ch))
                            ptrResult[i] = char.ToUpper(ch);
                        else
                            ptrResult[i] = ptrThis[i];
                    }
                }
            }

            return result;
        }

        // Convert to lower case
        public unsafe zstring ToLower()
        {
            var result = GET(Length);
            fixed (char* ptrThis = this._value)
            {
                fixed (char* ptrResult = result._value)
                {
                    for (int i = 0; i < _value.Length; i++)
                    {
                        var ch = ptrThis[i];
                        if (char.IsUpper(ch))
                            ptrResult[i] = char.ToLower(ch);
                        else
                            ptrResult[i] = ptrThis[i];
                    }
                }
            }

            return result;
        }

        // Remove cut
        public zstring Remove(int start) { return Remove(start, Length - start); }

        // Remove cut
        public zstring Remove(int start, int count) { return internal_remove(this._value, start, count); }

        // Insert count length characters from start
        public zstring Insert(char value, int start, int count) { return internal_insert(this._value, value, start, count); }

        // Insert start string
        public zstring Insert(string value, int start) { return internal_insert(this._value, value, start); }

        // Sub character replacement
        public unsafe zstring Replace(char oldValue, char newValue)
        {
            zstring result = GET(Length);
            fixed (char* ptrThis = this._value)
            {
                fixed (char* ptrResult = result._value)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        ptrResult[i] = ptrThis[i] == oldValue ? newValue : ptrThis[i];
                    }
                }
            }

            return result;
        }

        // Substring replacement
        public zstring Replace(string oldValue, string newValue) { return internal_replace(this._value, oldValue, newValue); }

        // Cut the subsequent substring from the start position
        public zstring Substring(int start) { return Substring(start, Length - start); }

        // Cut the count length substring from start
        public unsafe zstring Substring(int start, int count)
        {
            if (start < 0 || start >= Length)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (count > Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            zstring result = GET(count);
            fixed (char* src = this._value)
            fixed (char* dst = result._value)
                Memcpy(dst, src + start, count);

            return result;
        }

        // Substring contains judgment
        public bool Contains(string value) { return IndexOf(value) != -1; }

        // Character contains judgment
        public bool Contains(char value) { return IndexOf(value) != -1; }

        // The first occurrence of the substring
        public int LastIndexOf(string value)
        {
            int idx = -1;
            int lastFind = -1;
            while (true)
            {
                idx = internal_index_of(this._value, value, idx + value.Length);
                lastFind = idx;
                if (idx == -1 || idx + value.Length >= this._value.Length)
                    break;
            }

            return lastFind;
        }

        // Character's first appearance position
        public int LastIndexOf(char value)
        {
            int idx = -1;
            int lastFind = -1;
            while (true)
            {
                idx = internal_index_of(this._value, value, idx + 1);
                lastFind = idx;
                if (idx == -1 || idx + 1 >= this._value.Length)
                    break;
            }

            return lastFind;
        }

        // Character's first appearance position
        public int IndexOf(char value) { return IndexOf(value, 0, Length); }

        // The character's first appearance position since start
        public int IndexOf(char value, int start) { return internal_index_of(this._value, value, start); }

        // Characters within count length from start,
        public int IndexOf(char value, int start, int count) { return internal_index_of(this._value, value, start, count); }

        // The first occurrence of the substring
        public int IndexOf(string value) { return IndexOf(value, 0, Length); }

        // The first occurrence of the substring from the start position
        public int IndexOf(string value, int start) { return IndexOf(value, start, Length - start); }

        // The first occurrence of the substring within the length of count from the start position
        public int IndexOf(string value, int start, int count) { return internal_index_of(this._value, value, start, count); }

        // Whether to end with a string
        public unsafe bool EndsWith(string postfix)
        {
            if (postfix == null)
                throw new ArgumentNullException(nameof(postfix));

            if (this.Length < postfix.Length)
                return false;

            fixed (char* ptrThis = this._value)
            {
                fixed (char* ptrPostfix = postfix)
                {
                    for (int i = this._value.Length - 1, j = postfix.Length - 1; j >= 0; i--, j--)
                        if (ptrThis[i] != ptrPostfix[j])
                            return false;
                }
            }

            return true;
        }

        // Whether to start with a string
        public unsafe bool StartsWith(string prefix)
        {
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            if (this.Length < prefix.Length)
                return false;

            fixed (char* ptrThis = this._value)
            {
                fixed (char* ptrPrefix = prefix)
                {
                    for (int i = 0; i < prefix.Length; i++)
                        if (ptrThis[i] != ptrPrefix[i])
                            return false;
                }
            }

            return true;
        }

        // Get the number of cached strings of a certain length
        public static int GetCacheCount(int length)
        {
            GetStackInCache(length, out var stack);
            return stack.Count;
        }

        // Self + value splicing
        public zstring Concat(zstring value) { return internal_concat(this, value); }

        // Cluster of static stitching methods
        public static zstring Concat(zstring s0, zstring s1) { return s0 + s1; }

        public static zstring Concat(zstring s0, zstring s1, zstring s2) { return s0 + s1 + s2; }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3) { return s0 + s1 + s2 + s3; }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3, zstring s4) { return s0 + s1 + s2 + s3 + s4; }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3, zstring s4, zstring s5) { return s0 + s1 + s2 + s3 + s4 + s5; }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3, zstring s4, zstring s5, zstring s6) { return s0 + s1 + s2 + s3 + s4 + s5 + s6; }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3, zstring s4, zstring s5, zstring s6, zstring s7)
        {
            return s0 + s1 + s2 + s3 + s4 + s5 + s6 + s7;
        }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3, zstring s4, zstring s5, zstring s6, zstring s7, zstring s8)
        {
            return s0 + s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8;
        }

        public static zstring Concat(zstring s0, zstring s1, zstring s2, zstring s3, zstring s4, zstring s5, zstring s6, zstring s7, zstring s8, zstring s9)
        {
            return s0 + s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9;
        }

        // Static formatting method cluster
        public static zstring Format(
            string input,
            zstring arg0,
            zstring arg1,
            zstring arg2,
            zstring arg3,
            zstring arg4,
            zstring arg5,
            zstring arg6,
            zstring arg7,
            zstring arg8,
            zstring arg9)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));
            if (arg4 == null) throw new ArgumentNullException(nameof(arg4));
            if (arg5 == null) throw new ArgumentNullException(nameof(arg5));
            if (arg6 == null) throw new ArgumentNullException(nameof(arg6));
            if (arg7 == null) throw new ArgumentNullException(nameof(arg7));
            if (arg8 == null) throw new ArgumentNullException(nameof(arg8));
            if (arg9 == null) throw new ArgumentNullException(nameof(arg9));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            GFormatArgs[4] = arg4;
            GFormatArgs[5] = arg5;
            GFormatArgs[6] = arg6;
            GFormatArgs[7] = arg7;
            GFormatArgs[8] = arg8;
            GFormatArgs[9] = arg9;
            return internal_format(input, 10);
        }

        public static zstring Format(
            string input,
            zstring arg0,
            zstring arg1,
            zstring arg2,
            zstring arg3,
            zstring arg4,
            zstring arg5,
            zstring arg6,
            zstring arg7,
            zstring arg8)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));
            if (arg4 == null) throw new ArgumentNullException(nameof(arg4));
            if (arg5 == null) throw new ArgumentNullException(nameof(arg5));
            if (arg6 == null) throw new ArgumentNullException(nameof(arg6));
            if (arg7 == null) throw new ArgumentNullException(nameof(arg7));
            if (arg8 == null) throw new ArgumentNullException(nameof(arg8));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            GFormatArgs[4] = arg4;
            GFormatArgs[5] = arg5;
            GFormatArgs[6] = arg6;
            GFormatArgs[7] = arg7;
            GFormatArgs[8] = arg8;
            return internal_format(input, 9);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1, zstring arg2, zstring arg3, zstring arg4, zstring arg5, zstring arg6, zstring arg7)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));
            if (arg4 == null) throw new ArgumentNullException(nameof(arg4));
            if (arg5 == null) throw new ArgumentNullException(nameof(arg5));
            if (arg6 == null) throw new ArgumentNullException(nameof(arg6));
            if (arg7 == null) throw new ArgumentNullException(nameof(arg7));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            GFormatArgs[4] = arg4;
            GFormatArgs[5] = arg5;
            GFormatArgs[6] = arg6;
            GFormatArgs[7] = arg7;
            return internal_format(input, 8);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1, zstring arg2, zstring arg3, zstring arg4, zstring arg5, zstring arg6)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));
            if (arg4 == null) throw new ArgumentNullException(nameof(arg4));
            if (arg5 == null) throw new ArgumentNullException(nameof(arg5));
            if (arg6 == null) throw new ArgumentNullException(nameof(arg6));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            GFormatArgs[4] = arg4;
            GFormatArgs[5] = arg5;
            GFormatArgs[6] = arg6;
            return internal_format(input, 7);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1, zstring arg2, zstring arg3, zstring arg4, zstring arg5)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));
            if (arg4 == null) throw new ArgumentNullException(nameof(arg4));
            if (arg5 == null) throw new ArgumentNullException(nameof(arg5));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            GFormatArgs[4] = arg4;
            GFormatArgs[5] = arg5;
            return internal_format(input, 6);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1, zstring arg2, zstring arg3, zstring arg4)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));
            if (arg4 == null) throw new ArgumentNullException(nameof(arg4));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            GFormatArgs[4] = arg4;
            return internal_format(input, 5);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1, zstring arg2, zstring arg3)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            if (arg3 == null) throw new ArgumentNullException(nameof(arg3));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            GFormatArgs[3] = arg3;
            return internal_format(input, 4);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1, zstring arg2)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            GFormatArgs[2] = arg2;
            return internal_format(input, 3);
        }

        public static zstring Format(string input, zstring arg0, zstring arg1)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            GFormatArgs[0] = arg0;
            GFormatArgs[1] = arg1;
            return internal_format(input, 2);
        }

        public static zstring Format(string input, zstring arg0)
        {
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));

            GFormatArgs[0] = arg0;
            return internal_format(input, 1);
        }

        // Ordinary float->string is an implicit conversion. Only three significant digits are retained after the decimal point.
        // For higher precision requirements, implicit conversion can modify the static variable DecimalAccuracy.
        // Use this method for explicit conversion. The function ends DecimalAccuracy value and the previous The same
        public static zstring FloatToZstring(float value, uint decimalAccuracy)
        {
            uint oldValue = zstring.decimalAccuracy;
            zstring.decimalAccuracy = decimalAccuracy;
            zstring target = (zstring) value;
            zstring.decimalAccuracy = oldValue;
            return target;
        }

        // null or length equal zero
        public static bool IsNullOrEmpty(zstring str) { return str == null || str.Length == 0; }

        // Does it end with value
        public static bool IsPrefix(zstring str, string value) { return str.StartsWith(value); }

        // Whether to start with value
        public static bool IsPostfix(zstring str, string postfix) { return str.EndsWith(postfix); }

        #endregion
    }
}